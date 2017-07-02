using Core.Mvc.Attribute.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.SearchRelated
{
    public class SearchConstantField : SearchInfo
    {
        public string ConstantsCategoryName { get; set; }
        //public Type EnumType
        //{
        //    get
        //    {
        //        //On the assumption that CustomType is convertible to a proper Enum Type.
        //        return Type.GetType(base.CustomType);
        //    }
        //}
       //public SearchConstantField()
       //{
       //    base.HasCustomTypeOf(SearchRelatedTypes.Enum);
       //}

    }
}
