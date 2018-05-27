using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Trace
{
    [ServiceContract]
    public interface ITraceHostService
    {
        [OperationContract]
        byte[] GetTracesViaWCF(string filter);

        [OperationContract]
        byte[] GetTracesByWriterViaWCF(string filter, string writer);

        [OperationContract]
        byte[] GetTraceWritersViaWCF();

        [OperationContract]
        void DeleteWriterTraces(DateTime startDate, DateTime endDate, string writer);
    }
}
