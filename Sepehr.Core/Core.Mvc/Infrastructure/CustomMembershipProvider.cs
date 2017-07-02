using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Core.Entity;
using Core.Cmn;
using Core.Service;
using System.Web;
using System.Security.Cryptography;
using Core.Cmn;


namespace Core.Mvc.Infrastructure
{

    public class CustomMembershipProvider : MembershipProvider
    {

        readonly IUserService _userService;
        readonly IUserProfileService _profileservice;

        public CustomMembershipProvider()
        {

            _profileservice = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();
        }


        public CustomMembershipProvider(IUserService userService, IUserProfileService profileservice)
        {
            _userService = userService;
            _profileservice = profileservice;
        }


        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = _profileservice.Find(u => u.UserName.Equals(username));
            if (user != null)
            {
                var memUser = new MembershipUser("CustomMembershipProvider", username, user.Id, string.Empty,
                                                             string.Empty, string.Empty,
                                                             true, false, DateTime.MinValue,
                                                             DateTime.MinValue,
                                                             DateTime.MinValue,
                                                            DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }


        public override bool ValidateUser(string username, string password)
        {
            var md5Hash = GetMd5Hash(password);
            var requiredUser = _profileservice.Find(u => u.UserName.Equals(username) && u.Password.Equals(md5Hash));
            return requiredUser != null;

        }


        public static string GetMd5Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            throw new NotImplementedException();
        }


        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var args = new ValidatePasswordEventArgs(username, oldPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                return false;
            }
            if (ValidateUser(username, oldPassword))
            {
                var userprofile = _profileservice.Find(u => u.UserName.Equals(username));
                userprofile.Password = GetMd5Hash(newPassword);
                _profileservice.Update(userprofile);
                return true;
            }
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }


        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }


        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }


        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }


        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }


        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }


        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }


        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }


        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }


        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }


        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public int MinPasswordLength { get; private set; }


        public static CustomMembershipProvider Provider { get; set; }


        public static int? GetUserIdCookie()
        {
            int result = 0;
            var userIdCookie = System.Web.HttpContext.Current.Request.Cookies.Get(UserIdCookieName);
            if (userIdCookie != null && Int32.TryParse(userIdCookie.Value, out result))
                return result;
            else
                return null;

        }


        public static void SetUserIdCookie(string id)
        {
            //var authCookieExpireDays = new ConstantService().TryGetValueByKey<double>("AuthCookieExpireDays");
            //AuthCookieExpireDays = new ConstantService().TryGetValueByKey<double>("AuthCookieExpireDays");

            if (!string.IsNullOrEmpty(id))
            {
                var userIdCookie = new HttpCookie(UserIdCookieName, id);
                //userIdCookie.Expires = DateTime.Now.AddYears(1);
                userIdCookie.Expires = DateTime.Now.AddMinutes(AuthCookieExpireDays == 0 ? 30 : AuthCookieExpireDays);
                System.Web.HttpContext.Current.Response.Cookies.Set(userIdCookie);
            }
            else
            {
                throw new Exception("u must set id before call this method");
            }
        }

        public static string GetPassCodeCookie()
        {
            var PassCodeCookie = System.Web.HttpContext.Current.Request.Cookies.Get(PassCodeCookieName);
            if (PassCodeCookie != null)
            {
                return PassCodeCookie.Value;
            }
            return null;

        }

        public static void SetPassCodeCookie(string userName, string encodedPassWord)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(encodedPassWord))
            {
                var encodedUserName = Security.GetMd5Hash(MD5.Create(), userName);
                // var encodedPassWord = Security.GetMd5Hash(MD5.Create(), password);

                var PassCodeCookie = new HttpCookie(PassCodeCookieName, Security.GetMd5Hash(MD5.Create(), string.Format("{0}{1}", encodedUserName, encodedPassWord)));
                PassCodeCookie.Expires = DateTime.Now.AddYears(1);
                System.Web.HttpContext.Current.Response.Cookies.Set(PassCodeCookie);
            }
            else
            {
                throw new Exception("u must set UserName and Password before call this method");

            }


        }

        public static bool ValidatePassCode(string key)
        {
            var passCode = GetPassCodeCookie();
            if (passCode != null)
            {
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                return comparer.Compare(key, passCode) == 0;

            }
            return false;
        }

        public static void ClearMembershipCookie(string name)
        {

            HttpCookie cookie = new HttpCookie(name, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string UserIdCookieName
        {
            get
            {
                return "x";
            }
        }

        public static string PassCodeCookieName
        {
            get
            {
                return "xx";
            }
        }

        public static double AuthCookieExpireDays
        {
            get
            {
                return 365;
            }
        }

    }
}