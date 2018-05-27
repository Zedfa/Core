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

            var viewElementInfo = AppBase.GetMenuItemPathByUniqueName(CustomMembershipProvider.GetUserIdCookie() ?? 0, partialViewFileName);

            if (viewElementInfo != null)
            {
                if (viewElementInfo.ElementType == ElementType.Page)
                {
                    return PartialView(string.Format("{0}", viewElementInfo.Url));
                }
                else
                {

                    var currentUrl = (viewElementInfo.Url.Split('/'));
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

          
            Core.Cmn.AppBase.LogService.Handle(new NotSupportedException("unauthorize"), $"partialview {partialViewFileName} UnAuthorized");
            Response.SuppressFormsAuthenticationRedirect = true;
            return new HttpStatusCodeResult(statusCode: System.Net.HttpStatusCode.Unauthorized);
        }


    }
}