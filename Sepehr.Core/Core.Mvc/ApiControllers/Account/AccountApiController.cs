using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Core.Cmn.Extensions;
using System.Web;
using Core.Service;
using Core.Entity.Enum;
using Core.Mvc.Infrastructure;
using Core.Mvc.ViewModel.Account;
using System.Configuration;
using Core.Cmn;
using System.Security.Cryptography;

namespace Core.Mvc.ApiControllers
{
    public class AccountApiController : Core.Mvc.Controller.ApiControllerBase
    {

        private readonly IAuthentication _formsService;
        private readonly IMembership _membershipService;
        private readonly IViewElementRoleService _viewElementRoleService;
        private readonly ICompanyChartService _companyChartService;
        private IUserService _userService;
        private IServiceBase<CoreUserLog> _userLogService;
        private IUserProfileService _userProfileService;
        private IConstantService _constantService;
        private IDomainAuthenticationService _domainAuthenticationService;

        public AccountApiController(IAuthentication formsService, IViewElementRoleService viewElementRoleService, ICompanyChartService CompanyChartService,
            IUserService userService, IServiceBase<CoreUserLog> userLogService, IUserProfileService userprofileService, IConstantService constantService, IDomainAuthenticationService domainAuthenticationService)
        {

            _membershipService = new AccountMembership();

            _formsService = formsService;

            _viewElementRoleService = viewElementRoleService;
            _companyChartService = CompanyChartService;
            _userService = userService;
            _userLogService = userLogService;
            _userProfileService = userprofileService;
            _constantService = constantService;
            _domainAuthenticationService = domainAuthenticationService;
        }


        public HttpResponseMessage PostEntity([FromBody]LogOnViewModel model)
        {
            string fullName = string.Empty;
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            var exceptionMsg = string.Empty;
            var encryptionKey = string.Empty;
            if (_constantService.TryGetValue<string>("EncryptionKey", out encryptionKey))
            {
                string serverEncryptedKey = model.CaptchaCode + "-" + encryptionKey;

                serverEncryptedKey = EncryptionUtil.Sha1Util.Sha1HashString(serverEncryptedKey);


                if (serverEncryptedKey == model.HiddenId)
                {

                    if (ModelState.IsValid)
                    {
                        //string newUserName;
                        //if (AuthorizeWithDomain(model.UserName, out newUserName))
                        //if(!string.IsNullOrEmpty( model.Domain))
                        //{
                            //string domain = _constantService.All().FirstOrDefault(r =>
                            //    r.Key == "DomainName")?.Value?.Trim();
                            //if (String.IsNullOrWhiteSpace(domain))
                            //{
                            //    statusCode = System.Net.HttpStatusCode.BadRequest;

                            //    _constantService.TryGetValue<string>("DomainNotDefined", out exceptionMsg);
                            //    ModelState.AddModelError("UserIsNotValid", exceptionMsg);

                            //    return Request.CreateErrorResponse(statusCode, ModelState);
                            //}
                            //List<string> allowedRoles = _constantService.All().FirstOrDefault(r =>
                            //    r.Key == "AllowedRolesForDomainUsers")?.Value?.Split(',').Where(r => !String.IsNullOrWhiteSpace(r.Trim())).ToList();
                            //if (allowedRoles == null || allowedRoles.Count == 0)
                            //{
                            //    statusCode = System.Net.HttpStatusCode.BadRequest;
                            //    _constantService.TryGetValue<string>("NoRoleIsNotAllowed", out exceptionMsg);
                            //    ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                            //    return Request.CreateErrorResponse(statusCode, ModelState);
                            //}
                            //List<string> userRoles;
                           
                            //if (_domainAuthenticationService.Logon(model.UserName, model.Password, model.Domain, out fullName, out userRoles))
                            //{
                                //if (!allowedRoles.Any(r => userRoles.Contains(r.Trim().ToUpper())))
                                //{
                                //    statusCode = System.Net.HttpStatusCode.BadRequest;
                                //    _constantService.TryGetValue<string>("UserIsNotInPermittedRole", out exceptionMsg);
                                //    ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                //    return Request.CreateErrorResponse(statusCode, ModelState);
                                //}
                        //    }
                        //    else
                        //    {
                        //        statusCode = System.Net.HttpStatusCode.BadRequest;
                        //        _constantService.TryGetValue<string>("UserIsNotExistInDomain", out exceptionMsg);
                        //        ModelState.AddModelError("UserIsNotValid", exceptionMsg);

                        //        return Request.CreateErrorResponse(statusCode, ModelState);
                        //    }
                        //}
                         if (_membershipService.ValidateUser(model.UserName, model.Password) && _userService.HasAdminRecord())
                        {
                            //Check User Is Valid or Not...
                            if (!_userService.IsUserActive(model.UserName))
                            {
                                statusCode = System.Net.HttpStatusCode.BadRequest;
                                _constantService.TryGetValue<string>("UserIsNotActive", out exceptionMsg);
                                ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                return Request.CreateErrorResponse(statusCode, ModelState);
                            }
                            var modifiedUserName = model.UserName.CorrectPersianChars();
                            UserProfile userProfile = _userProfileService.Filter(profile => profile.UserName.ToLower().Equals(modifiedUserName.ToLower())).FirstOrDefault();
                            _formsService.SignIn(userProfile, model.RememberMe, true);
                            if (userProfile != null)
                            {
                                var user = userProfile.User;
                                fullName = string.Format("{0} {1}", user.FName, user.LName);

                                //TODO: please uncomment later
                                // AddRecordToUserLog(user);                                            
                            }



                        }
                        else
                        {
                            statusCode = System.Net.HttpStatusCode.BadRequest;
                            _constantService.TryGetValue<string>("IncorrectUserNameOrPassword", out exceptionMsg);
                            ModelState.AddModelError("wrongPassOrUsr", exceptionMsg);
                            return Request.CreateErrorResponse(statusCode, ModelState);

                        }
                    }
                }
                else
                {
                    statusCode = System.Net.HttpStatusCode.BadRequest;
                    _constantService.TryGetValue<string>("IncorrectSecurityCode", out exceptionMsg);
                    ModelState.AddModelError("wrongCaptchaCode", exceptionMsg);
                    return Request.CreateErrorResponse(statusCode, ModelState);

                }
            }
            else
            {
                statusCode = System.Net.HttpStatusCode.BadRequest;
                ModelState.AddModelError("encryptionKey", "there is no encryptionKey");
                return Request.CreateErrorResponse(statusCode, ModelState);

            }

            return Request.CreateResponse(statusCode, new { fullName = fullName, url = ConfigurationManager.AppSettings["RedirectUrlAfterLogin"] });

        }

        //private bool AuthorizeWithDomain(string userName, out string newUserName)
        //{
        //    newUserName = String.Empty;
        //    if (userName.Contains(@"\"))
        //    {
        //        newUserName = userName.Substring(userName.IndexOf('\\') + 1);
        //        return true;
        //    }
        //    else if (userName.Contains("@"))
        //    {
        //        newUserName = userName.Substring(0, userName.IndexOf('@'));
        //        return true;
        //    }
        //    return false;
        //}

        public HttpResponseMessage PutEntity()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                _formsService.SignOut(this.User.Identity.Name);
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);



        }
        private void AddRecordToUserLog(User user)
        {

            var userLog = new CoreUserLog
            {
                DateTime = DateTime.Now,
                Ip = HttpContext.Current.Request.UserHostAddress,
                LogType = LogType.Login,
                UserId = user.Id,
            };

            _userLogService.Create(userLog);
        }

        public bool GetUserHassAccess()
        {
            int? userId = CustomMembershipProvider.GetUserIdCookie();
            var isPassCodeValidate = CustomMembershipProvider.IsCurrentUserAuthenticate();
            //if (userId != null)
            //{
            //    UserProfile foundUserProfile = _userProfileService.Filter(entity => entity.Id.Equals(userId.Value)).FirstOrDefault();

            //    if (foundUserProfile != null)
            //    {
            //        var encodedUserName = Security.GetMd5Hash(MD5.Create(), foundUserProfile.UserName);

            //        var passCode = Security.GetMd5Hash(MD5.Create(), string.Format("{0}{1}", encodedUserName, foundUserProfile.Password));
            //        isPassCodeValidate = CustomMembershipProvider.ValidatePassCode(passCode);
            //    }

            //}

            bool isOnlineUser = Core.Service.ServiceBase.appBase.OnlineUsers.Exists(user => user.UserName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());

            return isPassCodeValidate && userId.HasValue && isOnlineUser;


        }


    }
}