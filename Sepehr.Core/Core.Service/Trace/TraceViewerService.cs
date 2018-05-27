using Core.Cmn.Attributes;
using System;
using Core.Cmn;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Core.Cmn.Trace;
using System.ServiceModel;

namespace Core.Service.Trace
{
   
    [Injectable(InterfaceType = typeof(ITraceViewer), DomainName = "Core", LifeTime = LifetimeManagement.ContainerControlledLifetime)]
    public sealed class TraceViewerService :  ITraceViewer
    {
        private static int _countOfInstances;
        private static object _justForLock = new object();
        
   
        private string EndpointUri { get { return ConfigHelper.GetConfigValue<string>(GeneralConstant.TraceServiceUri); } }
      
        public TraceViewerService()
        {
            lock (_justForLock)
            {
                if (_countOfInstances > 0)
                {
                    throw new NotSupportedException(@"TraceViewerService is a singleton instance class and you can't make more than one instance.
                                                      \n Best practice for instantiate of this class is using of Core.Cmn.AppBase.TraceWriter property.");
                }
                else
                {
                    _countOfInstances++;
                  
                }
            }
        }



        #region read from wcf service
      
        public List<TraceDto> GetTracesViaWCF(string filter)
        {
            if (string.IsNullOrEmpty(EndpointUri))
                return null;
            EndpointAddress endpointAddress = new EndpointAddress(EndpointUri);//("net.tcp://localhost:65000/TraceService");
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.CloseTimeout = new TimeSpan(0, 2, 0);
            binding.OpenTimeout = new TimeSpan(0, 2, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            List<TraceDto> result = null;

            using (TraceProxyService proxy = new TraceProxyService(binding, endpointAddress))
            {

                try
                {
                    var byteTraceList = proxy.GetTracesViaWCF(filter);
                    result = Core.Cmn.Extensions.SerializationExtensions.DeSerializeBinaryToObject<List<TraceDto>>(byteTraceList); //Core.Serialization.BinaryConverter.Deserialize<List<Source>>(proxy.GetTracesViaWCF(filter));

                    proxy.Close();

                }
                catch (FaultException exc)
                {
                    if (exc is System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>)
                    {

                        proxy.Abort();

                    }
                    else
                    {

                        proxy.Abort();

                    }
                }
                catch (Exception ex)
                {
                    proxy.Abort();
                }
            }
            return result;
        }

        public List<TraceDto> GetTracesByWriterViaWCF(string filter, string writer)
        {
            if (string.IsNullOrEmpty(EndpointUri))
                return null;
            EndpointAddress endpointAddress = new EndpointAddress(EndpointUri);//("net.tcp://localhost:65000/TraceService");
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.CloseTimeout = new TimeSpan(0, 2, 0);
            binding.OpenTimeout = new TimeSpan(0, 2, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            List<TraceDto> result = null;

            using (TraceProxyService proxy = new TraceProxyService(binding, endpointAddress))
            {

                try
                {
                    var byteTraceList = proxy.GetTracesByWriterViaWCF(filter, writer);
                    result = Core.Cmn.Extensions.SerializationExtensions.DeSerializeBinaryToObject<List<TraceDto>>(byteTraceList); //Core.Serialization.BinaryConverter.Deserialize<List<Source>>(proxy.GetTracesViaWCF(filter));

                    proxy.Close();

                }
                catch (FaultException exc)
                {
                    if (exc is System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>)
                    {

                        proxy.Abort();

                    }
                    else
                    {

                        proxy.Abort();

                    }
                }
                catch (Exception ex)
                {
                    proxy.Abort();
                }
            }
            return result;
        }
        public List<string> GetTraceWritersViaWCF()
        {
            if (string.IsNullOrEmpty(EndpointUri))
                return null;

            EndpointAddress endpointAddress = new EndpointAddress(EndpointUri);
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.CloseTimeout = new TimeSpan(0, 2, 0);
            binding.OpenTimeout = new TimeSpan(0, 2, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            List<string> result = null;

            using (TraceProxyService proxy = new TraceProxyService(binding, endpointAddress))
            {

                try
                {
                    var byteWriterList = proxy.GetTraceWritersViaWCF();
                    result = Core.Cmn.Extensions.SerializationExtensions.DeSerializeBinaryToObject<List<string>>(byteWriterList); //Core.cmn.BinaryConverter.Deserialize<List<string>>(proxy.GetTraceWritersViaWCF());

                    proxy.Close();

                }
                catch (FaultException exc)
                {
                    if (exc is System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>)
                    {

                        proxy.Abort();

                    }
                    else
                    {

                        proxy.Abort();

                    }
                }
                catch (Exception ex)
                {
                    proxy.Abort();
                }
            }
            return result;
        }

        public void DeleteWriterTraces(DateTime startDate, DateTime endDate, string writer)
        {
            if (string.IsNullOrEmpty(EndpointUri))
                return ;

            EndpointAddress endpointAddress = new EndpointAddress(EndpointUri);
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.CloseTimeout = new TimeSpan(0, 2, 0);
            binding.OpenTimeout = new TimeSpan(0, 2, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

          

            using (TraceProxyService proxy = new TraceProxyService(binding, endpointAddress))
            {

                try
                {
                   proxy.DeleteWriterTraces(startDate,endDate,writer);

                    proxy.Close();

                }
                catch (FaultException exc)
                {
                    if (exc is System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>)
                    {

                        proxy.Abort();

                    }
                    else
                    {

                        proxy.Abort();

                    }
                }
                catch (Exception ex)
                {
                    proxy.Abort();
                }
            }
          

        }


            #endregion


        }

}

