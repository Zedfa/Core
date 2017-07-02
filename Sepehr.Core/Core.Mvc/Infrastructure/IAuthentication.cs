using Core.Entity;
namespace Core.Mvc.Infrastructure
{
    public interface IAuthentication
    {
        void SignIn(UserProfile userProfile, bool createPersistentCookie, bool withCookie);
        void SignOut(string userName, bool redirectToLoginPage = true);
       
    }
}
