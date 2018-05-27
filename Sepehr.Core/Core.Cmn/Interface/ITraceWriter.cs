using Core.Cmn.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface ITraceWriter
    {
        string AppName { get;  }
        void Inform(string message, string traceKey = "");
        void Attention(string message, string traceKey = "");
        void Failure(string message, string traceKey = "");
        void Exception(Exception exception, string traceKey = "");
        void SubmitData(TraceDto source);
    }
}
