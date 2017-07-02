using Core.Cmn;
using Core.Cmn.Cache;
using System;

namespace Core.Rep
{
   public class RepositoryBase_ApplicationStart: ApplicationStartBase
    {
        public override void OnApplicationStart()
        {
          
            CacheConfig.OnRepositoryCacheConfig(RepositoryBase.DependencyInjectionFactory.CreateContextInstance().GetType());
        }

        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.CreateRepositoryCacheConfig;
            }
        }
    }
}
