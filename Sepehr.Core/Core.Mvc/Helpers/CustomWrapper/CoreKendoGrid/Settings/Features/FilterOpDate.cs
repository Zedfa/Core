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
    public class FilterOpDate : JsonObjectBase
    {
        public FilterOpDate(CultureInfo cultureInfo)
        {

        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["gt"] = "بعد از ";
            json["lt"] = "قبل از";
            json["eq"] = "برابر";
            json["neq"] = "نا برابر";
            json["gte"] = "بعد یا برابر";
        }
    }
}
