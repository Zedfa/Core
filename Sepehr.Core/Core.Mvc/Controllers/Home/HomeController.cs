using Core.Entity;
using Core.Mvc.Controller;
using Core.Mvc.Infrastructure;
using Core.Mvc.ViewModel;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers
{
    public class HomeController : ControllerBaseCr
    {

        // GET: Core/Home
        public ActionResult Index()
        {


            return View();
        }

        public PartialViewResult MainMenu()
        {
            // return PartialView("MainLayoutTemplates/TopMainMenu",new  TopMenuViewModel());
            //int? userId = CustomMembershipProvider.GetUserIdCookie();
            //bool isPassCodeValidate = CustomMembershipProvider.ValidatePassCode(CustomMembershipProvider.GetPassCodeCookie());
            int? userId = CustomMembershipProvider.GetUserIdCookie();

            bool isPassCodeValidate = CustomMembershipProvider.ValidatePassCode(CustomMembershipProvider.GetPassCodeCookie());

            bool isOnlineUser = Core.Service.ServiceBase.appBase.OnlineUsers.Exists(user => user.UserName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());

            if (isPassCodeValidate && userId.HasValue && isOnlineUser)
            {
                return PartialView("MainLayoutTemplates/TopMainMenu", new TopMenuViewModel());
            }
            return null;
        }

       
      
       
    }
}