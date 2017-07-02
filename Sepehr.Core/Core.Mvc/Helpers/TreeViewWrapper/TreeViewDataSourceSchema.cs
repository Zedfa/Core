using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Core.Mvc.Helpers
{
    internal class TreeViewDataSourceSchema : DataSourceSchema
    {
        protected override void Serialize(IDictionary<string, object> json)
        {
            base.Serialize(json);

            //json["id"] = this.Id;

            //json["Title"] = this.Title;

            //json["hasChildren"] = this.HasChildren;
        }
    }
}
