using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class ColumnMenu : JsonObjectBase
    {
        public ColumnMenu()
            : this(null)
        { }
        private CultureInfo _cultureInfo;
        public ColumnMenu(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            ColumnMenuEnabled = false;
            Sortable = true;
            Filterable = true;
            Columns = true;
        }
        public bool ColumnMenuEnabled { get; set; }
        public bool Sortable { get; set; }
        public bool Filterable { get; set; }
        public bool Columns { get; set; }
        /// <summary>
        /// یک رویداد است.
        /// </summary>
        public string ColumnMenuInit { get; set; }
        public ColumnMenuMessagesCr ColumnMenuMessages { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (ColumnMenuEnabled)
            {
                if (Sortable || Filterable || Columns)
                {
                    ColumnMenuMessages = new ColumnMenuMessagesCr(_cultureInfo);
                    json["messages"] = ColumnMenuMessages.ToJson();
                }
                json["sortable"] = Sortable;
                json["filterable"] = Filterable;
                json["columns"] = Columns;
            }
        }
    }
}
