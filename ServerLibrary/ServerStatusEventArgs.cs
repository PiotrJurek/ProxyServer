using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class ServerStatusEventArgs
    {
        public UpdateType UpdateType { get; }
        public string Message { get; } = string.Empty;

        public ServerStatusEventArgs(UpdateType updateType, string message)
        {
            UpdateType = updateType;
            Message = message;
        }
    }
}
