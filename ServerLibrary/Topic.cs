using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class Topic
    {
        public string Name { get; set; }
        public string ProducerID { get; set; }
        public TcpClient? Producer { get; set; }
        public List<TcpClient> Subscribers { get; set; } = new List<TcpClient>();

        public Topic(string name, string producerID, TcpClient? producer)
        {
            Name = name;
            ProducerID = producerID;
            Producer = producer;
        }
    }
}
