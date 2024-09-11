using ServerLibrary.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerLibrary.Messages
{
    internal class MessageHandler
    {
        private List<Topic> LT { get; }
        private ObservableCollection<(Message Message, TcpClient[] Recivers)> KKW { get; }

        private readonly Config.Config _config;

        public MessageHandler(List<Topic> LT, ObservableCollection<(Message Message, TcpClient[] Recivers)> KKW, Config.Config config)
        {
            this.LT = LT;
            this.KKW = KKW;
            _config = config;
        }

        public void HandleMessage(Message message, TcpClient client)
        {
            switch (message.Type)
            {
                case "register":
                    HandleRegisterMessage(message, client);
                    break;
                case "withdraw":
                    HandleWithdrawMessage(message, client);
                    break;
                case "message":
                case "file":
                    HandleMessageOrFile(message, client);
                    break;
                case "config":
                    HandleConfigMessage(message, client);
                    break;
                case "status":
                    HandleStatusMessage(message, client);
                    break;
                default:
                    SendRejectMessage(client, message, "Unknown message type.");
                    break;
            }
        }

        private void HandleRegisterMessage(Message message, TcpClient client)
        {
            lock (LT)
            {
                var topic = LT.FirstOrDefault(t => t.Name == message.Topic);

                if (message.Mode == "producer")
                {
                    if (topic == null)
                    {
                        LT.Add(new Topic(message.Topic, message.Id ,client));
                        SendAcknowledgeMessage(client, message, "OK");
                    }
                    else if (topic.Producer != client)
                    {
                        SendRejectMessage(client, message, "Topic already registered by another producer.");
                    }
                    else
                    {
                        SendRejectMessage(client, message, "Topic already registered by you.");
                    }
                }
                else if (message.Mode == "subscriber")
                {
                    if (topic != null)
                    {
                        if (!topic.Subscribers.Contains(client))
                        {
                            topic.Subscribers.Add(client);
                            SendAcknowledgeMessage(client, message, "OK");
                        }
                        else
                        {
                            SendRejectMessage(client, message, "Topic already subscribed by you.");
                        }
                    }
                    else
                    {
                        SendRejectMessage(client, message, "No such topic registered.");
                    }
                }
            }
        }

        private void HandleWithdrawMessage(Message message, TcpClient client)
        {
            lock (LT)
            {
                var topic = LT.FirstOrDefault(t => t.Name == message.Topic);

                if (topic == null)
                {
                    SendRejectMessage(client, message, "No such topic registered.");
                    return;
                }

                if (message.Mode == "producer")
                {
                    if(topic.Producer == client)
                    {
                        LT.Remove(topic);
                        var clients = topic.Subscribers.ToArray();
                        SendAcknowledgeMessage(client, message, "Producer withdrew the topic.");
                        KKW.Add((new Message
                        {
                            Type = "acknowledge",
                            Id = _config.ServerID,
                            Topic = "logs",
                            Timestamp = DateTime.UtcNow,
                            Payload = new Dictionary<string, string>
                        {
                            { "timestampOfMessage", message.Timestamp.ToString("o") },
                            { "topicOfMessage", message.Topic },
                            { "success", "true" },
                            { "message", "Producer withdrew the topic." }
                        }
                        }, clients));
                    }
                    else
                    {
                        SendRejectMessage(client, message, "You are not producer of this topic.");
                    }
                }
                else if (message.Mode == "subscriber")
                {
                    if (topic.Subscribers.Contains(client))
                    {
                        topic.Subscribers.Remove(client);
                        SendAcknowledgeMessage(client, message, "Subscription withdrawn.");
                    }
                    else
                    {
                        SendRejectMessage(client, message, "No subscription to this topic.");
                    }
                }
            }
        }

        private void HandleMessageOrFile(Message message, TcpClient client)
        {
            lock (LT)
            {
                var topic = LT.FirstOrDefault(t => t.Name == message.Topic);

                if (topic == null)
                {
                    SendRejectMessage(client, message, "No such topic registered.");
                    return;
                }
                else if (topic.Producer != client)
                {
                    SendRejectMessage(client, message, "You are not producer of this topic.");
                    return;
                }
                else if (topic.Subscribers.Count == 0)
                {
                    SendRejectMessage(client, message, "Topic has no subscribers.");
                    return;
                }

                var clients = topic.Subscribers.ToArray();
                KKW.Add((message, clients));
                SendAcknowledgeMessage(client, message, $"Message resent to {clients.Length} subscribers.");
            }
        }

        private void HandleConfigMessage(Message message, TcpClient client)
        {
            var configJson = JsonSerializer.Serialize(_config);
            var configMessage = new Message
            {
                Type = "config",
                Id = _config.ServerID,
                Topic = "logs",
                Timestamp = DateTime.UtcNow,
                Payload = new Dictionary<string, string>
                {
                    { "config", configJson }
                }
            };

            KKW.Add((configMessage, new[] { client }));
        }

        private void HandleStatusMessage(Message message, TcpClient client)
        {
            var topicsStatus = string.Join(", ", LT.Select(t => $"{t.Name} (Producer: {t.ProducerID}, Subscribers: {t.Subscribers.Count})"));
            var statusMessage = new Message
            {
                Type = "status",
                Id = _config.ServerID,
                Topic = "logs",
                Timestamp = DateTime.UtcNow,
                Payload = new Dictionary<string, string>
                {
                    { "status", topicsStatus }
                }
            };

            KKW.Add((statusMessage, new[] { client }));
        }

        private void SendAcknowledgeMessage(TcpClient client, Message originalMessage, string message)
        {
            var ackMessage = new Message
            {
                Type = "acknowledge",
                Id = _config.ServerID,
                Topic = "logs",
                Timestamp = DateTime.UtcNow,
                Payload = new Dictionary<string, string>
                {
                    { "timestampOfMessage", originalMessage.Timestamp.ToString("o") },
                    { "topicOfMessage", originalMessage.Topic },
                    { "success", "true" },
                    { "message", message }
                }
            };

            KKW.Add((ackMessage, new[] { client }));
        }

        private void SendRejectMessage(TcpClient client, Message originalMessage, string errorMessage)
        {
            var rejectMessage = new Message
            {
                Type = "reject",
                Id = _config.ServerID,
                Topic = "logs",
                Timestamp = DateTime.UtcNow,
                Payload = new Dictionary<string, string>
                {
                    { "timestampOfMessage", originalMessage.Timestamp.ToString("o") },
                    { "topicOfMessage", originalMessage.Topic },
                    { "success", "false" },
                    { "message", errorMessage }
                }
            };

            KKW.Add((rejectMessage, new[] { client }));
        }
    }
}
