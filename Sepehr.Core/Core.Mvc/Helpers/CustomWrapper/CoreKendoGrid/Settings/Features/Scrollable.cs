
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    /// <summary>
    /// 1.To achieve vertical scrolling, the Grid must have a set height. Otherwise, it will expand vertically to show all rows.
    /// 2.To achieve horizontal scrolling, all columns must have set widths and their sum must exceed the Grid width. 
    ///   Otherwise widthless columns will shrink to fit in the space determined by the Grid's width
    /// </summary>
    [Serializable()]
    public class Scrollable : JsonObjectBase
    {
        public Scrollable()
        {
            IsVirtual = true;
            IsScrollable = false;
            Position = ScrollPosition.Bottom;
        }
        /// <summary>
        /// Will load in data from the remote data source as you scroll down the grid
        /// </summary>
        public bool IsVirtual { get; set; }
        public bool IsScrollable { get; set; }
        public ScrollPosition Position { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["virtual"] = IsVirtual;
            json["position"] = Position == 0 ? "top" : "bottom";
        }
    }
    public enum ScrollPosition
    {
        Top,
        Bottom
    }
}
