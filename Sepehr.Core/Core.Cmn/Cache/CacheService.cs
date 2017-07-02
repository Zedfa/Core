using Core.Cmn;
using Core.Cmn.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class CacheService
    {
        #region Variable

        static ObjectCache _cache = MemoryCache.Default;
        public static ObjectCache ObjectCache
        {
            get
            {
                return _cache;
            }
        }
        static ILogService _logService = AppBase.LogService;

        #endregion

        #region TryGetCache

        public static bool TryGetCache<T>(string key, out T result)
        {
            var res = _cache[key];
            if (res != null)
            {
                result = ((CacheData<T>)res).Data;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }
        #endregion

        #region SetCache

        public static void SetCache<T>(string key, T result, double secondTimePolicy)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(secondTimePolicy);
            var cacheData = new CacheData<T>() { Data = result };
            _cache.Set(key, cacheData, policy);

        }


        #endregion

        #region Remove Cache

        public static void RemoveCache(string key)
        {
            _cache.Remove(key);
        }

        #endregion




    }
}
