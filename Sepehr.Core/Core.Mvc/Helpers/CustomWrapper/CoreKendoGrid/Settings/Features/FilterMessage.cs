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
    public class FilterMessage : JsonObjectBase
    {
        private FilterConfig _filterConfig;
        public FilterMessage(FilterConfig filterConfig , CultureInfo cultureInfo)
        {
            _filterConfig = filterConfig;
        }
        public FilterMessage(CultureInfo cultureInfo)
        {

        }
        public string And { get; set; }
        public string Or { get; set; }
        public string Filter { get; set; }
        public string Clear { get; set; }
        public string Info { get; set; }
        public string IsFalse { get; set; }
        public string IsTrue { get; set; }
        public string SelectValue { get; set; }
        

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (_filterConfig.FilterAffection)
            {
                json["and"] = "و";
                json["or"] = "یا";
                json["filter"] = "فیلتر";
                json["clear"] = " حذف";
                json["info"] = "جستجو";
                json["isFalse"] = "اشتباه";
                json["isTrue"] = "صحیح";
                json["selectValue"] = "انتخاب از مجموعه";
            } 
        }
    }
}
