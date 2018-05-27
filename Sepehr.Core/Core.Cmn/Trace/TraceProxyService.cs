using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Trace
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IServiceChannel : ITraceHostService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]

    public partial class TraceProxyService : System.ServiceModel.ClientBase<ITraceHostService>, ITraceHostService
    {

        public TraceProxyService()
        {
        }

        public TraceProxyService(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public TraceProxyService(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public TraceProxyService(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public TraceProxyService(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public byte[] GetTracesViaWCF(string filter)
        {
            return base.Channel.GetTracesViaWCF(filter);
        }
        public byte[] GetTracesByWriterViaWCF(string filter, string writer)
        {
            return base.Channel.GetTracesByWriterViaWCF(filter, writer);
        }
        public byte[] GetTraceWritersViaWCF()
        {
            return base.Channel.GetTraceWritersViaWCF();
        }
        public void DeleteWriterTraces(DateTime startDate, DateTime endDate, string writer)
        {   
            base.Channel.DeleteWriterTraces(startDate, endDate, writer);

        }
    }

}

