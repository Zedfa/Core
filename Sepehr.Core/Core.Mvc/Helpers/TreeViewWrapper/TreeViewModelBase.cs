using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers
{
  
    public class TreeViewModelBase//:JsonObject
    {
        public string Title { get; set; }

        public bool HasChildren { get; set; }

        public int Id { get; set; }


        //protected override void Serialize(IDictionary<string, object> json)
        //{
            
        //  //  json["id"] = this.Id;

        //    json["Title"] = this.Title;

        //    json["hasChildren"] = this.HasChildren;

        //    this.Serialize(json, "hasChildren", this.HasChildren, false);
        //}

    }
}
