using Core.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Core.Mvc.Infrastructure
{

    public class _AuthorizationBaseApi : System.Web.Http.AuthorizeAttribute
    {

        protected virtual bool _IsAuthorized(HttpActionContext actionContext)
        {
            return true;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        //protected new virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            var routeTemplate = actionContext.ControllerContext.RouteData.Route.RouteTemplate;
            string areaName = routeTemplate.Split('/')[1];
           // if (areaName.ToLower() == this.GetType().Assembly.GetName().Name.ToLower().Replace(".mvc", ""))
            //{
                return _IsAuthorized(actionContext);
            //}

           // return true;




        }
    }
}