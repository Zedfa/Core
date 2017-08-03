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
        [Cacheable(EnableSaveCacheOnHDD = true, ExpireCacheSecondTime = 60, EnableAutomaticallyAndPeriodicallyRefreshCache = true, EnableUseCacheServer = true)]
        public static bool WriteToHardCaches()
        {
            var cacheList = CacheService.ObjectCache.ToList();
            CacheConfig.CacheInfoDic.Values.ToList().Where(ci => ci.EnableSaveCacheOnHDD).ToList().ForEach(ci =>
            {
                cacheList.Where(cache => cache.Key.StartsWith(ci.BasicKey.ToString()) && !cache.Key.Contains("_Fake")).ToList().ForEach(cache =>
                {
                    try
                    {
                        var binary = Core.Cmn.Extensions.SerializationExtensions.SerializetoBinary(cache.Value.GetType().GetProperty("Data").GetValue(cache.Value));
                        if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache"))
                            System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache");
                        System.IO.File.WriteAllBytes(string.Format(System.AppDomain.CurrentDomain.BaseDirectory + @"/Cache/{0}.Cache", ci.Name), binary);
                    }
                    catch (Exception ex)
                    {
                        //  Core.Cmn.AppBase.LogService.Handle(ex, "", string.Format("error in write cache in HDD = {0}", ci.Name));
                        Core.Cmn.AppBase.LogService.Handle(ex, string.Format("error in write cache in HDD = {0}", ci.Name));
                    }
                });
            });

            return true;
        }
    }
}
