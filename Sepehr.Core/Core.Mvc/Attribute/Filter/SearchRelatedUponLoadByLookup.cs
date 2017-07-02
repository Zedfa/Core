using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Attribute.Filter
{
    public class SearchRelatedUponLoadByLookupAttribute : System.Attribute
    {
        // public string CustomType { get; set; }
        public string PropertyNameForValue { get; set; }
        public string PropertyNameForBinding { get; set; }
        public string PropertyNameForDisplay { get; set; }
        public string viewInfoName { get; set; }
        public string Id { get; set; }
        public string ViewModel { get; set; }
        public string Title { get; set; }
        //public string[] lookUpGridFor { get; set; }
        //public string[] lookUpTreeFor { get; set; }//iViewInfoProperty , isMultiSelectFor
    }
}
