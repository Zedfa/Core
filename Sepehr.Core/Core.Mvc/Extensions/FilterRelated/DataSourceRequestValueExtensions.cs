using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Extensions.FilterRelated
{
    public static class DataSourceRequestValueExtensions
    {
        public static Dictionary<string, object> GetFilterValues(this DataSourceRequest request)
        {
            return GetFilterValues((List<Kendo.Mvc.IFilterDescriptor>)request.Filters);
        }

        public static Dictionary<string, object> GetFilterValues(this List<Kendo.Mvc.IFilterDescriptor> Filters)
        {
            var dictionary = new Dictionary<string, object>();
            var rootFilterItem = (Filters as List<Kendo.Mvc.IFilterDescriptor>).First();


            if (rootFilterItem is Kendo.Mvc.CompositeFilterDescriptor)
            {
                Kendo.Mvc.Infrastructure.Implementation.FilterDescriptorCollection filterDescriptors = (rootFilterItem as Kendo.Mvc.CompositeFilterDescriptor).FilterDescriptors;
                DataSourceRequestValueExtensions.ExtractCompositeFilterDescriptorValues(dictionary, filterDescriptors);
            }
            else if (rootFilterItem is Kendo.Mvc.FilterDescriptor)
            {
                Kendo.Mvc.FilterDescriptor simpleItem = (Kendo.Mvc.FilterDescriptor)rootFilterItem;
                DataSourceRequestValueExtensions.ExtractSimpleFilterDescriptorValues(dictionary, simpleItem);
            }

            return dictionary;

        }

        private static void ExtractSimpleFilterDescriptorValues(Dictionary<string, object> dictionary, Kendo.Mvc.FilterDescriptor simpleFilterDescriptorRule)
        {
            dictionary.Add(simpleFilterDescriptorRule.Member , simpleFilterDescriptorRule.ConvertedValue);
        }
        private static void ExtractCompositeFilterDescriptorValues(Dictionary<string, object> dictionary , Kendo.Mvc.Infrastructure.Implementation.FilterDescriptorCollection filterDescriptors)
        {
            foreach (var filterItem in filterDescriptors)
            {
                if (filterItem is Kendo.Mvc.CompositeFilterDescriptor)
                {
                    var fDescriptors = (filterItem as Kendo.Mvc.CompositeFilterDescriptor).FilterDescriptors;
                    ExtractCompositeFilterDescriptorValues(dictionary, fDescriptors);
                }
                else if (filterItem is Kendo.Mvc.FilterDescriptor)
                {
                    var sItem = (Kendo.Mvc.FilterDescriptor)filterItem;
                    ExtractSimpleFilterDescriptorValues(dictionary, sItem);
                }
            }
        }



    }
}
