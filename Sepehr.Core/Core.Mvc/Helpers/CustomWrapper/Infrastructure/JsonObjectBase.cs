using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.Infrastructure
{
    [Serializable()]
    public abstract class JsonObjectBase
    {
        protected abstract void Serialize(IDictionary<string, object> json);
        
        public IDictionary<string, object> ToJson()
        {
            var json = new Dictionary<string, object>();

            Serialize(json);

            return json;
        }

    }
}
