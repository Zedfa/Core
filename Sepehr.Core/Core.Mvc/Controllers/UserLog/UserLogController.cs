using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Mvc.Controller;

namespace Core.Mvc.Controllers
{
    public class UserLogController : ControllerBaseCr
    {
        //
        // GET: ControlPanel/UserLog

        public ActionResult Index()
        {
            return View();
        }

    }
}
