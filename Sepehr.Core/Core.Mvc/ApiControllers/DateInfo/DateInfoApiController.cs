using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers.DateInfo
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
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
