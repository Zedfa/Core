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
        public string Details { get; set; }
        public string StackTrace { get; set; }
        public bool IsRTL { get; set; }

        public string Source { get; set; }

        public ExceptionInfo(string message, bool isRTL = true)
        {
            this.Message = message;
            this.IsRTL = isRTL;
        }
        public ExceptionInfo(Exception excp, bool isRTL = true)
        {
            this.Message = excp.Message;
            this.Details = excp.InnerException != null ? excp.InnerException.Message : string.Empty;
            this.StackTrace = excp.StackTrace;
            this.IsRTL = isRTL;
            Source = excp.Source;
        }
    }
}
