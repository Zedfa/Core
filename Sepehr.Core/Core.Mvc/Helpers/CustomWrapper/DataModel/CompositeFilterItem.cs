using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    public class CompositeFilterItem :JsonObject , IFilterItem
    {
        public FilterCompositionOperator Logic { get; set; }

        public List<IFilterItem> Filters { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            var twoFilters = new List<IDictionary<string, object>>();
            
            if (Filters.Count == 2)
            {
                Filters.ForEach(item => {
                    twoFilters.Add((item as FilterItem).ToJson());
                });
                json["filters"] = twoFilters;
                json["logic"] = Logic == FilterCompositionOperator.Conjunction ? "and" : "or";
            }
            else if (Filters.Count > 2)
            {
                var topItem = Filters[0] as FilterItem;
                var remainedFilters = this.Filters.Remove(this.Filters[0]);
                json["filters"] = SerializeForTwoRules(topItem, new CompositeFilterItem() { Filters = this.Filters, Logic = this.Filters[0].Logic });
                json["logic"] = topItem.Logic == FilterCompositionOperator.Conjunction ? "and" : "or";
            }
              
            
        }

        private List<IDictionary<string, object>> SerializeForTwoRules(FilterItem topItem , CompositeFilterItem compFilterItem)
        {
            var biFilterRules = new List<IDictionary<string, object>>();
                      biFilterRules.Add(topItem.ToJson());
                      biFilterRules.Add(compFilterItem.ToJson());
            
            return biFilterRules;
        }
    }
}
