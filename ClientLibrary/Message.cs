using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary
{
    public class Message
    {
        public string Type { get; set; } = "";
        public string Id { get; set; } = "";
        public string Topic { get; set; } = "";
        public string Mode { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> Payload { get; set; }

        public Message()
        {
            Payload = new Dictionary<string, string>();
        }
    }
}
