using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Kendo.Mvc.Extensions;
using Core.Entity;
using Core.Service;
using Core.Cmn;

namespace Core.Mvc.Controllers
{
    public class RoleController : ControllerBaseCr
    {
        


        private IServiceBase<Role> _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        // GET: ControlPanel/Role
        public ActionResult Index()
        {
            return View();
        }








    }
}