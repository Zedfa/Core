using System.Web.Security;

namespace Core.Mvc.Infrastructure
{
   
    public interface IMembership
    {
        //int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
