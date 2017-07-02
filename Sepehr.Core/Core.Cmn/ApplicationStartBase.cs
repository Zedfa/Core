using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public enum ExecutionPriorityBeforeApplicationStart
    {

        LastOperation = int.MaxValue
    }

    public  enum ExecutionPriorityOnApplicationStart
    {
        WebClientModuleManagement = 1000,
        CreateInstanceDIFactory = 2000,
        TraceViewerService = 3000,
        LogServiceBase = 4000,
        SetDefaultCulture = 5000,
        CreateRepositoryCacheConfig = 6000,
        CreateServiceCacheConfig = 7000,
        LastOperation = int.MaxValue,
    }

    public abstract class ApplicationStartBase
    {
       
        public virtual void BeforeApplicationStart() { }
        public virtual void OnApplicationStart() { }
        public virtual Enum ExecutionPriorityBeforeApplicationStart { get { return Core.Cmn.ExecutionPriorityBeforeApplicationStart.LastOperation; } }
        public virtual Enum ExecutionPriorityOnApplicationStart { get { return Core.Cmn.ExecutionPriorityOnApplicationStart.LastOperation; } }
        
    }

}
