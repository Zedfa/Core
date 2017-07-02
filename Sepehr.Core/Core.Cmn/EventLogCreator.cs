using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public abstract class EventLogCreator
    {
        public abstract IEventLog BuildOccuredEvent();
    }
}
