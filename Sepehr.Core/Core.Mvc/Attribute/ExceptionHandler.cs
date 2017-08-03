using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Core.Mvc.Attribute
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        private ILogService _logService = Core.Cmn.AppBase.LogService;
        public override void OnException(HttpActionExecutedContext actionContext)
        {

            base.OnException(actionContext);


            var context = actionContext.Request.Properties["MS_HttpContext"] as HttpContextWrapper;

            Exception exp = actionContext.Exception;

            if (!context.User.Identity.Name.ToLower().Equals("admin"))
            {
                //var _constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
                //var msg = string.Empty;
                //_constantService.TryGetValue<string>("ApplicationFaild", out msg);
                //exp = new Exception(msg /*Core.Resources.ExceptionMessage.ApplicationFaild*/);

                /// clear stack trace
                exp = new Exception($"{exp.Message} {exp.Source}");
            }

            //var eLog = _logService.GetEventLogObj();
            //eLog.UserId = "ApiControllerBase";
            //eLog.CustomMessage = $"Error accured in {actionContext.Request?.RequestUri?.OriginalString};Details: {exp.Message}";
            //eLog.LogFileName = "ApiControllerBaseLog";
            //eLog.OccuredException = actionContext.Exception;
            //_logService.Handle(eLog);
            _logService.Handle(actionContext.Exception, customMessage: $"Error accured in {actionContext.Request?.RequestUri?.OriginalString};Details: {exp.Message}", source: "ApiControllerBaseLog");

            actionContext.Response = actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, exp);

        }

    }
}
