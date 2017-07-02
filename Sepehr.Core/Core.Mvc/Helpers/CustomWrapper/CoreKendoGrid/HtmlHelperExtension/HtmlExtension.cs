using Core.Cmn;
using Core.Cmn.Reflection;
using Core.Entity;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Core.Mvc.Helpers.CoreKendoGrid
{
    public static class HtmlExtension
    {
        public static MvcHtmlString GridCr(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, string Width = null, string Height = null)// where T : class
        {
            return (new GridCr<object>(helper, gridName, gridTotalConfig, null, null, Width, Height)).Render();
        }
        public static MvcHtmlString GridCr(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, ClientDependentFeature clientDependency, string Width = null, string Height = null)// where T : class
        {
            return (new GridCr<object>(helper, gridName, gridTotalConfig, null, clientDependency, Width, Height)).Render();
        }

        public static MvcHtmlString GridCr(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, string fullName , ClientDependentFeature clientDependency= null, string Width = null, string Height = null)// where T : class
        {
            return (new GridCr<object>(helper, gridName, gridTotalConfig, ReflectionHelpers.GetType(fullName), clientDependency, Width, Height)).Render();
        }
        public static MvcHtmlString GridCr<T>(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, string Width = null, string Height = null) where T : IViewModel, new()
        {
            return (new GridCr<T>(helper, gridName, gridTotalConfig, null, null, Width, Height)).Render();
        }
        public static MvcHtmlString GridCr<T>(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, ClientDependentFeature clientDependency, string Width = null, string Height = null) where T : IViewModel, new()
        {
            return (new GridCr<T>(helper, gridName, gridTotalConfig, null, clientDependency, Width, Height)).Render();
        }
        public static MvcHtmlString GridCr(this HtmlHelper helper, string gridName, GridInfo gridTotalConfig, Type viewModelType, ClientDependentFeature clientDependency, string Width = null, string Height = null)
        {
            return (new GridCr<object>(helper, gridName, gridTotalConfig, viewModelType, clientDependency, Width, Height)).Render();
        }
        //public static string GetKendoGridCrScripts(this HtmlHelper helper)
        //{
        //    EmbeddedResourceVirtualPathProvider.EmbeddedResource fgh = new EmbeddedResourceVirtualPathProvider.EmbeddedResource(System.Reflection.Assembly.GetAssembly(typeof(GridCr<object>)), "Core.Mvc.Scripts.KendoGridCr.js", "Core.Mvc.Scripts.KendoGridCr.js");
        //    return fgh.ResourcePath;
        //}
    }
}
