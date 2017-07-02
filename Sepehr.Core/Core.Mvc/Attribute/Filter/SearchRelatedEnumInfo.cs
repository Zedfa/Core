using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Attribute.Filter
{
    public class SearchRelatedEnumInfoAttribute : System.Attribute
    {
        public string CustomType { get; set; }
        public string MainPropertyNameOfModel { get; set; }
        public Type EnumType { get; set; }
        public SearchRelatedEnumInfoAttribute()
        {

        }
    }
}
