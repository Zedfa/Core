using Core.Cmn;
using System;

namespace Core.WebClientModuleManagement
{
    public class WebClientModuleManagement_ApplicationStart:ApplicationStartBase
    {
        public override void OnApplicationStart()
        {
            ResourceManagementBase.Start();
        }
        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.WebClientModuleManagement;
            }
        }
    }
}
