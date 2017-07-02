using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface ILogService
    {
        void Handle(Exception ex, string logFileName, string customMessage);
        void Handle(Exception ex, string logFileName, string logUserId, string customMessage);
        void Handle(IEventLog eventLog);
        void BatchHandle(List<EventLog> eventLogs);
        EventLog GetEventLogObj();
    }
}
