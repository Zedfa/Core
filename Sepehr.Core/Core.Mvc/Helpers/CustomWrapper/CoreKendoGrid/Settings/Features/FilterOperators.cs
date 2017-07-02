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
    public class FilterOperators : JsonObjectBase
    {
        public FilterOperators(CultureInfo cultureInfo)
        {
            String = new FilterOpString(cultureInfo);
            Number = new FilterOpNumber(cultureInfo);
            Date = new FilterOpDate(cultureInfo);
            Enum = new FilterOperatorEnum(cultureInfo);
        }
        public FilterOpString String { get; private set; }
        public FilterOpNumber Number { get; private set; }
        public FilterOpDate Date { get; private set; }
        public FilterOperatorEnum Enum { get;private set; }
        protected override void Serialize(IDictionary<string, object> json)
        {
            json["string"] = String.ToJson();
            json["number"] = Number.ToJson();
            json["date"] = Date.ToJson();
            //json["enum"] = Enum.ToJson();
        }
    }
}
