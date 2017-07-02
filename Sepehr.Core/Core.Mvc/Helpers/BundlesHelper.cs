using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;


namespace Core.Mvc.Helpers
{
    public static class BundlesHelper
    {
        //public static IHtmlString Script(this HtmlHelper helper, params string[] urls)
        //{
        //    //string bundlePath = (helper.ViewContext.View as RazorView).ViewPath;

        //   // bundlePath = bundlePath.Replace("Views", "Scripts/Views").Replace("cshtml", "js");

        //   // if (System.IO.File.Exists( Server.MapPath(bundleDirectory)))
        //   // {
        //   // }

        //    //var bundleDirectory = viewPath //"~/Scripts/bundles/" + FindPhysicalPath(helper); //FindPhysicalPath(".js", urls);
        //    var thisBundle = new ScriptBundle(bundlePath).Include(urls);
        //    BundleTable.Bundles.Add(thisBundle);

        //    return Scripts.Render(bundlePath);
        //}

        public static IHtmlString ScriptCr(this HtmlHelper helper, string fileName)
        {
            string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();

            string viewPath = (helper.ViewContext.View as RazorView).ViewPath;

            string scriptPath = string.Empty,
                   logicalDivision = string.Empty;

            if (string.IsNullOrEmpty(fileName))
            {
                scriptPath = viewPath.Replace("Views", "Scripts/Views").Replace("cshtml", "js");

                logicalDivision = controllerName.ToLower().Contains("template") ?
                controllerName + "/" + helper.ViewContext.HttpContext.Request.QueryString.GetValues("viewmodel")[0]
                : controllerName;
            }
            else
            {
                string temp = viewPath.Replace("Views", "Scripts/Views") ;
                
                scriptPath = string.Format("{0}/{1}.js", temp.Substring(0, temp.LastIndexOf("/")), fileName);

                logicalDivision = controllerName.ToLower().Contains("template") ?
                            string.Format("{0}/{1}", controllerName, fileName)
                             : fileName;
            }

            var bundlePath = "~/Scripts/bundles/" + logicalDivision;

            if (System.IO.File.Exists(helper.ViewContext.HttpContext.Server.MapPath(scriptPath)))
            {
                var bundles = BundleTable.Bundles;
                // BundleContext context = new BundleContext(helper.ViewContext.HttpContext, BundleTable.Bundles, bundlePath);
                var existBundle = bundles.GetBundleFor(bundlePath);

                if (existBundle == null)
                {
                    var bundle = new ScriptBundle(bundlePath).Include(scriptPath);
                    bundles.Add(bundle);
                }
            }

            //var bundleDirectory = viewPath //"~/Scripts/bundles/" + FindPhysicalPath(helper); //FindPhysicalPath(".js", urls);


            return Scripts.Render(bundlePath);
        }

        public static IHtmlString ScriptCr(this HtmlHelper helper)
        {
            return ScriptCr(helper, string.Empty);
        }

        private static string FindPhysicalPath(string path)
        {
            //var view = helper. //filterContext.Result as ViewResult;
            // if (view != null)
            // {

            //     var razorEngin = view.ViewEngineCollection.OfType<RazorViewEngine>().Single();
            //     var viewName = !String.IsNullOrEmpty(view.ViewName) ? view.ViewName : filterContext.RouteData.Values["action"].ToString();
            //     //var razorView = ViewEngines.Engines.FindView(filterContext.Controller.ControllerContext, view.ViewName, view.MasterName).View as RazorView;
            //     var razorView = razorEngin.FindView(filterContext.Controller.ControllerContext, viewName, view.MasterName, false).View as RazorView;
            //     var path = razorView.ViewPath;
            //     path = path.Replace("Views", "Scripts/Views").Replace("cshtml", "js");

            //     if (System.IO.File.Exists(Server.MapPath(path)))
            //     {
            //         var bundles = BundleTable.Bundles;

            //         var pageScripts = bundles.GetBundleFor("~/bundles/pagescripts");

            //         pageScripts.Transforms.Add(new JsMinify());
            //         pageScripts.Include(path);
            //     }
            // }
            return string.Empty;
        }
    }
}
