using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IDependencyInjectionFactory
    {
        T CreateInjectionInstance<T>();
        IDbContextBase CreateContextInstance();
        IRequest TryToResolveIRequest();
    }       
}
