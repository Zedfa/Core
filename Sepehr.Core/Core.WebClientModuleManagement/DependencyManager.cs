using Core.Mvc.UnityRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Core.WebClientModuleManagement
{
    public class DependencyManager
    {
        public static void Register(Assembly assembly)
        {
            //var nameSpace = assembly.GetName().Name;
            var unityClasses = assembly.GetTypes().Where(type => typeof(UnityConfigBase).IsAssignableFrom(type) && !type.IsAbstract);
            foreach (var unityClass in unityClasses)
            {
                //for call constructor and set dependency resolver
                Activator.CreateInstance(unityClass);

               // Activator.CreateInstance(unityClass,StartupProjectConfig.DllName);
            }
        }
    }
}
