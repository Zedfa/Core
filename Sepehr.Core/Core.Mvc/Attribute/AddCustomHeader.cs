using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Core.Mvc.Attribute
{
    public class AddCustomHeader : ActionFilterAttribute
    {

        public override System.Threading.Tasks.Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            var messageStorage = (actionExecutedContext.ActionContext.ControllerContext.Controller as Core.Mvc.Controller.ApiControllerBase).MessageStrore;
            foreach (var message in messageStorage)
            {
                //actionExecutedContext.Response.Headers.Add(Enum.GetName(typeof(MessageType), message.type), message.text);
                // var mediaType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
                //mediaType.CharSet = "utf-8";
                // actionExecutedContext.Response.Content.Headers.ContentType =mediaType;
                //var a = System.Text.Encoding.UTF8.GetBytes(message.text);
                // var b = Convert.ToBase64String( a);
                //var  a = System.Text.ASCIIEncoding.ASCII.GetBytes(message.text);
                // var b = System.Convert.ToBase64String(a);

                actionExecutedContext.Response.Headers.Add(Enum.GetName(typeof(MessageType), message.type), message.text);


            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

    }
}