using Core.Cmn;
using Core.Mvc.Attribute;

using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Core.Mvc.Controller
{
    [ExceptionHandler]
    [AddCustomHeader]
    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase()
        {
        }

        private List<Message> _messageStrore;

        public List<Message> MessageStrore
        {
            get
            {
                if (_messageStrore == null)
                {
                    _messageStrore = new List<Message>();
                }
                return _messageStrore;
            }
            set
            {
                _messageStrore.AddRange(value);
            }
        }

      
        protected virtual CultureInfo GetCurrentCulture()
        {
            if (Request == null)
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fa-IR");
            string culture = Request.RequestUri.AbsolutePath.Split('/')[1];
            if (culture == "en")
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
            else if (culture == "ar")
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ar-SA");
            }
            else
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fa-IR");
            }
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            GetCurrentCulture();
            SetUserRequestInfo();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IRequest request = null;
            Core.Cmn.AppBase.AllRequests.TryRemove(Thread.CurrentThread.ManagedThreadId, out request);
        }
        private void SetUserRequestInfo()
        {
            var request = AppBase.DependencyInjectionManager.Resolve<IRequest>();
            Core.Cmn.AppBase.AllRequests.TryAdd(Thread.CurrentThread.ManagedThreadId, request);
            if (request != null) request.UserRequest = new UserApiRequest(Request);

        }
    }
}