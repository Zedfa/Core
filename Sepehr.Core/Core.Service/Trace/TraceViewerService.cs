﻿using Core.Cmn.Attributes;
using System;
using Core.Cmn;
using Microsoft.Diagnostics.Tracing;
using System.Linq;


namespace Core.Service.Trace
{
    [EventSource(Name = "TraceViewerService")]
    [Injectable(InterfaceType = typeof(ITraceViewer), DomainName = "Core", LifeTime = LifetimeManagement.ContainerControlledLifetime)]
    public sealed class TraceViewerService : EventSource, ITraceViewer
    {
        private static object _justForLock = new object();
        private static int _countOfInstances;
        private IConstantService _constanService;

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
                    _constanService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();
               
                }
            }
        }
    
        private bool IsTraceEnabled(string traceKey)
        {

            return !string.IsNullOrEmpty(traceKey) ? _constanService.GetValueByCategory<bool>(traceKey, Core.Cmn.GeneralConstant.TraceConfig) : true;

        }


        [Event(1, Message = "Application Failure: {0}", Level = EventLevel.Error, Keywords = Keywords.Debug, Channel = EventChannel.Admin)]
        public void Failure(string message, string traceKey = "")
        {
            
            if (IsTraceEnabled(traceKey))
                WriteEvent(1, message, traceKey);
        }

        [Event(2, Message = "Information: {0}", Level = EventLevel.Informational, Keywords = Keywords.Debug, Channel = EventChannel.Admin)]
        public void Inform(string message, string traceKey = "")
        {

            if (IsTraceEnabled(traceKey))
                WriteEvent(2, message, traceKey);
        }

        [Event(3, Message = "warning: {0}", Level = EventLevel.Warning, Keywords = Keywords.Debug, Channel = EventChannel.Admin)]
        public void Attention(string message, string traceKey = "")
        {

            if (IsTraceEnabled(traceKey))
                WriteEvent(3, message, traceKey);
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


}

