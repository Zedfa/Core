using System;
using System.Linq;
using Core.Cmn.Extensions;
using Core.Entity;
using Core.Cmn.Attributes;
using Core.Cmn;

namespace Core.Rep
{

    public class UserProfileRepository : RepositoryBase<UserProfile>
    {
        public UserProfileRepository(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
        }
        public UserProfileRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
        }

        [Cacheable(EnableUseCacheServer = false, ExpireCacheSecondTime = 5, EnableAutomaticallyAndPeriodicallyRefreshCache = true, EnableToFetchOnlyChangedDataFromDB = true, NameOfNavigationPropsForFetchingOnlyChangedDataFromDB = "User")]
        public static IQueryable<UserProfile> AllUserProfileCache(IQueryable<UserProfile> query)
        {
            return query.AsNoTracking().Include("User");
        }

        public override IQueryable<UserProfile> All(bool canUseCacheIfPossible = true)
        {
            return Cache(AllUserProfileCache, canUseCacheIfPossible);
        }

        public override IQueryable<UserProfile> Filter(System.Linq.Expressions.Expression<Func<UserProfile, bool>> predicate, bool allowFilterDeleted = true)
        {
            return allowFilterDeleted ? All().Where(predicate).AsQueryable() : base.Filter(predicate);
        }

        public UserProfile GetUserProfile(string userName)
        {
            var userProfile = this.Filter(u => u.UserName.Equals(userName)).Include("User").SingleOrDefault();
            return userProfile;
        }
    }
}
