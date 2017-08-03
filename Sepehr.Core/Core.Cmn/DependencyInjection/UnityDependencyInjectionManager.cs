
using Core.Cmn.Attributes;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn.Extensions;

namespace Core.Cmn.DependencyInjection
{
    public class UnityDependencyInjectionManager : IDependencyInjectionManager
    {
        public UnityDependencyInjectionManager(List<Type> allTypes)
        {
            var allInjectableKeyValuePaires = allTypes.Select(type =>
            {
                var result = new KeyValuePair<Type, InjectableAttribute>(type, type.GetAttributeValue<InjectableAttribute, InjectableAttribute>(item => item));
                return result;
            }).Where(item => item.Value != null).ToList();

            var allGroupedInjectableTypes = allInjectableKeyValuePaires.GroupBy(kv => kv.Value.InterfaceType);

            foreach (var groupedItem in allGroupedInjectableTypes)
            {
                var item = groupedItem.OrderByDescending(g => g.Value.Version).First();
                var interfaceType = item.Value.InterfaceType;
                var injectableItem = item.Key;
                var lifetime = GetLifetimeStateInstance(item.Value.LifeTime);
                this.RegisterType(interfaceType, injectableItem, lifetime);
            }
        }
        private LifetimeManager GetLifetimeStateInstance(LifetimeManagement lifetime)
        {

            switch (lifetime)
            {
                case LifetimeManagement.TransientLifetime:
                    return new TransientLifetimeManager();

                case LifetimeManagement.ContainerControlledLifetime:
                    return new ContainerControlledLifetimeManager();

                case LifetimeManagement.PerThreadLifetime:
                    return new PerThreadLifetimeManager();

                case LifetimeManagement.PerResolveLifetime:
                    return new PerResolveLifetimeManager();

                case LifetimeManagement.PerRequestLifetime:
                    return new PerRequestLifetimeManager();

                default:
                    return new TransientLifetimeManager();

            }

        }
        private void RegisterType(Type InterfaceType, Type InjectableType, LifetimeManager lifetime)
        {
            (DiContainer as UnityContainer)
                .RegisterType(InterfaceType, InjectableType, lifetime);

        }

        private UnityContainer _diContainer;
        public object DiContainer
        {
            get
            {

                if (_diContainer == null)
                {
                    _diContainer = new UnityContainer();
                }
                return _diContainer;

            }
        }

        private object _dependencyResolverForWebApi;
        public object GetDependencyResolverForWebApi()
        {
            if (_dependencyResolverForWebApi == null)
            {
                _dependencyResolverForWebApi = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(DiContainer as UnityContainer);
            }
            return _dependencyResolverForWebApi;
        }

        private object _dependencyResolverForMvc;
        public object GetDependencyResolverForMvc()
        {
            if (_dependencyResolverForMvc == null)
            {
                _dependencyResolverForMvc = new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(DiContainer as UnityContainer);
            }
            return _dependencyResolverForMvc;
        }

        public T Resolve<T>()
        {
            return (DiContainer as UnityContainer).Resolve<T>();
        }

        public object Resolve(Type type, params ParameterOverride[] constructorsParams)
        {
            var parameters = constructorsParams.Select(param => new Microsoft.Practices.Unity.ParameterOverride(param.ParamName, param.ParamValue)).ToArray();
            return UnityContainerExtensions.Resolve((DiContainer as UnityContainer), type, parameters);
        }
    }
}
