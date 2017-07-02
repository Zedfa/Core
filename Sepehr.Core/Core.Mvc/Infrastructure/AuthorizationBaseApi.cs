using Core.Cmn;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;



namespace Core.Mvc.Infrastructure
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]

    public class AuthorizationBaseApi : _AuthorizationBaseApi
    {
        protected override bool _IsAuthorized(HttpActionContext actionContext)
        {
            return this.IsAuthorized(actionContext);
        }
        protected new virtual bool IsAuthorized(HttpActionContext actionContext)
        {

            string controller = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = actionContext.ActionDescriptor.ActionName;
            string query = actionContext.Request.RequestUri.Query;
            var queryString = new NameValueCollection(System.Web.HttpUtility.ParseQueryString(query));

            var viewElementService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IViewElementService>();

            var requestedUrl = string.Format("{0}/{1}", controller, action);

            if (queryString.Count > 0)
            {
                var queryParams = MakeUrlParameters(queryString);
                requestedUrl = string.Format("{0}?{1}", requestedUrl, queryParams);

            }

            if (viewElementService.HasAnonymousAccess(requestedUrl))
                return true;

            var userId = CustomMembershipProvider.GetUserIdCookie();

            //if (userId != null)
            //{
            //    var userProfileService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();

            //    var foundUserProfile = userProfileService.Find(userId);
            //    if (foundUserProfile != null)
            //    {
            //        var encodedUserName = Security.GetMd5Hash(MD5.Create(), foundUserProfile.UserName);

            //       var passCode = Security.GetMd5Hash(MD5.Create(), string.Format("{0}{1}", encodedUserName, foundUserProfile.Password));
            //       if (CustomMembershipProvider.ValidatePassCode(passCode))
            //        {

            //           return viewElementService.RoleHasAccess(foundUserProfile.Id, requestedUrl);

            //        }
            //    }
            //}
            if (userId.HasValue && CustomMembershipProvider.IsCurrentUserAuthenticate())
            {
                return viewElementService.RoleHasAccess(userId.Value, requestedUrl);
            }

            return false;
        }

        private string MakeUrlParameters(NameValueCollection queryString)
        {
            var urlParam = string.Empty;
            string queryParams = string.Empty;
            if (queryString.HasKeys() && !string.IsNullOrEmpty(queryString["_"]))
            {
                queryString.Remove("_");
            }


            if (queryString.HasKeys())
            {
                foreach (var qs in queryString)
                {
                    if (qs != null)
                    {
                        urlParam = qs + "=" + queryString[qs.ToString()] + "&";
                    }


                }
                queryParams = urlParam.Remove(urlParam.Length - 1, 1);
            }
            return queryParams;
        }

    }


}