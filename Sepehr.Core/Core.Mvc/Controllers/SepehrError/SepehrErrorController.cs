using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    public class SepehrErrorController : ControllerBaseCr
    {
        // GET: Error
        public ActionResult NotFound()
        {
            return View();
        }
    }
}