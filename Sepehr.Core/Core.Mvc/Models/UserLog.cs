using System.Web;
using System.Web.Mvc;
using Core.Entity;
using Core.Service;
using Core.Cmn.Attributes;

namespace Core.Mvc.Models
{
    [Injectable(InterfaceType = typeof(IUserLog), DomainName = "Core")]
    public class UserLog : IUserLog
    {

        public string GetUserName()
        {
            if (HttpContext.Current == null)
                return null;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return null;
            return HttpContext.Current.User.Identity.Name;//When auth - id stored in cookie
        }

        public int? GetCompanyId()
        {
            var _userLog = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IServiceBase<Core.Entity.CoreUserLog>>();

            return _userLog.AppBase.CompanyId;

        }


        public string GetIpAddress()
        {
            if (HttpContext.Current == null)
                return null;
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}
