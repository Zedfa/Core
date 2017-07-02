using Core.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef
{
    public interface IDependencyInjectionFactory
    {
        T CreateInjectionInstance<T>();
    }
}
