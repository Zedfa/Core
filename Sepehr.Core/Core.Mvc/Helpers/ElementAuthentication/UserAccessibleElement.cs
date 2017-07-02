using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Infrastructure;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Core.Mvc.Helpers.ElementAuthentication
{
    [Serializable]
    public class UserAccessibleElement
    {
        public static void DefineCrudActionAuthority( AccessOperation crudOpt,CrudCr crudInfo)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                //object area = null;
                //var tokens = System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
                //if (tokens.TryGetValue("area", out area) )
                //{
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.StatusCode = 403;//forbidden

                //}
                //else
                //{
                //    FormsAuthentication.RedirectToLoginPage();
                //}
                return;
            }
            else
            {
                var currentUserId = CustomMembershipProvider.GetUserIdCookie() ?? 0;
                var readUrl = crudInfo.Read.Url.ToLower();
                if (readUrl.StartsWith("api/") || readUrl.StartsWith("/api/"))
                {
                    var originalUrl = readUrl.Split('/');
                    var actualUrlName = string.Empty;
                    //has Area Name
                    if (originalUrl.Length == 3)
                    {
                        if (originalUrl[2].ToLower().Equals("getentities"))
                        {
                            actualUrlName = originalUrl[0] + "/" + originalUrl[1];
                        }
                        else
                        {
                            actualUrlName = originalUrl[1] + "/" + originalUrl[2];
                        }
                    }
                 
                    else
                    {
                        if (originalUrl.Length == 4)
                        {
                            actualUrlName = originalUrl[1] + "/" + originalUrl[2];
                        }
                        else
                        {
                            actualUrlName = originalUrl[1];
                        }
                    }

                    if (crudOpt.Insertable)
                    {
                        var insertUrl = string.IsNullOrEmpty(crudInfo.Insert.Url) ? actualUrlName + "/PostEntity" : crudInfo.Insert.Url;
                        crudOpt.Insertable = AppBase.HasCurrentUserAccess(currentUserId, insertUrl);

                    }

                    if (crudOpt.Updatable)
                    {
                        var updateUrl = string.IsNullOrEmpty(crudInfo.Update.Url) ? actualUrlName + "/PutEntity" : crudInfo.Update.Url;

                        crudOpt.Updatable = AppBase.HasCurrentUserAccess(currentUserId, updateUrl);

                    }

                    if (crudOpt.Removable)
                    {
                        var removeUrl = string.IsNullOrEmpty(crudInfo.Remove.Url) ? actualUrlName + "/DeleteEntity" : crudInfo.Remove.Url;

                        crudOpt.Removable = AppBase.HasCurrentUserAccess(currentUserId, removeUrl);
                    }
                    
                }
                else
                {
                    // Must be implemented for classical controller. 
                    throw new NotImplementedException();
                }
            }
        }

        public static bool HasCustomActionAuthorized(string customActionName)
        {
            var currentUser = CustomMembershipProvider.GetUserIdCookie() ?? 0;
            return AppBase.HasCurrentUserAccess(currentUser, null, customActionName);
        }


    }
}
