using Core.Mvc.Attribute;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Collections.Generic;

namespace Core.Mvc.Controller
{
    [ExceptionHandler]
    [AddCustomHeader]
    public class ApiControllerBase : ApiController
    {
        public ApiControllerBase()
        {
            GetCurrentCulture();
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
        }
    }



}
