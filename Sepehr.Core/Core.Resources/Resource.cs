using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;

namespace Core.Resources
{

    

    public interface IResourceProvider : IDisposable
    {
        String GetResource(String resourceKey, Language lang);

        String GetResource(String resourceKey);
    }

    public static class GetResource
    {
        public  static string GetResourceString(string resourceName, Language lang)
        {

            var resProviderInstance = DiManager.Current.GetService<IResourceProvider>();

            return resProviderInstance.GetResource(resourceName, lang);

        }

        public static string GetResourceString(string resourceName)
        {

            var resProviderInstance = DiManager.Current.GetService<IResourceProvider>();
            return resProviderInstance.GetResource(resourceName);

        }

    }
   

}
