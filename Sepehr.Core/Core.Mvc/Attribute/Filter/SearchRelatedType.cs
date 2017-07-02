using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Attribute.Filter
{
    public class SearchRelatedTypeAttribute : System.Attribute
    {
        public string CustomType { get; set; }
        public string MainPropertyNameOfModel { get; set; }
        public string NavigationProperty { get; set; }
        public string FalseEquivalent { get; set; }
        public string TrueEquivalent { get; set; }
        public bool IdReplacement { get; set; }
        public SearchRelatedTypeAttribute()
        {

        }
    }
}
