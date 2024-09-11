using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary
{
    public class Config
    {
        public string ServerID { get; set; } = "";
        public string[] ListenAddresses { get; set; } = new string[0];
        public int ListenPort { get; set; }
        public int TimeOut { get; set; }
        public int SizeLimit { get; set; }
        public string[] AllowedIPAddresses { get; set; } = new string[0];
    }
}
