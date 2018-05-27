using Core.Cmn.Trace;
using System;
using System.Collections.Generic;

namespace Core.Cmn
{
    public interface ITraceViewer
    {
        List<TraceDto> GetTracesViaWCF(string filter);
        List<TraceDto> GetTracesByWriterViaWCF(string filter, string writer);
        List<string> GetTraceWritersViaWCF();
        void DeleteWriterTraces(DateTime startDate, DateTime endDate, string writer);
    }




}
