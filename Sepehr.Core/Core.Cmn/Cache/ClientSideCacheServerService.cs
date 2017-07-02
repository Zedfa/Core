namespace Core.Cmn.Cache
{
    using Core.Cmn.Cache;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    [ServiceKnownType("GetKnownTypes", typeof(CacheWCFTypeHelper))]
    [ServiceContract()]
    public interface IServerSideCacheServerService
    {

        [System.ServiceModel.OperationContractAttribute()]

        object GetCacheDataViaWcf(ICacheDataProvider cacheDataProvider);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IServiceChannel : IServerSideCacheServerService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<IServerSideCacheServerService>, IServerSideCacheServerService
    {

        public ServiceClient()
        {
        }

        public ServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public ServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public object GetCacheDataViaWcf(ICacheDataProvider cacheDataProvider)
        {
            return base.Channel.GetCacheDataViaWcf( cacheDataProvider);
        }

    }
}
