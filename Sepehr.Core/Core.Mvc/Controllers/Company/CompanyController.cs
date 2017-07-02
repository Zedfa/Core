using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    public class CompanyController : ControllerBaseCr
    {
        //
        // GET: ControlPanel/Company

        public ActionResult Index()
        {
            return View();
        }

    }
}
