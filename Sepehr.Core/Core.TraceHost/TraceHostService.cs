using Core.Cmn.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.ServiceModel.Description;

namespace Core.TraceHost
{
    public class TraceHostService : ITraceHostService
    {
        private static ServiceHost ServiceHost;
        public static void Start()
        {

            Uri baseAddress = new Uri(Core.Cmn.ConfigHelper.GetConfigValue<string>("TraceServiceUri"));
            Uri metadataAddress = new Uri("mex", UriKind.Relative);
            ServiceHost = new ServiceHost(typeof(TraceHostService), baseAddress);

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


            ServiceHost.AddServiceEndpoint(typeof(ITraceHostService), binding, baseAddress);

            // Add metadata exchange behavior to the service
            ServiceDebugBehavior debug = ServiceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on
            if (debug == null)
            {
                ServiceHost.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
            var serviceBehavior = new ServiceMetadataBehavior();
            ServiceHost.Description.Behaviors.Add(serviceBehavior);

            // Add a service Endpoint for the metadata exchange
            ServiceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), metadataAddress);

            // Run the service
            ServiceHost.Open();

        }

        public void DeleteWriterTraces(DateTime startDate,DateTime endDate, string writer)
        {
            try
            {
                var filteredTraces = ETWRegistrant.TraceList[writer].ToList();

                filteredTraces.RemoveAll(trace => trace.SystemTime >= startDate && trace.SystemTime <= endDate);

                ETWRegistrant.TraceList[writer] = new System.Collections.Concurrent.ConcurrentQueue<TraceDto>(filteredTraces);

                GC.Collect();
            }
            catch (Exception ex)
            {

                registerErrorInTrace(ex);

            }

        }

        public byte[] GetTracesByWriterViaWCF(string filter, string writer)
        {
            List<TraceDto> traceList = new List<TraceDto>();
            try
            {
                traceList = ETWRegistrant.TraceList[writer].Where(filter).ToList();
            }
            catch (Exception ex)
            {
                traceList.Add(registerErrorInTrace(ex));
            }
            byte[] byteTraceList = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(traceList);

            return byteTraceList;
        }

        public byte[] GetTracesViaWCF(string filter)
        {
            List<TraceDto> result = new List<TraceDto>();
            try
            {
                var traceList = ETWRegistrant.TraceList.Values?.Select((item, index) =>
                {
                    result.AddRange(item.Where(filter)?.ToList());
                    return result;
                }).ToList();

            }
            catch (Exception ex)
            {
                result.Add(registerErrorInTrace(ex));
            }

            byte[] byteTraceList = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(result);

            return byteTraceList;
        }

        public byte[] GetTraceWritersViaWCF()
        {
            List<string> writersList = new List<string>();

            try
            {
                writersList = ETWRegistrant.TraceList.Keys.ToList();
            }
            catch (Exception ex)
            {
                registerErrorInTrace(ex);
             
            }

            byte[] byteWritersList = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(writersList);

            return byteWritersList;
        }
        private TraceDto registerErrorInTrace(Exception exception)
        {

            var exeptionData = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in exception.Data)
            {
                exeptionData.Add(item.Key, item.Value);
            }
            TraceDto expTrace = new TraceDto
            {
                Writer = "TraceHost",
                Data = exeptionData,
                Level = 1,
                Message = exception.Message + "\t Details:" + exception?.InnerException?.Message,
                TraceKey = "TraceHostError"
            };

            ETWRegistrant.AddTraceToTraceList(expTrace);

            return expTrace;

        }
    }
}