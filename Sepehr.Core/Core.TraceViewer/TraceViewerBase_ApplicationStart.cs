using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TraceViewer
{
    public class TraceViewerBase_ApplicationStart// : ApplicationStartBase
    {
        public static void OnApplicationStart()
        {
            var eventListener = new TraceViewerEventListener();
            Core.Cmn.AppBase.TraceViewer = new Core.Service.TraceViewerService();

        }
        public  Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.TraceViewerService;
            }
        }
    }

}