using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    public class ExceptionLogController : ControllerBaseCr
    {
        // GET: ControlPanel/ExceptionLog
        public ActionResult Index()
        {
            return View();
        }
    }
}