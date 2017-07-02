using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Cmn.Attributes;


namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IUserProfileService), DomainName = "Core")]
    public class UserProfileService : ServiceBase<UserProfile>, IUserProfileService
    {
        public UserProfileService(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
            _repositoryBase = new UserProfileRepository(dbContextBase);
        }

        public UserProfileService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new UserProfileRepository(dbContextBase, userLog);
        }
        //public void AddOnlineUsers(string userName)
        //{
        //    lock (appBase.OnlineUsers)
        //    {
        //        string _userName = userName.ToLower();
        //        var userProf = Find(u => u.UserName.ToLower().Equals(_userName));
        //        if (appBase.OnlineUsers.ToList().Any(u => u.UserName.ToLower().Equals(_userName)))
        //        {
        //            //Maybe same users loged in together...
        //            return;
        //        }
        //        appBase.OnlineUsers.Add(userProf);
        //    }
        //}
        public void AddOnlineUsers(UserProfile userProfile)
        {
            lock (appBase.OnlineUsers)
            {

                if (appBase.OnlineUsers.ToList().Any(u => u.UserName.ToLower().Equals(userProfile.UserName)))
                {
                    //Maybe same users loged in together...
                    return;
                }
                appBase.OnlineUsers.Add(userProfile);
            }
        }

        public void RemoveOnlineUsers(string userName)
        {
            appBase.OnlineUsers.RemoveAll(p => p.UserName.ToLower().Equals(userName.ToLower()));
        }

        public override IQueryable<UserProfile> Filter(System.Linq.Expressions.Expression<System.Func<UserProfile, bool>> predicate, bool allowFilterDeleted = true)
        {
            return (_repositoryBase as UserProfileRepository).Filter(predicate, allowFilterDeleted);
        }


        public UserProfile GetUserProfile(string userName)
        {
            return (_repositoryBase as UserProfileRepository).GetUserProfile(userName);
        }
    }
}
