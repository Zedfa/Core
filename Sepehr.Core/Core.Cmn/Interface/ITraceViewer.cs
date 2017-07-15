using System;

namespace Core.Cmn
{
    public interface ITraceViewer
    {
        string Name { get; }
        void Inform(string message, string traceKey = "");
        void Attention(string message, string traceKey = "");
        void Failure(string message, string traceKey = "");
    }




}
