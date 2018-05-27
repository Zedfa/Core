using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class ExceptionInfo
    {
        public string Message { get; set; }
        public ExceptionInfo InnerException { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public ExceptionInfo(string message)
        {
            Message = message;
        }

        public ExceptionInfo(Exception excp)
        {
            Message = excp.Message;

            StackTrace = excp.StackTrace;

            Source = excp.Source;

            Data = new Dictionary<string, string>();

            foreach (string key in excp.Data.Keys)
            {
                Data.Add(key, excp.Data[key]?.ToString());
            }
            InnerException = excp.InnerException != null ? new ExceptionInfo(excp.InnerException) : null;

        }
    }
}
