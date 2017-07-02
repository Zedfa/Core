using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using Core.Entity;
using Core.Service;
using Core.Service.Models;
using Core.Cmn.Attributes;
using System.Security.Cryptography;
using Core.Cmn;

namespace Core.Mvc.Infrastructure
{
    [Injectable(InterfaceType =typeof(IAuthentication),DomainName ="Core")]
    public class Authentication : IAuthentication
    {
        private IUserProfileService _userProService;
        private ICompanyService _companyService;
        private IViewElementService _viewElementService;
        private IViewElementRoleService _viewElementRoleService;
        private ICompanyChartService _companyChartService;
       

        public Authentication(IUserProfileService userProService, ICompanyService companyService,
            IViewElementService viewElementService, IViewElementRoleService viewElementRoleService, ICompanyChartService companyChartService)
        {
            _userProService = userProService;
            _companyService = companyService;
            _viewElementService = viewElementService;
            _viewElementRoleService = viewElementRoleService;
            _companyChartService = companyChartService;
           
        }


        //public void SignIn(string userName, bool createPersistentCookie, bool withCookie)
        //{
        //    if (withCookie)
        //        FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

        //    _userProService.AddOnlineUsers(userName);

        //    var foundUser = _userProService.Filter(a => a.UserName.ToLower() == userName.ToLower()).SingleOrDefault();
        //    _userProService.AppBase.UserId = foundUser.Id;
        //    _companyService.SetCompany((int)foundUser.User.CompanyChartId);
        //}


        public void SignIn( UserProfile userProfile, bool createPersistentCookie, bool withCookie )
        {
            if (withCookie)
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(userProfile.UserName, createPersistentCookie, int.MaxValue);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                //authCookie.Secure = FormsAuthentication.RequireSSL;
                HttpContext.Current.Response.Cookies.Add(authCookie);

            }

           // var foundUser = _userProService.Filter(a => a.UserName.ToLower() == userName.ToLower()).SingleOrDefault();
            _userProService.AddOnlineUsers(userProfile);
            //_userProService.AppBase.UserId = userProfile.Id;
            _companyService.SetCompany((int)userProfile.User.CompanyChartId);

            _viewElementRoleService.SetViewElementGrantedToUser(userProfile);
            _companyChartService.SetCompanyChartInfo(userProfile.UserName);


            CustomMembershipProvider.SetUserIdCookie(userProfile.Id.ToString());
            CustomMembershipProvider.SetPassCodeCookie(userProfile.UserName, userProfile.IsDCUser? Security.GetMd5Hash(MD5.Create(), userProfile.DCPassword): userProfile.Password);

            Core.Cmn.AppBase.OnAfterUserLogin(EventArgs.Empty);
         

        }
 

        public void SignOut(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            //remove from ViewElementGrantedToUser
            {
                _userProService.RemoveOnlineUsers(userName);
                var userId = CustomMembershipProvider.GetUserIdCookie() ?? 0;
              
                UserViewElement userViewElement = null;
                if (_viewElementService.AppBase.ViewElementsGrantedToUser.TryGetValue(userId, out userViewElement))
                {
                    UserViewElement removedElement = null;
                    _viewElementService.AppBase.ViewElementsGrantedToUser.TryRemove(userId,out removedElement); 
                }
            }
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            // Clear authentication cookie
            CustomMembershipProvider.ClearMembershipCookie(FormsAuthentication.FormsCookieName);

            CustomMembershipProvider.ClearMembershipCookie(CustomMembershipProvider.UserIdCookieName);
            CustomMembershipProvider.ClearMembershipCookie(CustomMembershipProvider.PassCodeCookieName);

            Core.Cmn.AppBase.OnAfterUserSignOut(EventArgs.Empty);
            //if (redirectToLoginPage)
            //FormsAuthentication.RedirectToLoginPage();

        }

        

    }
}
