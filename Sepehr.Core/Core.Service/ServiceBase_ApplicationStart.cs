using Core.Cmn;
using Core.Cmn.Cache;
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
            Core.Cmn.AppBase.TraceViewer = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<ITraceViewer>();
           // var eventListener = new TraceViewerEventListener();
        }
        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.TraceViewerService;
            }
        }
    }
}


   

