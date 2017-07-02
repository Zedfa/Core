using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class CrudInfoEdit : CrudInfo 
    {
        public CrudInfoEdit()
        {
            EntityKeyValue = new Dictionary<string, string>();
        }
        public IDictionary<string, string> EntityKeyValue { get; private set; }
    }
}
