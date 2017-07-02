using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Mvc.Helpers
{
    public class TreeViewCheckboxesSettings : JsonObject
    {

        public TreeViewCheckboxesSettings(bool checkChildren=true)
        {
            this.Template = "<input type='checkbox' name='checkedNodes' #= item.checked ? 'checked' : '' # value='#= item.id #' />";

            this.Enabled = true;

            this.CheckChildren = checkChildren;

        }

        public bool CheckChildren { get; set; }

        public bool Enabled { get; set; }

        public object Template { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (this.Enabled)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary["template"] = this.Template;
                if (this.CheckChildren)
                {
                    dictionary["checkChildren"] = true;
                }
                json["checkboxes"] = dictionary;
            }
        }

        
    }
}
