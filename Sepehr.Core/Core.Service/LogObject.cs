using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Service
{
    class LogObject
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string ExceptionType { get; set; }
    }

    public class LogParametersInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

}
