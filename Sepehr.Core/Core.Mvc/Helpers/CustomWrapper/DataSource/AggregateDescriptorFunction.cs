using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    public class AggregateDescriptorFunction : JsonObject 
    {
        public string Field { get; set; }

        public string Aggregate { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["field"] = this.Field;
            json["aggregate"] = this.Aggregate;

        }
    }
}
