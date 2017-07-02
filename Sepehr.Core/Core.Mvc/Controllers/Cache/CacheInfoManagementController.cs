using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    public class CacheInfoManagementController : ControllerBaseCr
    {
        // GET: CacheInfoManagement
        public ActionResult Index()
        {
            return View();
        }
    }
}