using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    [Serializable()]
    public enum DataSourceEvent
    {
        OnError,
        OnRequestStart,
        OnRequestEnd,
        Sync,
        OnChange
    }
}
