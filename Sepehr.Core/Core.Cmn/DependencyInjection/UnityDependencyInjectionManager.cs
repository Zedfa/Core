
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

            foreach (var item in allInjectableKeyValuePaires)
            {
                var interfaceType = Reflection.ReflectionHelpers.GetType(item.Value.InterfaceType.FullName);
                var injectableItem = Reflection.ReflectionHelpers.GetType(item.Key.FullName);

                this.RegisterType(interfaceType, injectableItem);
            }
        }
        public void RegisterType(Type InterfaceType, Type InjectableType)
        {
            (DiContainer as UnityContainer)
                .RegisterType(InterfaceType, InjectableType);

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
    }
}
