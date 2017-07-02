using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class PreDefinedEvents
    {
        public string OnError { get; set; }
        public string OnChange { get; set; }
    }
}
