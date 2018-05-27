using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using System.Linq;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IServiceBase<RouteMapConfig>), DomainName = "Core")]
    public class RouteMapConfigService : ServiceBase<RouteMapConfig>
    {
        public RouteMapConfigService() : base()
        {
            _repositoryBase = new RouteMapConfigRepository();
        }

        public override IQueryable<RouteMapConfig> All(bool canUseCache = true)
        {
            return _repositoryBase.All(canUseCache);
        }
    }
}