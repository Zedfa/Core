using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class ColumnMenuMessagesCr : JsonObjectBase
    {
        private CultureInfo _cultureInfo;
        public ColumnMenuMessagesCr(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        public string Columns { get; set; }
        public string Filter { get; set; }
        public string SortAscending { get; set; }
        public string SortDescending { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            Columns = "ستون ها";
            Filter = "فیلتر";
            SortAscending = "مرتب سازی صعودی";
            SortDescending = "مرتب سازی نزولی";
            json["columns"] = Columns;
            json["filter"] = Filter;
            json["sortAscending"] = SortAscending;
            json["sortDescending"] = SortDescending;
        }
    }
}
