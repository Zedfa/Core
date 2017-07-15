using System;
using System.Linq;
using Core.Cmn.Extensions;
using Core.Entity;
using Core.Cmn.Attributes;
using Core.Cmn;
using Core.Rep.Interface;

namespace Core.Rep
{
    [Injectable(InterfaceType = typeof(IUserProfileRepository), DomainName = "Core")]

    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository 
    {
        
        public UserProfileRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
        }

        [Cacheable(EnableSaveCacheOnHDD = true, EnableUseCacheServer = true, ExpireCacheSecondTime = 5, EnableAutomaticallyAndPeriodicallyRefreshCache = true, EnableToFetchOnlyChangedDataFromDB = true,
            NameOfNavigationPropsForFetchingOnlyChangedDataFromDB = "User")]
        public static IQueryable<UserProfile> AllUserProfileCache(IQueryable<UserProfile> query)
        {
            return query.AsNoTracking().Include("User");
        }

        public override IQueryable<UserProfile> All(bool canUseCacheIfPossible = true)
        {
            return Cache(AllUserProfileCache, canUseCacheIfPossible);
        }


        //[Cacheable(EnableUseCacheServer = true, ExpireCacheSecondTime = 60, EnableAutomaticallyAndPeriodicallyRefreshCache = true, EnableToFetchOnlyChangedDataFromDB = true,
        //    NameOfNavigationPropsForFetchingOnlyChangedDataFromDB = "User")]
        //public static IQueryable<UserProfileDTO> AllUserProfileDTOCache(IQueryable<UserProfile> query)
        //{
        //    var userProfiles = query.AsNoTracking().Include("User");

        //    return userProfiles.Select(profile => new UserProfileDTO()
        //    {
        //        Id = profile.Id,
        //        UserName = profile.UserName,
        //        Password = profile.Password,
        //    });
        //}

        //public IQueryable<UserProfileDTO> GetAllUserProfileDTO(bool canUseCacheIfPossible = true)
        //{
        //    return Cache(AllUserProfileDTOCache, canUseCacheIfPossible);
        //}

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
