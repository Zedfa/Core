using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class FilterConfig : JsonObjectBase
    {
        public FilterConfig(CultureInfo cultureInfo)
        {
            FilterMessageSec = new FilterMessage(this, cultureInfo);
            FilterOperatorSec = new FilterOperators(cultureInfo);

        }
        public bool FilterAffection { get; set; }
        public FilterMessage FilterMessageSec { get; private set; }
        public FilterOperators FilterOperatorSec { get; private set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["messages"] = FilterMessageSec.ToJson();
            json["operators"] = FilterOperatorSec.ToJson();
        }
    }
}
