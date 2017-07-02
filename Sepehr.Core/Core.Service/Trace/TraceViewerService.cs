using Core.Cmn.Attributes;
using System;
using Core.Cmn;
using Microsoft.Diagnostics.Tracing;
using System.Linq;


namespace Core.Service
{
    [EventSource(Name = "TraceViewerService")]
    [Injectable(InterfaceType = typeof(ITraceViewer), DomainName = "Core", LifeTime = LifetimeManagement.ContainerControlledLifetime)]
    public sealed class TraceViewerService : EventSource, ITraceViewer
    {
        private static object _justForLock = new object();
        private static int _countOfInstances;

        string ITraceViewer.Name
        {
            get
            {
                return this.Name;
            }

        }

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
        private string CreateTraceKey(string key)
        {
            return $"{Core.Cmn.GeneralConstant.PayLoadKey}_{key}";
        }

        [Event(1, Keywords = Keywords.Requests, Message = "Start processing request\n\t*** {0} ***\nfor URL\n\t=== {1} ===", Channel = EventChannel.Admin, Task = Tasks.Request, Opcode = EventOpcode.Start)]
        public void RequestStart( int RequestID, string Url, string traceKey = "")
        {
            WriteEvent(1, RequestID, Url, CreateTraceKey(traceKey));
        }
        [Event(2 , Keywords = Keywords.Requests, Channel = EventChannel.Analytic, Message = "Entering Phase {1} for request {0}", Task = Tasks.Request, Opcode = EventOpcode.Info, Level = EventLevel.Verbose)]
        public void RequestPhase( int RequestID, string PhaseName, string traceKey = "")
        {
            WriteEvent(2, RequestID, PhaseName, CreateTraceKey(traceKey));

        }
        [Event(3, Keywords = Keywords.Requests, Message = "Stop processing request\n\t*** {0} ***", Channel = EventChannel.Admin, Task = Tasks.Request, Opcode = EventOpcode.Stop)]
        public void RequestStop( int RequestID, string traceKey = "")
        {
            WriteEvent(3, RequestID, CreateTraceKey(traceKey));
        }
        [Event(4, Message = "{0}", Keywords = Keywords.Debug, Channel = EventChannel.Debug)]
        public void DebugTrace( string message, string traceKey = "", EventLogEntryType level = EventLogEntryType.Information)
        {
            WriteEvent(4, message, CreateTraceKey(traceKey), level);
        }


        #region Keywords / Tasks / Opcodes

        /// <summary>
        /// By defining keywords, we can turn on events independently.   Because we defined the 'Request'
        /// and 'Debug' keywords and assigned the 'Request' keywords to the first three events, these 
        /// can be turned on and off by setting this bit when you enable the EventSource.   Similarly
        /// the 'Debug' event can be turned on and off independently.  
        /// </summary>
        public class Keywords   // This is a bitvector
        {
            public const EventKeywords Requests = (EventKeywords)0x0001;
            public const EventKeywords Debug = (EventKeywords)0x0002;
        }

        public class Tasks
        {
            public const EventTask Request = (EventTask)0x1;
        }

        #endregion

    }

    public class TraceViewerEventListener : EventListener
    {
        private IConstantService _constanService;
        public TraceViewerEventListener()
        {
            _constanService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // For any EventSource, turn it on.   
            EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);

        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {

            string constantValueStr = eventData.Payload.FirstOrDefault(payload => payload.ToString().StartsWith(Core.Cmn.GeneralConstant.PayLoadKey))?.ToString();
            string constantValue = !string.IsNullOrEmpty(constantValueStr) ? constantValueStr.Replace(Core.Cmn.GeneralConstant.PayLoadKey + "_", "") : "";
            bool isTraceEnabled = !string.IsNullOrEmpty(constantValue) ? _constanService.GetValueByCategory<bool>(constantValue, Core.Cmn.GeneralConstant.TraceConfig) : true;

            if (isTraceEnabled)
            {
                string eventLogName = System.Diagnostics.EventLog.LogNameFromSourceName(eventData.EventSource.Name, ".");
                if (string.IsNullOrEmpty(eventLogName))
                    throw new Exception("you must add eventlog first by run Core.TraceViewer.exe, then use Trace feature");

                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog(eventLogName);

                eventLog.Source = eventData.EventSource.Name;

                string[] args = eventData.Payload != null ? eventData.Payload.Where(payload => payload.ToString() != constantValueStr)
                    .Select(payload => payload.ToString()).ToArray()
                      : null;
                var message = string.Format(eventData.Message, args);

                System.Diagnostics.EventLogEntryType level = System.Diagnostics.EventLogEntryType.Information;

                if (eventData.PayloadNames.ToList().Contains("level"))
                {
                    var customLevel = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), eventData.Payload.ElementAt(eventData.PayloadNames.ToList().IndexOf("level")).ToString());
                    switch (customLevel)
                    {
                        case EventLogEntryType.Error:
                            level = System.Diagnostics.EventLogEntryType.Error;
                            break;
                        case EventLogEntryType.Warning:
                            level = System.Diagnostics.EventLogEntryType.Warning;
                            break;
                        case EventLogEntryType.Information:
                            level = System.Diagnostics.EventLogEntryType.Information;
                            break;
                        case EventLogEntryType.SuccessAudit:
                            level = System.Diagnostics.EventLogEntryType.SuccessAudit;
                            break;
                        case EventLogEntryType.FailureAudit:
                            level = System.Diagnostics.EventLogEntryType.FailureAudit;

                            break;
                        default:
                            break;
                    }

                }
                eventLog.WriteEntry(message, level, eventData.EventId);

                eventLog.Close();
            }

        }
    }

}

