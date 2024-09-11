using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ServerLibrary.Config;
using ServerLibrary.Messages;
using System.Collections.ObjectModel;

namespace ServerLibrary
{
    public delegate void ServerStatusEventHandler(object sender, ServerStatusEventArgs e);

    public class ProxyServer
    {
        public event ServerStatusEventHandler? OnUpdateServerStatus;

        private List<Topic> LT { get; } = new List<Topic>();
        private Queue<(Message Message, TcpClient Sender)> KKO { get; } = new Queue<(Message, TcpClient)>();
        private ObservableCollection<(Message Message, TcpClient[] Recivers)> KKW { get; } = new ObservableCollection<(Message, TcpClient[])>();

        private readonly Config.Config _config;

        private List<Thread> _serverThreads;
        private Thread _monitorThread;
        private bool _stopServer = false;
        private bool _stopClient = false;

        public ProxyServer()
        {
            _config = ConfigLoader.LoadConfig("config.json");
            KKW.CollectionChanged += SendMessagesQueue_CollectionChanged;

            _serverThreads = new List<Thread>();
            _monitorThread = new Thread(MonitoringThread);
        }

        public void Start()
        {
            _stopClient = false;
            _stopServer = false;

            LT.Clear();
            KKO.Clear();
            KKW.Clear();

            _serverThreads = new List<Thread>();
            IPAddress[] ipAddresses = IpHelper.GetListenIPAddresses(_config.ListenAddresses);
            foreach(IPAddress ip in ipAddresses)
            {
                Thread thread = new Thread(() => ServerCommunicationThread(ip));
                thread.Start();
                _serverThreads.Add(thread);
            }

            _monitorThread = new Thread(MonitoringThread);
            LT.Add(new Topic("logs", _config.ServerID, null));
            OnUpdateServerStatus?.Invoke(this,
                                new ServerStatusEventArgs(UpdateType.TopicsChange,
                                $"Topics changed."));
            _monitorThread.Start();
        }

        private async void ServerCommunicationThread(IPAddress iPAddress)
        {
            TcpListener? listener = null;

            try
            {
                listener = new TcpListener(iPAddress, _config.ListenPort);
                listener.Start();

                OnUpdateServerStatus?.Invoke(this, 
                    new ServerStatusEventArgs(UpdateType.ServerStarted, 
                    $"{iPAddress}:{_config.ListenPort}"));

                while (!_stopServer)
                {
                    try
                    {
                        TcpClient client = await AcceptTcpClientAsync(listener, TimeSpan.FromSeconds(_config.TimeOut));
                        IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

                        OnUpdateServerStatus?.Invoke(this,
                            new ServerStatusEventArgs(UpdateType.Info,
                            $"Client {clientEndPoint?.Address} connected on {iPAddress}:{_config.ListenPort}"));

                        if (IpHelper.IsClientAllowed(clientEndPoint.Address, _config.AllowedIPAddresses))
                        {
                            Thread clientThread = new Thread(() => HandleClient(client));
                            clientThread.Start();
                        }
                        else
                        {
                            OnUpdateServerStatus?.Invoke(this,
                                new ServerStatusEventArgs(UpdateType.Info,
                                $"Client {clientEndPoint.Address} not allowed on {iPAddress}:{_config.ListenPort}. Disconnecting."));
                            client.Close();
                        }
                    }
                    catch (TimeoutException)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                OnUpdateServerStatus?.Invoke(this,
                        new ServerStatusEventArgs(UpdateType.Error,
                        $"Error: {ex.Message}"));
            }
            finally
            {
                listener?.Stop();
                OnUpdateServerStatus?.Invoke(this,
                        new ServerStatusEventArgs(UpdateType.ServerStoped,
                        $"{iPAddress}:{_config.ListenPort}"));
            }
        }

        private async Task<TcpClient> AcceptTcpClientAsync(TcpListener listener, TimeSpan timeout)
        {
            Task<TcpClient> acceptTask = listener.AcceptTcpClientAsync();
            Task delayTask = Task.Delay(timeout);

            Task completedTask = await Task.WhenAny(acceptTask, delayTask);
            if (completedTask == delayTask)
            {
                throw new TimeoutException();
            }

            return await acceptTask;
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                using var stream = client.GetStream();
                stream.ReadTimeout = _config.TimeOut * 1000;

                byte[] buffer = new byte[_config.SizeLimit];
                int bytesRead;
                Message message = new Message();

                while (!_stopClient)
                {
                    try
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }

                        var jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        message = JsonSerializer.Deserialize<Message>(jsonMessage);

                        if (message != null)
                        {
                            KKO.Enqueue((message, client));
                            OnUpdateServerStatus?.Invoke(this,
                                    new ServerStatusEventArgs(UpdateType.Info,
                                    $"Received message and added to KKO."));
                        }
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
                    {
                        continue;
                    }
                    catch (JsonException)
                    {
                        while (stream.DataAvailable)
                        {
                            stream.Read(buffer, 0, buffer.Length);
                        }
                        var rejectMessage = new Message
                        {
                            Type = "reject",
                            Id = _config.ServerID,
                            Topic = "logs",
                            Timestamp = DateTime.UtcNow,
                            Payload = new Dictionary<string, string>
                        {
                            { "timestampOfMessage", message.Timestamp.ToString("o") },
                            { "topicOfMessage", message.Topic },
                            { "success", "false" },
                            { "message", "Message was too long." }
                        }
                        };
                        KKW.Add((rejectMessage, new[] { client }));
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
            finally
            {
                OnUpdateServerStatus?.Invoke(this,
                        new ServerStatusEventArgs(UpdateType.Info,
                        $"Client disconnected."));
                DisconnectClient(client);
            }
        }

        private void DisconnectClient(TcpClient client)
        {
            client.Close();

            lock (LT)
            {
                List<Topic> topicsToRemove = new List<Topic>();
                List<TcpClient> subscribersToDisconnect = new List<TcpClient>();

                foreach (var topic in LT)
                {
                    topic.Subscribers.RemoveAll(subscriber => subscriber == client);

                    if (topic.Producer == client)
                    {
                        topicsToRemove.Add(topic);
                    }
                }

                foreach (var topic in topicsToRemove)
                {
                    LT.Remove(topic);

                    OnUpdateServerStatus?.Invoke(this,
                        new ServerStatusEventArgs(UpdateType.TopicsChange,
                        $"Topics changed."));

                    KKW.Add((new Message
                    {
                        Type = "acknowledge",
                        Id = _config.ServerID,
                        Topic = "logs",
                        Timestamp = DateTime.UtcNow,
                        Payload = new Dictionary<string, string>
                        {
                            { "timestampOfMessage", DateTime.UtcNow.ToString("o") },
                            { "topicOfMessage", topic.Name },
                            { "success", "true" },
                            { "message", "Producer withdrew the topic." }
                        }
                    }, topic.Subscribers.ToArray()));

                    foreach (var subscriber in topic.Subscribers)
                    {

                        if (!LT.Any(t => t.Subscribers.Contains(subscriber) || t.Producer == subscriber))
                        {
                            subscriber.Close();
                        }
                    }
                }
            }
        }

        private void MonitoringThread()
        {
            while (!_stopServer)
            {
                if (KKO.Count == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                var (message, client) = KKO.Dequeue();

                var validationResult = MessageValidator.Validate(message);
                if (!validationResult.IsValid)
                {
                    OnUpdateServerStatus?.Invoke(this,
                                new ServerStatusEventArgs(UpdateType.Info,
                                $"Invalid message: {validationResult.ErrorMessage}"));
                    var rejectMessage = new Message
                    {
                        Type = "reject",
                        Id = _config.ServerID,
                        Topic = "logs",
                        Timestamp = DateTime.UtcNow,
                        Payload = new Dictionary<string, string>
                        {
                            { "timestampOfMessage", message.Timestamp.ToString("o") },
                            { "topicOfMessage", message.Topic },
                            { "success", "false" },
                            { "message", validationResult.ErrorMessage }
                        }
                    };
                    KKW.Add((rejectMessage, new[] { client }));
                }
                else
                {
                    MessageHandler messageHandler = new MessageHandler(LT, KKW, _config);
                    messageHandler.HandleMessage(message, client);
                    OnUpdateServerStatus?.Invoke(this,
                                new ServerStatusEventArgs(UpdateType.TopicsChange,
                                $"Topics changed."));
                }
            }
        }

        public string GetTopicsInfo()
        {
            var sb = new StringBuilder();
            lock (LT)
            {
                foreach (var topic in LT)
                {
                    sb.AppendLine($"Topic: {topic.Name}");
                    sb.AppendLine($"  Producer: {topic.Producer?.Client.RemoteEndPoint}");
                    sb.AppendLine($"  Subscribers: {topic.Subscribers.Count}");
                    foreach (var subscriber in topic.Subscribers)
                    {
                        sb.AppendLine($"    - {subscriber.Client.RemoteEndPoint}");
                    }
                }
            }
            return sb.ToString();
        }

        public void Stop()
        {
            _stopClient = true;
            _stopServer = true;
            foreach(var thread in _serverThreads)
            {
                thread.Join();
            }
            if(_monitorThread.ThreadState == ThreadState.Running)
            {
                _monitorThread?.Join();
            }
            OnUpdateServerStatus?.Invoke(this,
                    new ServerStatusEventArgs(UpdateType.ServerStoped,
                    $"*"));
        }

        private void SendMessagesQueue_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    var (message, clients) = (ValueTuple<Message, TcpClient[]>)newItem;
                    foreach (var client in clients)
                    {
                        try
                        {
                            var jsonMessage = JsonSerializer.Serialize(message);
                            var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                            client.GetStream().Write(buffer, 0, buffer.Length);
                        }
                        catch (Exception ex)
                        {
                            OnUpdateServerStatus?.Invoke(this,
                                        new ServerStatusEventArgs(UpdateType.Error,
                                        $"Error sending message to client: {ex.Message}"));
                        }
                    }
                }
            }
        }
    }
}
