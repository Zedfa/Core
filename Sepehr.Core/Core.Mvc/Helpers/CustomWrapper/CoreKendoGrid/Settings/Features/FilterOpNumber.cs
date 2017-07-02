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
    public class FilterOpNumber : JsonObjectBase
    {
        private CultureInfo _cultureInfo;
        public FilterOpNumber(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["eq"] = "برابر با";
            json["neq"] = "نا برابر با";
            json["gte"] = "بزرگتر مساوی";
            json["gt"] = "بزرگتر از";
            json["lte"] = "کوچکتر مساوی";
            json["lt"] = "کوچکتر از";
        }
    }
}
