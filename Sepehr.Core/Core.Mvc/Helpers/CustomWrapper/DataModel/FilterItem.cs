using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable]
    public class FilterItem : JsonObjectBase, IFilterItem
    {
        public string Field { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }
       


        protected override void Serialize(IDictionary<string, object> json)
        {
            if (!string.IsNullOrEmpty(Field) && !string.IsNullOrEmpty(Operator) && !string.IsNullOrEmpty(Value))
            {
                json["field"] = Field;
                json["operator"] = Operator;
                json["value"] = Value;
            }
        }

        public FilterCompositionOperator Logic
        {
            get;
            set;
        }
    }
}
