using Core.Mvc.Controller;
using Core.Mvc.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Service;
using Core.Mvc.Infrastructure;
using Core.Entity;

namespace Core.Mvc.Controllers
{
    public class AccountController : ControllerBaseCr
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Core/Account
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult LogOn()
        {
            return PartialView("_LogOn", new LogOnViewModel());
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ChangeUserPassword()
        {
            var userId = CustomMembershipProvider.GetUserIdCookie();
            string userName = "";
            if (userId != null)
            {
                var user = _userService.GetUserAndUserProfileByUserId(userId ?? 0);
                userName = user.UserProfile.UserName;
            }

            ViewBag.name = userName;
            return View();

        }
    }
}