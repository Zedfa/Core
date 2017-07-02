using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IServiceBase<RouteMapConfig>), DomainName = "Core")]
    public class RouteMapConfigService : ServiceBase<RouteMapConfig>,IServiceBase<RouteMapConfig>
    {
       
        public RouteMapConfigService():base()
        {
            _repositoryBase = new RouteMapConfigRepository();
        }
     
    }
}
