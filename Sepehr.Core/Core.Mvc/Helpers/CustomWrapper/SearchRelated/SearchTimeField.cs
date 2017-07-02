using Core.Mvc.Attribute.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.SearchRelated
{
    public class SearchTimeField : SearchInfo 
    {
        public SearchTimeField()
       {
           base.HasCustomTypeOf(SearchRelatedTypes.Time);
       }
    }
}
