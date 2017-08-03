using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class LogInfo
    {
        public LogInfo(string source) {
            ApplicationName = ConfigHelper.GetConfigValue<string>("ApplicationNameForLog");
            Source = source;

        }

        public LogInfo([CallerFilePath] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            ApplicationName = ConfigHelper.GetConfigValue<string>("ApplicationNameForLog");
            //IP  = AppBase.Request.IP;
            Source = $"File: {file} , Method: {method} , Line: {line}";
        }
        
        public Exception OccuredException { get; set; }
        public string CustomMessage { get; set; }

        public string Source { get; private set; }
        public bool RaiseThrowException { get; set; }
        public string Platform { get; set; }

        public string IP { get; set; }

        public string ApplicationName { get; private set; }

       

    }
}
