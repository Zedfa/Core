using Core.Cmn;
using Core.Entity;
using Core.Service;
using Core.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;


namespace Core.Mvc.Infrastructure
{

    public class AuthorizationBase : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            object area;
            bool hasArea = httpContext.Request.RequestContext.RouteData.DataTokens.TryGetValue("area", out area);

            var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            var action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();


            var viewElementService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IViewElementService>();

            var requestedUrl = hasArea ? string.Format("{0}/{1}/{2}", area, controller, action) : string.Format("{0}/{1}", controller, action);


            if (httpContext.Request.QueryString.Count > 0)
            {
                var rawUrl = httpContext.Request.RawUrl;
                var queryString = rawUrl.Substring(rawUrl.IndexOf("?"));
                requestedUrl = string.Format("{0}{1}", requestedUrl, queryString);

            }
            var authentication = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IAuthentication>();
            int? userId = CustomMembershipProvider.GetUserIdCookie();
            if (userId == null)
            {
                if (viewElementService.HasAnonymousAccess(requestedUrl))
                    return true;

                authentication.SignOut(httpContext.User.Identity.Name);
                return false;

            }


            var userProfileService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();

            UserProfile foundUserProfile = userProfileService.Filter(entity => entity.Id.Equals(userId.Value)).FirstOrDefault();

            if (foundUserProfile != null)
            {
                var encodedUserName = Security.GetMd5Hash(MD5.Create(), foundUserProfile.UserName);

                var passCode = Security.GetMd5Hash(MD5.Create(), string.Format("{0}{1}", encodedUserName, foundUserProfile.Password));

                if (CustomMembershipProvider.ValidatePassCode(passCode))
                {
                    if (!userProfileService.AppBase.OnlineUsers.Any(u => u.UserName.ToLower().Equals(foundUserProfile.UserName)))
                    {
                        authentication.SignIn(foundUserProfile, true, true);
                    }
                    return viewElementService.RoleHasAccess(foundUserProfile.Id, requestedUrl);
                }
            }
            return false;

        }
    }
}
