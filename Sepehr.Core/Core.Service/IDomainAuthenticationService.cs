using Core.Entity;
using System.Collections.Generic;


namespace Core.Service
{
    public interface IDomainAuthenticationService
    {
        bool Logon(string userName, string password, string domain, out string fullName, out List<string> roles);
        //void GetAllDCUsers(string userName, string password, string domain);
        //bool ValidateUser(UserProfileDTO info);
        bool ValidateUser(UserProfile info);
    }
}
