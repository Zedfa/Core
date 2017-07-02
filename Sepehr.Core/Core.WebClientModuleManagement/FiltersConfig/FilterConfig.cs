using System;
using System.Reflection;
using System.Web.Mvc;


using System.Linq;
using Core.Mvc.Infrastructure;

namespace Core.WebClientModuleManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, Assembly assembly)
        {

            //var nameSpace = assembly.GetName().Name;
            var filtersClasses = assembly.GetTypes().Where(type => typeof(AuthorizationBase).IsAssignableFrom(type));
            foreach (var filterclass in filtersClasses)
            {
                filters.Add(Activator.CreateInstance(filterclass));
            }
        }
    }
}