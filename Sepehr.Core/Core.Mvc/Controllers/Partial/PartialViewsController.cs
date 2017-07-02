using Core.Entity.Enum;
using Core.Mvc.Controller;
using Core.Mvc.Infrastructure;
using Core.Service;
using Core.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Core.Mvc.Controllers
{
    public class PartialViewsController : ControllerBaseCr//System.Web.Mvc.Controller
    {
        private readonly IViewElementRoleService _viewElementRoleService;
      //  private IUserProfileService _userProfileService;
        public PartialViewsController(IViewElementRoleService viewElementRoleService)
        {
            _viewElementRoleService = viewElementRoleService;
         //   _userProfileService = userProfileService;
        }


        //
        // GET: /PartialView/
        public ActionResult Index(string partialViewFileName)
        {
            //if (_viewElementRoleService.AppBase.ViewElementsGrantedToAnonymousUser == null)
            //{
            //    var userProfile = _userProfileService.Filter(entity => entity.UserName.ToLower().Equals("anonymous")).FirstOrDefault();
            //    var viewElements = _viewElementRoleService.GetViewElementGrantedToUser(userProfile);

            //    _viewElementRoleService.AppBase.ViewElementsGrantedToAnonymousUser = new UserViewElement { UserName = "anonymous", ViewElements = viewElements };
            //}



            var menuItemId = AppBase.GetMenuItemPathByUniqueName(CustomMembershipProvider.GetUserIdCookie()??0, partialViewFileName);


            if (menuItemId.ElementType == ElementType.Page)
            {
                return PartialView(string.Format("{0}", menuItemId.Url));
            }
            else
            {

                var currentUrl = (menuItemId.Url.Split('/'));
                var area = string.Empty;
                var controller = string.Empty;
                var action = string.Empty;

                if (currentUrl.Length == 3)
                {
                    area = currentUrl[0];
                    controller = currentUrl[1];
                    action = currentUrl[2];
                }
                else
                {
                    controller = currentUrl[0];
                    action = currentUrl[1];
                }
                //var domain = url.Scheme + Uri.SchemeDelimiter + url.Host + ":" + url.Port + "/";
                return RedirectToAction(action, controller, new { area = area });
            }


        }

        //if(partialViewFileName=="common")
        //{
        //    return PartialView("~/areas/common/views/partialviews/DirectiveTemplates/Common.cshtml");
        //}
        //if (partialViewFileName == "menu")
        //{
        //    return PartialView("~/areas/core/views/partialviews/DirectiveTemplates/menu.cshtml");
        //}
    }
}