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
    public class Pageability : JsonObjectBase
    {
        //private CultureInfo _cultureInfo;
        public Pageability(CultureInfo cultureInfo)
        {
            PageSizesConfig = new PageSizesConfig(cultureInfo);
            PageMessages = new PageMessage(this , cultureInfo);
            AcceptsPageConfig = true;
           PageSize = 100;
            ApplyRefresh = false;
            PageInfo = true;
            AcceptsInput = false;
            Numeric = false;
            PreviousNext = false;
        }

        public bool AcceptsPageConfig { get; set; }
       public int PageSize { get; set; }
        public PageMessage PageMessages { get; set; }
        public bool ApplyRefresh { get; set; }
        public bool PageInfo { get; set; }
        public bool AcceptsInput { get; set; }//It Makes sense in existence of Property Page of PageMessages Class
        public PageSizesConfig PageSizesConfig { get; set; }
        public bool Numeric { get; set; }
        public bool PreviousNext { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            //json["refresh"] = ApplyRefresh;
            json["pageSize"] = PageSize;
            json["numeric"] = Numeric;
            json["previousNext"] = PreviousNext;
            json["messages"] = PageMessages.ToJson();
        }
    }
}

