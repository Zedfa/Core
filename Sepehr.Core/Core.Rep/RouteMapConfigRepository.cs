using Core.Cmn.Attributes;
using Core.Entity;
using System.Linq;
using Core.Cmn.Extensions;
using Core.Cmn;

namespace Core.Rep
{
    public class RouteMapConfigRepository:RepositoryBase<RouteMapConfig>
    {
        public RouteMapConfigRepository(IDbContextBase dbContext)
            : base(dbContext)
        {
           
        }
        public RouteMapConfigRepository():base()
        {

        }
        
        [Cacheable(
            EnableSaveCacheOnHDD = true,
            EnableUseCacheServer = false,
            AutoRefreshInterval  = 300, 
            EnableCoreSerialization = true
            )]
        public static IQueryable<RouteMapConfig> AllRouteMapConfigs(IQueryable<RouteMapConfig> query)
        {
            return query.AsNoTracking();
        }
        public override IQueryable<RouteMapConfig> All(bool canUseCacheIfPossible = true)
        {
            return Cache<RouteMapConfig>(AllRouteMapConfigs, canUseCacheIfPossible);
        }

    }
}
