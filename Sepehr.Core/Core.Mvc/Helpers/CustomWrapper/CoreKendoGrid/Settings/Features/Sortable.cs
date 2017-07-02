
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
    [Serializable()]
    public class Sortable : JsonObjectBase
    {
        public Sortable()
        {
            IsSortable = true;
            IsMultisort = true;
        }
        public bool IsSortable { get; set; }
        public bool IsMultisort { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["sortable"] = IsSortable;
        }
    }
}
