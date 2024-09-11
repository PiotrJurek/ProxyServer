using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public enum UpdateType
    {
        Info,
        Error,
        ServerStarted,
        ServerStoped,
        TopicsChange
    }
}
