using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ClientLibrary
{
    public delegate void RecievedMessageHandler(string message);

    public class ClientApi
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private Config? _serverConfig;
        private string _clientID = "";
        private string _serverIp = "";
        private int _serverPort;
        private bool _connected;
        private Thread? _listenerThread;
        private Action<Dictionary<string, string>>? _serverStatusCallback = null;
        private Action<bool, string>? _serverLogsCallback = null;
        private Dictionary<string, Action<string, Dictionary<string, string>>?> _subscriberCallbacks = new Dictionary<string, Action<string, Dictionary<string, string>>?>();
        private List<string> _producedTopics = new List<string>();
        private List<Message> _sendedMessages = new List<Message>();

        public void Start(string serverIp, int serverPort, string clientID)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
            _clientID = clientID;

            try
            {
                _client = new TcpClient();
                _client.Connect(_serverIp, _serverPort);
                _stream = _client.GetStream();
                _connected = true;

                _listenerThread = new Thread(ListenForMessages);
                _listenerThread.Start();

                SendConfigRequest();
            }
            catch (Exception)
            {
                _connected = false;
                throw;
            }
        }

        public bool IsConnected()
        {
            return _connected;
        }

        public string GetStatus()
        {
            var status = new
            {
                ProducedTopics = _producedTopics,
                SubscribedTopics = _subscriberCallbacks.Keys.ToList(),
            };

            return JsonSerializer.Serialize(status);
        }

        public void GetServerStatus(Action<Dictionary<string, string>>? callback)
        {
            _serverStatusCallback = callback;
            var message = CreateMessage("status", "logs", "", new Dictionary<string, string>());
            SendMessage(message);
        }

        public void GetServerLogs(Action<bool, string>? callback)
        {
            _serverLogsCallback = callback;
        }

        public void CreateProducer(string topicName)
        {
            var message = CreateMessage("register", topicName, "producer", new Dictionary<string, string>());
            SendMessage(message);
        }

        public void Produce(string topicName, Dictionary<string, string> payload)
        {
            var message = CreateMessage("message", topicName, "", payload);
            SendMessage(message);
        }

        public void SendFile(string topicName, string path2File)
        {
            var content = Convert.ToBase64String(System.IO.File.ReadAllBytes(path2File));
            var info = new System.IO.FileInfo(path2File);

            var payload = new Dictionary<string, string>
            {
                {"fileName", info.Name },
                {"fileContent", content }
            };

            var message = CreateMessage("file", topicName, "", payload);

            SendMessage(message);
        }

        public void WithdrawProducer(string topicName)
        {
            var message = CreateMessage("withdraw", topicName, "producer", new Dictionary<string, string>());
            SendMessage(message);
        }

        public void CreateSubscriber(string topicName, Action<string, Dictionary<string, string>>? callback)
        {
            if (!_subscriberCallbacks.ContainsKey(topicName))
            {
                _subscriberCallbacks.Add(topicName, callback);
            }
            var message = CreateMessage("register", topicName, "subscriber", new Dictionary<string, string>());
            SendMessage(message);
        }

        public void WithdrawSubscriber(string topicName)
        {
            var message = CreateMessage("withdraw", topicName, "subscriber", new Dictionary<string, string>());
            SendMessage(message);
        }

        public void Stop()
        {
            _connected = false;
            _stream?.Dispose();
            _listenerThread?.Join();
            _stream?.Close();
            _client?.Close();
            _serverStatusCallback = null;
            _subscriberCallbacks.Clear();
            _producedTopics.Clear();
            _serverConfig = null;
            _serverLogsCallback = null;
            _sendedMessages.Clear();
        }

        private void ListenForMessages()
        {
            try
            {
                while (_connected)
                {
                    byte[] buffer = new byte[_serverConfig == null ? 8192 : _serverConfig.SizeLimit];
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Message? message =  JsonSerializer.Deserialize<Message>(response);
                    Message? sendedMessage;
                    switch (message?.Type)
                    {
                        case "config":
                            _serverConfig = JsonSerializer.Deserialize<Config>(message.Payload["config"]);
                            break;
                        case "reject":
                            sendedMessage = _sendedMessages.FirstOrDefault(m => m.Timestamp.ToString("o") == message.Payload["timestampOfMessage"]);
                            if(sendedMessage != null)
                            {
                                if(sendedMessage.Type == "register" && sendedMessage.Mode == "subscriber" && message.Payload["message"] != "Topic already subscribed by you.")
                                {
                                    _subscriberCallbacks.Remove(sendedMessage.Topic);
                                }
                                _sendedMessages.Remove(sendedMessage);
                            }
                            _serverLogsCallback?.Invoke(false, message.Payload["message"]);
                            break;
                        case "acknowledge":
                            if(message.Payload["message"] == "Producer withdrew the topic.")
                            {
                                _subscriberCallbacks.Remove(message.Payload["topicOfMessage"]);
                            }
                            sendedMessage = _sendedMessages.FirstOrDefault(m => m.Timestamp.ToString("o") == message.Payload["timestampOfMessage"]);
                            if (sendedMessage != null)
                            {
                                if (sendedMessage.Type == "register" && sendedMessage.Mode == "producer")
                                {
                                    _producedTopics.Add(sendedMessage.Topic);
                                }
                                else if (sendedMessage.Type == "withdraw" && sendedMessage.Mode == "producer")
                                {
                                    _producedTopics.Remove(sendedMessage.Topic);
                                }
                                else if (sendedMessage.Type == "withdraw" && sendedMessage.Mode == "subscriber")
                                {
                                    _subscriberCallbacks.Remove(sendedMessage.Topic);
                                }
                                _sendedMessages.Remove(sendedMessage);
                            }
                            _serverLogsCallback?.Invoke(true, message.Payload["message"]);
                            break;
                        case "message":
                        case "file":
                            _subscriberCallbacks[message.Topic]?.Invoke(message.Type, message.Payload);
                            break;
                        case "status":
                            _serverStatusCallback?.Invoke(message.Payload);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                if(_connected)
                {
                    _connected = false;
                    _stream?.Close();
                    _client?.Close();
                    _serverStatusCallback = null;
                    _subscriberCallbacks.Clear();
                    _producedTopics.Clear();
                    _serverConfig = null;
                    _serverLogsCallback = null;
                    _sendedMessages.Clear();
                }
            }
        }

        private void SendConfigRequest()
        {
            var message = CreateMessage("config", "logs", "", new Dictionary<string, string>());
            SendMessage(message);
        }

        private Message CreateMessage(string type, string topic, string mode, Dictionary<string, string> payload)
        {
            return new Message
            {
                Type = type,
                Id = _clientID,
                Topic = topic,
                Mode = mode,
                Timestamp = DateTime.UtcNow,
                Payload = payload
            };
        }

        private void SendMessage(Message message)
        {
            if (_connected && _stream != null)
            {
                var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Message>(message));
                if(_serverConfig != null && data.Length > _serverConfig.SizeLimit)
                {
                    throw new ArgumentOutOfRangeException("Message it too long for server size limit.");
                }
                if(message.Type != "config" && message.Type != "status")
                {
                    _sendedMessages.Add(message);
                }
                _stream.Write(data, 0, data.Length);
            }
        }
    }
}
