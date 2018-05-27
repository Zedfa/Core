using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep.DTO;
using System.Linq;

namespace Core.Rep
{
    public class RoleRepository : RepositoryBase<Role>
    {



        public RoleRepository(IDbContextBase dbContext)
            : base(dbContext)
        {

        }

        [Cacheable(
    EnableSaveCacheOnHDD = true,
    EnableUseCacheServer = false,
    AutoRefreshInterval = 3600,
    CacheRefreshingKind = Cmn.Cache.CacheRefreshingKind.SqlDependency,
    EnableToFetchOnlyChangedDataFromDB = true,
    EnableCoreSerialization = true
    )]
        public static IQueryable<Role> AllRolesCache(IQueryable<Role> query)
        {
            return query;
        }
        public IQueryable<Role> AllCache(bool canUseCacheIfPossible = true)
        {
            return Cache(AllRolesCache, canUseCacheIfPossible);
        }


        public Role GetRoleByTitle(string roleTitle)
        {

            return All().FirstOrDefault(role => role.Name == roleTitle);

        }
        public IQueryable<RoleDTO> GetAllRoleDTO()
        {
            return All().Select(role => new RoleDTO { Id = role.ID, Name = role.Name });
        }
        public Role FindRoleWithRoleHotelAndRoleDestinationByRoleId(int roleId)
        {
            return All().First(role => role.ID == roleId);
        }
        public IQueryable<Role> GetAllRolesByCompany(int userId)
        {
            var user = ContextBase.Set<User>().Find(userId);
            if (user.CurrentCompanyId != null)
            {
                return base.All().Where(role => role.CurrentCompanyId == user.CurrentCompanyId);
            }
            return base.All();
        }
    }
}
