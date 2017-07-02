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
    public class FilterOperatorEnum : JsonObjectBase
    {
        public FilterOperatorEnum(CultureInfo cultureInfo)
        {

        }


        protected override void Serialize(IDictionary<string, object> json)
        {
            json["eq"] = "برابر با";
            json["neq"] = "نا برابر با";
        }
    }
}
