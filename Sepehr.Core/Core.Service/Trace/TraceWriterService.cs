using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Trace;
using Core.Entity;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Core.Service.Trace
{
    [EventSource(Name = "TraceWriterService")]
    [Injectable(InterfaceType = typeof(ITraceWriter), DomainName = "Core", LifeTime = LifetimeManagement.ContainerControlledLifetime)]
    public sealed class TraceWriterService : EventSource, ITraceWriter
    {
        private static int _countOfInstances;
        private static object _justForLock = new object();
        private IConstantService _constanService;

        public TraceWriterService()
        {
            lock (_justForLock)
            {
                if (_countOfInstances > 0)
                {
                    throw new NotSupportedException(@"TraceWriterService is a singleton instance class and you can't make more than one instance.
                                                      \n Best practice for instantiate of this class is using of Core.Cmn.AppBase.TraceWriter property.");
                }
                else
                {
                    _countOfInstances++;

                    _constanService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();

                    var appName = AppDomain.CurrentDomain.RelativeSearchPath?? AppDomain.CurrentDomain.BaseDirectory;
                    var userName = (System.Security.Principal.WindowsIdentity.GetCurrent()).Name;
                    AppName = $"{userName} | {appName}";
                }
            }
        }

        public string AppName
        {
            get;
        }

        #region warning

        [NonEvent]
        public void Attention(string message, string traceKey = "")
        {
            if (IsTraceEnabled(traceKey))
                this.Attention(message, AppName, traceKey);
        }

        [Event(3, Message = "{0}", Level = EventLevel.Warning, Channel = EventChannel.Admin)]
        private void Attention(string message, string writer, string traceKey = "")
        {
            WriteEvent(3, message, writer, traceKey);
        }

        #endregion warning

        #region error

        [NonEvent]
        public void Failure(string message, string traceKey = "")
        {
            if (IsTraceEnabled(traceKey))
                this.Failure(message, AppName, traceKey);
        }

        [Event(1, Message = "{0}", Level = EventLevel.Error, Channel = EventChannel.Admin)]
        private void Failure(string message, string writer, string traceKey = "")
        {
            WriteEvent(1, message, writer, traceKey);
        }

        #endregion error

        #region information

        [NonEvent]
        public void Inform(string message, string traceKey = "")
        {
            if (IsTraceEnabled(traceKey))
                this.Inform(message, AppName, traceKey);
        }

        [Event(2, Message = "{0}", Level = EventLevel.Informational, Channel = EventChannel.Admin)]
        private void Inform(string message, string writer, string traceKey = "")
        {
            WriteEvent(2, message, writer, traceKey);
        }

        #endregion information

        #region exception

        [NonEvent]
        public void Exception(Exception exception, string traceKey = "")
        {
          //  if (IsTraceEnabled(traceKey))
          //  {
                //var data = GetDataFromException(exception, null);

                //var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                //var message = Newtonsoft.Json.JsonConvert.SerializeObject(exception);
                //this.Exception(message, this.AppName, serializedData, traceKey);
                //
                var customException = new ExceptionInfo(exception);

                var trace = new TraceDto
                {
                    Data = GetDataFromException(customException, null),
                    Message = Newtonsoft.Json.JsonConvert.SerializeObject(customException),
                    Writer = AppName,
                    TraceKey = traceKey
                };

                this.Exception(trace.Message, trace.Writer, trace.SerializedData, trace.TraceKey);
           // }
        }

        [Event(4, Message = "{0}", Level = EventLevel.Critical, Channel = EventChannel.Admin)]
        private void Exception(string message, string writer, string data, string traceKey)
        {
            WriteEvent(4, message, writer, data, traceKey);
        }

        [NonEvent]
        private Dictionary<string, string> GetDataFromException(ExceptionInfo exception, Dictionary<string, string> data)
        {
            data = data ?? new Dictionary<string, string>();

            if (exception.Data != null)
            {
                foreach (KeyValuePair<string, string> item in exception.Data)
                {
                    data.Add(item.Key, item.Value);
                }

                if (exception.InnerException != null)

                    GetDataFromException(exception.InnerException, data);
            }
            return data;
        }

        #endregion exception

        #region SubmitData

        [NonEvent]
        public void SubmitData(TraceDto trace)
        { 
            if (trace == null || trace.Data == null || string.IsNullOrEmpty(trace.Message))
                throw new System.Exception("one or more property (like Data or Message) in 'Source' should be filled ");
            else if (IsTraceEnabled(trace.TraceKey))
            {
                trace.Writer = AppName;

                this.SubmitData(trace.Message, trace.Writer, trace.SerializedData, string.IsNullOrEmpty(trace.TraceKey) ? "GeneralTraceKey" : trace.TraceKey);
            }
        }

        [Event(5, Message = "{0}", Level = EventLevel.Informational, Channel = EventChannel.Admin)]
        private void SubmitData(string message, string writer, string data, string traceKey)
        {
            WriteEvent(5, message, writer, data, traceKey);
        }

        #endregion SubmitData

        [NonEvent]
        private bool IsTraceEnabled(string traceKey)
        {
            var result = true;

            // age key be method dade beshe va dar database ham in key vojod dashte bashad , value ra set mikonad(enable ya disable) . dar gheyre in sorat hamishe enable ast;

            Constant constant;
            if (!string.IsNullOrEmpty(traceKey) && _constanService.GetAllTraceKey().TryGetValue(traceKey, out constant))
            {
                //1. baraye bala bordane performance az convert estefade nakardam
                //2. value ro tolower nakardam choon saritar kar kone hatman too db kochik bayad bashad
                result = constant.Value == "true";
            }

            return result;
        }
    }
}