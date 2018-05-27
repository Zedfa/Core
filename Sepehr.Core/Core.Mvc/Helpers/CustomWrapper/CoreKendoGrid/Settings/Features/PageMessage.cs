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
    public class PageMessage : JsonObjectBase
    {
        private Pageability _pageConfig;
        public PageMessage(Pageability pageConfig, CultureInfo cultureInfo)
        {
            _pageConfig = pageConfig;
        }
        public PageMessage(CultureInfo cultureInfo)
        {

        }
        public string Previous { get; set; }
        public string Next { get; set; }
        public string Last { get; set; }
        public string First { get; set; }
        public string Refresh { get; set; }
        public string ItemsPerPage { get; set; }
        public string Page { get; set; }
        public string Of { get; set; }
        public string Display { get; set; }
        public string Empty { get; set; }


        protected override void Serialize(IDictionary<string, object> json)
        {
            json["empty"] = "هیچ داده ای وجود ندارد";//, " تا ",
            var fromToMsg = string.Format("{0} {1}", "{0}", " تا {1}");
            json["display"] = "نمایش {0}-{1} از {2} داده";// "Showing {0}-{1} from {2} data items";
            json["of"] = "از";
            if (_pageConfig.AcceptsInput)
            {
                json["page"] = "شماره صفحه را وارد کنید";
            }
            if (_pageConfig.PageSizesConfig.PageSizesEnabled && _pageConfig.PageSizesConfig.PageSizes != null)
            {
                json["itemsPerPage"] = "آیتم(ها) در هر صفحه";
            }
            if (_pageConfig.ApplyRefresh)
            {
                json["refresh"] = "بار گذاری مجدد";
            }
            json["first"] = "اولین";
            json["last"] = "آخرین";
            json["next"] = "بعدی";
            json["previous"] = "قبلی";
        }
    }
}
