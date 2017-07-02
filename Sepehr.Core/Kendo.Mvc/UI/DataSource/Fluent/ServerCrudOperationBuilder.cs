﻿using System;
using System.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Kendo.Mvc.UI.Fluent
{
    /// <summary>
    /// Defines the fluent interface for configuring the <see cref="CrudOperation"/> options for server binding.
    /// </summary>
    public class ServerCrudOperationBuilder: IHideObjectMembers
    {
        private readonly CrudOperation operation;
        private readonly ViewContext viewContext;
        private readonly IUrlGenerator urlGenerator;

        public ServerCrudOperationBuilder(CrudOperation operation, ViewContext viewContext, IUrlGenerator urlGenerator)
        {
            this.viewContext = viewContext;
            this.urlGenerator = urlGenerator;
            this.operation = operation;
        }

        /// <summary>
        /// Sets the route values for the operation.
        /// </summary>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Route(RouteValueDictionary routeValues)
        {
            operation.Action(routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action, contoller and route values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Action(string actionName, string controllerName, object routeValues)
        {
            operation.Action(actionName, controllerName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action, contoller and route values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>        
        public ServerCrudOperationBuilder Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            operation.Action(actionName, controllerName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the action and contoller values for the operation.
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>        
        public ServerCrudOperationBuilder Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, (object)null);
        }

        /// <summary>
        /// Sets the route name and values for the operation.
        /// </summary>
        /// <param name="routeName">Route name</param>
        /// <param name="routeValues">Route values</param>        
        public ServerCrudOperationBuilder Route(string routeName, RouteValueDictionary routeValues)
        {
            operation.Route(routeName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the route name and values for the operation.
        /// </summary>
        /// <param name="routeName">Route name</param>
        /// <param name="routeValues">Route values</param>
        public ServerCrudOperationBuilder Route(string routeName, object routeValues)
        {
            operation.Route(routeName, routeValues);

            SetUrl();

            return this;
        }

        /// <summary>
        /// Sets the route name for the operation.
        /// </summary>
        /// <param name="routeName"></param>
        public ServerCrudOperationBuilder Route(string routeName)
        {
            operation.Route(routeName, (object)null);

            SetUrl();

            return this;
        }

        public ServerCrudOperationBuilder Action<TController>(Expression<Action<TController>> controllerAction) where TController : Controller
        {
            operation.Action(controllerAction);

            SetUrl();

            return this;
        }

        private void SetUrl()
        {
            operation.Url = operation.GenerateUrl(viewContext, urlGenerator);
        }
    }
}
