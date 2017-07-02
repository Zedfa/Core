using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class ExceptionEventLogCreator : EventLogCreator
    {
        public override IEventLog BuildOccuredEvent()
        {
            EventLog eventLog = new EventLog();
            return eventLog;
        }
    }
}
