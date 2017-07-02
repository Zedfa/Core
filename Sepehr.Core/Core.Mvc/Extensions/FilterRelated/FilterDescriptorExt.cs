using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Extensions.FilterRelated
{
    public static class FilterDescriptorExt
    {
        public static void GetFilterDescriptors(this IFilterDescriptor filters, ref List<FilterDescriptor> result)
        {
            if (filters is CompositeFilterDescriptor)
            {
                var descriptor = filters as CompositeFilterDescriptor;
                foreach (var filterDescriptor in descriptor.FilterDescriptors)
                {
                    GetFilterDescriptors(filterDescriptor, ref result);
                }
            }
            else if (filters is FilterDescriptor)
            {
                var filter = filters as FilterDescriptor;
                if (filter != null)
                {
                    result.Add(filter);
                    // result.Add(filter.Member, filter.Value.ToString());
                }
            }
        }

        //public static void GetFilterDescriptor(this IFilterDescriptor filters, ref Dictionary<string, string> result)
        //{
        //    if (filters is CompositeFilterDescriptor)
        //    {
        //        var descriptor = filters as CompositeFilterDescriptor;
        //        foreach (var filterDescriptor in descriptor.FilterDescriptors)
        //        {
        //            GetFilterDescriptor(filterDescriptor, ref result);
        //        }
        //    }
        //    else if (filters is FilterDescriptor)
        //    {
        //        var filter = filters as FilterDescriptor;
        //        if (filter != null)
        //        {
        //            result.Add(filter.Member, filter.Value.ToString());
        //        }
        //    }
        //}
    }
}