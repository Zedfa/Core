using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Extensions;
using Core.Entity;
using Core.Rep.DTO.UserRoleDTO;
using System.Linq;

namespace Core.Rep
{
    public class UserRoleRepository : RepositoryBase<UserRole>
    {
        #region Variable

        private IDbContextBase _dc;

        #endregion Variable

        public UserRoleRepository(IDbContextBase dc)
            : base(dc)
        {
            _dc = dc;
        }

        public override IQueryable<UserRole> All(bool canUseCacheIfPossible = true)
        {
            return Cache<UserRole>(AllUserRoleCache, canUseCacheIfPossible);
        }

        [Cacheable(
            EnableSaveCacheOnHDD = true,
            EnableUseCacheServer = false,
            EnableCoreSerialization = true,
            AutoRefreshInterval = 3600,
            CacheRefreshingKind = Cmn.Cache.CacheRefreshingKind.SqlDependency,
            EnableToFetchOnlyChangedDataFromDB = true
            )]
        public static IQueryable<UserRole> AllUserRoleCache(IQueryable<UserRole> query)
        {
            return query.AsNoTracking()
                .Include(userRole => userRole.Role)
                .Include(userRole => userRole.User);
        }

        public IQueryable<UserRole> GetRolesByUserId(int userId)
        {
            return All().Where(userRole => userRole.UserId == userId);
        }

        public IQueryable<UserRoleDTO> GetAllUserRolesDto()
        {
            return All().Select(userRole => new UserRoleDTO
            {
                RoleId = userRole.RoleID,
                RoleName = userRole.Role.Name,
                UserId = userRole.UserId
            });
        }

        public int Update(int newRoleId, int oldRoleId, int userId, bool allowSaveChange = true)
        {
            var userRole = ContextBase.Set<UserRole>();
            var oldUserRole = userRole.Where(a => a.RoleID == oldRoleId && a.UserId == userId).SingleOrDefault();
            userRole.Remove(oldUserRole);

            var newUserRole = new UserRole()
            {
                RoleID = newRoleId,
                UserId = userId,
            };
            userRole.Add(newUserRole);

            if (allowSaveChange)
                SaveChanges();
            return 0;
        }

        //public List<User> GetKishUsers()
        //{
        //    return _dc.Set<UserRole>().Where(item => item.RoleID == Constants.KishConstants.KishRole)
        //        .Select(item => item.User).ToList();
        //}
        public bool HasDuplicateRoleAssigne(int selectedRoleId, int userId)
        {
            var foundRoles = Filter(a => a.RoleID == selectedRoleId && a.UserId == userId).ToList();
            if (foundRoles.Any())
                return true;
            return false;
        }
    }
}