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
    public class Grouping : JsonObjectBase
    {
       // private CultureInfo _cultureInfo;
        public Grouping(CultureInfo cultureInfo)
        {
            GroupingMessages = new GroupingMessages(cultureInfo);
        }
        public GroupingMessages GroupingMessages { get;private set; }


        protected override void Serialize(IDictionary<string, object> json)
        {
            json["messages"] = GroupingMessages.ToJson();
        }
    }
}
