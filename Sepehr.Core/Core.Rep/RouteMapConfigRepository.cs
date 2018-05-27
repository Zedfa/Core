using Core.Cmn;
using Core.Entity;
using System.Linq;

namespace Core.Rep
{
    public class RouteMapConfigRepository : RepositoryBase<RouteMapConfig>
    {
        public RouteMapConfigRepository(IDbContextBase dbContext)
            : base(dbContext)
        {
        }

        public RouteMapConfigRepository() : base()
        {
        }
    }
}