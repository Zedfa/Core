using System.Web.Mvc;
using System.Web.Http;
using Core.Service;
using Core.Entity;

namespace Core.Mvc
{
    public class CoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Core"; }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {



            RegisterRouteMapTable(context);

            context.MapRoute(
                "Core_default",
                "Core/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = RouteParameter.Optional },
                namespaces: new[] { "Core.Mvc.Controllers" }
            );


            context.Routes.MapHttpRoute(
                              name: "CoreApi_default",
                              routeTemplate: "api/Core/{controller}/{id}",
                              defaults: new { id = RouteParameter.Optional });


        }

        private void RegisterRouteMapTable(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Core_defaultLogin",
            //    "Login1",
            //    new { area="Core", controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "Core.Mvc.Controllers" }
            //);


            var routeMapServices = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IServiceBase<RouteMapConfig>>();
            var routes = routeMapServices.All();

            foreach (var route in routes)
            {
                if (route.Pattern.StartsWith("api/"))
                {
                    context.Routes.MapHttpRoute(route.Name, route.Pattern);  
                }
                else
                {
                    var seperatedRoute = route.defaults.Split('/');
                    var areaRoute = seperatedRoute[0];
                    var controllerRoute = seperatedRoute[1];
                    var actionRoute = seperatedRoute[2];
                    object defaults = null;

                    if (!string.IsNullOrEmpty(areaRoute) && !areaRoute.Equals("-"))
                    {
                        route.Namespace = string.IsNullOrEmpty(route.Namespace) ? string.Format("{0}.Mvc.Controllers", areaRoute) : route.Namespace;

                        defaults = new { area = areaRoute, controller = controllerRoute, action = actionRoute, id = UrlParameter.Optional };

                        context.MapRoute(route.Name, route.Pattern, defaults, namespaces: new[] { route.Namespace });
                    }
                    else
                    {
                        defaults = new { controller = controllerRoute, action = actionRoute, id = UrlParameter.Optional };

                        context.MapRoute(route.Name, route.Pattern, defaults);
                    }
                }


            }


        }

    }
}