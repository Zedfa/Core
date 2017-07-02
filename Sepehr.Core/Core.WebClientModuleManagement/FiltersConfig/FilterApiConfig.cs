using Core.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;


namespace Core.WebClientModuleManagement
{
    public class FilterApiConfig
    {
        public static void RegisterHttpFilters(HttpFilterCollection filters, Assembly assembly)
        {
            //filters.Add(new AuthorizationApi());

             //var nameSpace = assembly.GetName().Name;
            
             var filtersClasses = assembly.GetTypes().Where(type => typeof(AuthorizationBaseApi).IsAssignableFrom(type));
             foreach (var filterclass in filtersClasses)
             {
                 filters.Add(Activator.CreateInstance(filterclass) as IFilter); 
             }
            //var bundleConfigClass = assembly.GetType(string.Format("{0}.AuthorizationBaseApi", nameSpace));
            //var registerBundleInstance = Activator.CreateInstance(bundleConfigClass);
            //var registerBundlesMethod = bundleConfigClass.GetMethod("RegisterBundles");

            //var relativeBundlePath = string.Format("~/{0}", relativePath.Replace("\\", "/"));
            //registerBundlesMethod.Invoke(registerBundleInstance, new object[] { bundles, relativeBundlePath });
        }
    }
}