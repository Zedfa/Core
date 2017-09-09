using Core.Cmn;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System;
using System.Threading;

namespace Core.Mvc.Controller
{
    [ValidateInput(true)]
    public class ControllerBaseCr : System.Web.Mvc.Controller
    {
        private ILogService _logService = Core.Cmn.AppBase.LogService;

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            //Core.Cmn.AppBase.TraceViewer.Failure(filterContext.Exception.InnerException + filterContext.Exception.StackTrace + filterContext.Exception.Message);

            ExceptionInfo excepInfo = new ExceptionInfo(filterContext.Exception, false);

            if (!this.HttpContext.User.Identity.Name.ToLower().Equals("admin"))
            {
                var constantService = AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
                var applicationFaildMsg = string.Empty;
                constantService.TryGetValue<string>("ApplicationFaild", out applicationFaildMsg);

                excepInfo.Message = applicationFaildMsg/*Core.Resources.ExceptionMessage.ApplicationFaild*/;
                excepInfo.Details = "";
                excepInfo.IsRTL = true;
            }

            filterContext.Result = SetException(excepInfo);
            filterContext.ExceptionHandled = true;

            _logService.Handle(filterContext.Exception, excepInfo.Message);
        }

        private ActionResult SetException(ExceptionInfo exception)
        {
            this.HttpContext.Response.Clear();

            this.HttpContext.Response.StatusCode = exception.StatusCode.Value;

            this.HttpContext.Response.TrySkipIisCustomErrors = true;

            return new JsonResult
            {
                Data = new { Message = exception.Message, Details = exception.Details, IsRTL = exception.IsRTL }
                ,
                RecursionLimit = 3,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult ShowException(ExceptionInfo exception)
        {
            return SetException(exception);
        }

        public void AddModelError(List<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.ErrorMessage, ((string[])(validationResult.MemberNames))[0]);
            }
        }

        [HttpGet]
        public virtual ContentResult CreateHelpView(string viewModelName)
        {
            var filePath = "HelpContent/Help.xml";
            var tokens = this.ControllerContext.RouteData.DataTokens;
            if (tokens != null)
            {
                filePath = string.Format("~/Areas/{0}/{1}", tokens["area"].ToString(), filePath);
            }
            else
            {
                filePath = string.Format("~/{0}", filePath);
            }

            System.Xml.XmlDocument doc = new XmlDocument();

            doc.Load(Server.MapPath(filePath));

            var viewModelHelp = doc.GetElementsByTagName(viewModelName);

            var result = string.Empty;

            foreach (XmlNode item in viewModelHelp)
            {
                result += item.InnerXml;
            }
            return Content(result);
        }

        private CultureInfo SetCurrentCulture(System.Web.Routing.RequestContext requestContext)
        {
            string culture = requestContext.HttpContext.Request.Url.AbsolutePath.Split('/')[1];
            if (culture == "en")
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
            else if (culture == "ar")
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar-SA");
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ar-SA");
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fa-IR");
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fa-IR");
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            SetCurrentCulture(requestContext);
            base.Initialize(requestContext);
        }

        private string GenerateViewPath(RouteData routeData, string viewName)
        {
            var controllerName = routeData.GetRequiredString("controller");
            var actionName = routeData.GetRequiredString("action");

            if (controllerName.ToLower().Equals("partialviews") && actionName.ToLower().Equals("index"))
            {
                return viewName;
            }

            if (viewName.StartsWith("~/"))
            {
                viewName = viewName.Replace("~/", "~/Areas/Core/");
            }
            else
            {
                if (viewName.Equals(actionName))
                {
                    viewName = string.Format("~/Areas/Core/{0}/{1}", controllerName, actionName);
                }
                else
                {
                    viewName = string.Format("~/Areas/Core/{0}/{1}", controllerName, viewName);
                }
            }
            return viewName;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            object areaName = string.Empty;

            if (filterContext.RouteData.DataTokens.TryGetValue("area", out areaName) &&
                areaName.Equals("Core") && (filterContext.Result is ViewResult || filterContext.Result is PartialViewResult))
            {
                if (filterContext.Result is ViewResult)
                {
                    var view = filterContext.Result as ViewResult;
                    view.ViewName = GenerateViewPath(filterContext.RouteData, view.ViewName);
                    filterContext.Result = view;
                    // view.ExecuteResult(this.ControllerContext);
                }
                else if (filterContext.Result is PartialViewResult)
                {
                    var partialView = filterContext.Result as PartialViewResult;
                    partialView.ViewName = GenerateViewPath(filterContext.RouteData, partialView.ViewName);
                    filterContext.Result = partialView;
                    //partialView.ExecuteResult(this.ControllerContext);
                }
            }

            //base.OnResultExecuted(filterContext);
        }
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var request = AppBase.DependencyInjectionManager.Resolve<IRequest>();

            Core.Cmn.AppBase.AllRequests.TryAdd(Thread.CurrentThread.ManagedThreadId, request);
            if (request != null) request.UserRequest = new UserControllerRequest(requestContext.HttpContext.Request);
            return base.BeginExecute(requestContext, callback, state);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IRequest request = null;
            Core.Cmn.AppBase.AllRequests.TryRemove(Thread.CurrentThread.ManagedThreadId, out request);
        }
    }
}