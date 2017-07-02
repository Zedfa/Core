using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Extensions.FilterRelated
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SearchExcludingFieldsAttribute : System.Attribute
    {
        public string FieldsToExcludeFromSearchable { get; set; }
    }
}
