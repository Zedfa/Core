using Core.Cmn;
using Core.Cmn.Cache;
using Core.Cmn.Interface;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class LogServiceBase_ApplicationStart : ApplicationStartBase
    {
        public override void OnApplicationStart()
        {

            Core.Cmn.AppBase.LogService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<ILogService>();


        }
        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.LogServiceBase;
            }
        }
    }

    public class SetDefaultCultureServiceBase_ApplicationStart : ApplicationStartBase
    {
        public override void OnApplicationStart()
        {
            IConstantService constantService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IConstantService>();
            if (constantService != null)
            {
                string defaultCulture = constantService.GetDefaultCulture(false);
                System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(defaultCulture);
            }
        }
        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.SetDefaultCulture;
            }
        }
    }

    public class CacheServiceBase_ApplicationStart : ApplicationStartBase
    {
        public override void OnApplicationStart()
        {

            CacheConfig.OnServiceCacheConfig(ServiceBase.DependencyInjectionFactory.CreateContextInstance().GetType());
        }

        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.CreateServiceCacheConfig;
            }
        }
    }

    public class TraceViewerBase_ApplicationStart : ApplicationStartBase
    {
        public override void OnApplicationStart()
        {

            Core.Cmn.AppBase.TraceWriter = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<ITraceWriter>();

            ////long traceSize = 0;
            ////bool traceIsOn = false;
            ////string traceName = string.Empty;
            ////if (ConfigHelper.TryGetConfigValue<bool>( Cmn.GeneralConstant.EnableTrace, out traceIsOn) &&
            ////    ConfigHelper.TryGetConfigValue<string>(Cmn.GeneralConstant.TraceName, out traceName) &&
            ////    ConfigHelper.TryGetConfigValue<long>(Cmn.GeneralConstant.TraceSize, out traceSize))
            ////{


            ////    Core.Cmn.AppBase.SharedMemoryConfig.Open();

            ////    Core.Cmn.AppBase.Trace.Open();
            ////}
            //// var eventListener = new TraceViewerEventListener();

            //Core.Cmn.AppBase.TraceWriter = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<ITraceWriterService>();

        }
        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.TraceWriterService;
            }
        }
    }
}




