using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Core.Mvc.ApiControllers.DateInfo
{
    public class DateInfoApiController : ApiControllerBase
    {
        public DateInfoApiController()
        {

        }

        public string GetTodaysShamsiDate()
        {
            return Core.Cmn.Extensions.DateExt.MiladiToShamsi(DateTime.Now);
        }
        public string GetTodaysMiladiDate()
        {
            return DateTime.Now.ToShortDateString();
        }
    }
}
