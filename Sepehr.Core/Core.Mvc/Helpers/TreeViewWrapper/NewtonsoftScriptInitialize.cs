using Kendo.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Core.Mvc.Helpers
{
    public class NewtonsoftScriptInitialize : IJavaScriptSerializer
    {
        string IJavaScriptSerializer.Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
