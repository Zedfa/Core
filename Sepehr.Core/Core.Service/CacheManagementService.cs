using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class CacheManagementService : IServiceCache
    {
        public CacheManagementService(IDbContextBase dbcontext)
        {

        }
        [Cacheable(EnableSaveCacheOnHDD = true, AutoRefreshInterval = 120, CacheRefreshingKind = CacheRefreshingKind.Slide)]
        public static bool WriteToHardCaches()
        {
            var cacheList = CacheService.ObjectCache.ToList();
            CacheConfig.CacheInfoDic.Values.ToList().Where(ci => ci.EnableSaveCacheOnHDD && (ci.CacheRefreshingKind != CacheRefreshingKind.SqlDependency || ci.EnableToFetchOnlyChangedDataFromDB)).ToList().ForEach(ci =>
            {
                cacheList.Where(cache => cache.Key.StartsWith(ci.BasicKey.ToString()) && !cache.Key.Contains("_Fake")).ToList().ForEach(cache =>
                {
                    lock (ci)
                    {
                        try
                        {
                            var data = cache.Value.GetType().GetProperty("Data").GetValue(cache.Value);
                            var binary = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(data);
                            if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache"))
                                System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache");
                            System.IO.File.WriteAllBytes(string.Format(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache/{0}.Cache", ci.Name), binary);
                        }
                        catch (Exception ex)
                        {
                            //  Core.Cmn.AppBase.LogService.Handle(ex, "", string.Format("error in write cache in HDD = {0}", ci.Name));
                            Core.Cmn.AppBase.LogService.Handle(ex, string.Format("error in write cache in HDD = {0}", ci.Name));
                        }
                    }
                });
            });

            return true;
        }
    }
}
