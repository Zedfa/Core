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



namespace Core.Mvc.ApiControllers
{
    public class AccountApiController : Core.Mvc.Controller.ApiControllerBase
    {

        private readonly IAuthentication _formsService;
        //private readonly MembershipProvider Provider;
        private readonly IMembership _membershipService;
        private readonly IViewElementRoleService _viewElementRoleService;
        private readonly ICompanyChartService _companyChartService;
        private IUserService _userService;
        private IServiceBase<CoreUserLog> _userLogService;
        private IUserProfileService _userprofileService;
        private IConstantService _constantService;
        private IDomainAuthenticationService _domainAuthenticationService;

        public AccountApiController(IAuthentication formsService, IViewElementRoleService viewElementRoleService, ICompanyChartService CompanyChartService,
            IUserService userService, IServiceBase<CoreUserLog> userLogService, IUserProfileService userprofileService, IConstantService constantService, IDomainAuthenticationService domainAuthenticationService)
        {

            // _membershipService = new AccountMembership(Provider);
            _membershipService = new AccountMembership(null);

            _formsService = formsService;

            _viewElementRoleService = viewElementRoleService;
            _companyChartService = CompanyChartService;
            _userService = userService;
            _userLogService = userLogService;
            _userprofileService = userprofileService;
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
                // string serverEncryptedKey = CaptchaValue + "-" + encryptionKey;
                string serverEncryptedKey = model.CaptchaCode + "-" + encryptionKey;

                serverEncryptedKey = EncryptionUtil.Sha1Util.Sha1HashString(serverEncryptedKey);


                if (serverEncryptedKey == model.HiddenId)
                {

                    if (ModelState.IsValid)
                    {
                        string newUserName;
                        if (AuthorizeWithDomain(model.UserName, out newUserName))
                        {
                            string domain = _constantService.All().FirstOrDefault(r =>
                                r.Key == "DomainName")?.Value?.Trim();
                            if (String.IsNullOrWhiteSpace(domain))
                            {
                                statusCode = System.Net.HttpStatusCode.BadRequest;

                                _constantService.TryGetValue<string>("DomainNotDefined", out exceptionMsg);
                                ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                //ModelState.AddModelError("UserIsNotValid", ExceptionMessage.DomainNotDefined);

                                return Request.CreateErrorResponse(statusCode, ModelState);
                            }
                            List<string> allowedRoles = _constantService.All().FirstOrDefault(r =>
                                r.Key == "AllowedRolesForDomainUsers")?.Value?.Split(',').Where(r => !String.IsNullOrWhiteSpace(r.Trim())).ToList();
                            if (allowedRoles == null || allowedRoles.Count == 0)
                            {
                                statusCode = System.Net.HttpStatusCode.BadRequest;
                                _constantService.TryGetValue<string>("NoRoleIsNotAllowed", out exceptionMsg);
                                ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                //ModelState.AddModelError("UserIsNotValid", ExceptionMessage.NoRoleIsNotAllowed);
                                return Request.CreateErrorResponse(statusCode, ModelState);
                            }
                            List<string> userRoles;
                            if (_domainAuthenticationService.Logon(newUserName, model.Password, domain, out fullName, out userRoles))
                            {
                                if (!allowedRoles.Any(r => userRoles.Contains(r.Trim().ToUpper())))
                                {
                                    statusCode = System.Net.HttpStatusCode.BadRequest;
                                    _constantService.TryGetValue<string>("UserIsNotInPermittedRole", out exceptionMsg);
                                    ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                    //ModelState.AddModelError("UserIsNotValid", ExceptionMessage.UserIsNotInPermittedRole);
                                    return Request.CreateErrorResponse(statusCode, ModelState);
                                }
                            }
                            else
                            {
                                statusCode = System.Net.HttpStatusCode.BadRequest;
                                _constantService.TryGetValue<string>("UserIsNotExistInDomain", out exceptionMsg);
                                ModelState.AddModelError("UserIsNotValid", exceptionMsg);

                               // ModelState.AddModelError("UserIsNotValid", ExceptionMessage.UserIsNotExistInDomain);
                                return Request.CreateErrorResponse(statusCode, ModelState);
                            }
                        }
                        else if (_membershipService.ValidateUser(model.UserName, model.Password) && _userService.HasAdminRecord())
                        {
                            //Check User Is Valid or Not...
                            if (!_userService.IsUserActive(model.UserName))
                            {
                                statusCode = System.Net.HttpStatusCode.BadRequest;
                                _constantService.TryGetValue<string>("UserIsNotActive", out exceptionMsg);
                                ModelState.AddModelError("UserIsNotValid", exceptionMsg);
                                //ModelState.AddModelError("UserIsNotValid", ExceptionMessage.UserIsNotActive);
                                return Request.CreateErrorResponse(statusCode, ModelState);
                            }
                            var modifiedUserName = model.UserName.CorrectPersianChars();
                            UserProfile userProfile = _userprofileService.Filter(profile => profile.UserName.ToLower().Equals(modifiedUserName.ToLower())).FirstOrDefault();
                            _formsService.SignIn(userProfile, model.RememberMe, true);
                            // var userProfile = _viewElementRoleService.AppBase.OnlineUsers.FirstOrDefault<IUserProfile>(userp => userp.UserName.ToLower() == model.UserName.ToLower().Trim().CurrectPersianChars()) as UserProfile;
                            if (userProfile != null)
                            {
                                var user = userProfile.User;
                                fullName = string.Format("{0} {1}", user.FName, user.LName);

                                //TODO: please uncomment later
                                // AddRecordToUserLog(user);


                                //_viewElementRoleService.SetViewElementGrantedToUser(model.UserName);
                                //_companyChartService.SetCompanyChartInfo(model.UserName);

                            }



                        }
                        else
                        {
                            statusCode = System.Net.HttpStatusCode.BadRequest;
                            _constantService.TryGetValue<string>("IncorrectUserNameOrPassword", out exceptionMsg);
                            ModelState.AddModelError("wrongPassOrUsr", exceptionMsg);
                           // ModelState.AddModelError("wrongPassOrUsr", ExceptionMessage.IncorrectUserNameOrPassword);
                            return Request.CreateErrorResponse(statusCode, ModelState);

                        }
                    }
                }
                else
                {
                    statusCode = System.Net.HttpStatusCode.BadRequest;
                    _constantService.TryGetValue<string>("IncorrectSecurityCode", out exceptionMsg);
                    ModelState.AddModelError("wrongCaptchaCode", exceptionMsg);
                    //ModelState.AddModelError("wrongCaptchaCode", ExceptionMessage.IncorrectSecurityCode);
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

        private bool AuthorizeWithDomain(string userName, out string newUserName)
        {
            newUserName = String.Empty;
            if (userName.Contains(@"\"))
            {
                newUserName = userName.Substring(userName.IndexOf('\\') + 1);
                return true;
            }
            else if (userName.Contains("@"))
            {
                newUserName = userName.Substring(0, userName.IndexOf('@'));
                return true;
            }
            return false;
        }

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

            bool isPassCodeValidate = CustomMembershipProvider.ValidatePassCode(CustomMembershipProvider.GetPassCodeCookie());

            bool isOnlineUser = Core.Service.ServiceBase.appBase.OnlineUsers.Exists(user => user.UserName.ToLower().Trim() == User.Identity.Name.ToLower().Trim());

            return isPassCodeValidate && userId.HasValue && isOnlineUser;


        }


    }
}