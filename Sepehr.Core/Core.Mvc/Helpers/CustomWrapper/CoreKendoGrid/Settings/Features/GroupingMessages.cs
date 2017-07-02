using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class GroupingMessages : JsonObjectBase
    {
        private CultureInfo _cultureInfo;
        public GroupingMessages(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            Empty = "برای گروه بندی روی عنوان ستون مورد نظر کلیک کرده و آن را به این محل منتقل کنید ";
        }
        public string Empty { get;private set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["empty"] = Empty;
        }
    }
}
