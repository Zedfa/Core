﻿using Core.Cmn.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Core.Cmn.DependencyInjection
{
    public interface IDependencyInjectionManager
    {
        void RegisterType(Type InterfaceType, Type InjectableType);
        object GetDependencyResolverForMvc();
        object GetDependencyResolverForWebApi();
        T Resolve<T>();
        object DiContainer { get; }
    }
}
