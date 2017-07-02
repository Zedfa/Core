using Core.Cmn;
using Core.Ef;
using Core.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class CacheService
    {
        #region Variable

        static ObjectCache _cache = MemoryCache.Default;
        static ILogService _logService = Core.Cmn.AppBase.LogService;

        #endregion

        #region TryGetCache

        public static bool TryGetCache<T>(string key, out T result)
        {
            result = (T)_cache[key];
            if (result != null)
                return true;
            else return false;
        }
        public static bool TryGetCache<T>(string key, out List<T> result)
        {

            result = (List<T>)_cache[key];
            if (result != null)
            {
                try
                {
                    result = result.ToList();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else return false;
        }

        #endregion

        #region SetCache

        public static void SetCache<T>(string key, T result, double secondTimePolicy)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(secondTimePolicy);
            _cache.Set(key, result, policy);
        }
        public static void SetCache<T>(string key, List<T> result, double secondTimePolicy)
        {
            _logService.Handle(
                        new Exception("SetCache Trace"),
                        "",//userId
                        "key: " + key + Environment.NewLine +
                        "result count:" + result.Count + Environment.NewLine +
                        "result type: " + typeof(T).FullName + Environment.NewLine +
                        "secondTimePolicy: " + secondTimePolicy.ToString() + Environment.NewLine + 
                        "DateTime.Now: " + DateTime.Now.ToLongDateString()//customMessage
                        );
              

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(secondTimePolicy);
            _cache.Set(key, result, policy);
        }

        #endregion

        #region Remove Cache

        public static void RemoveCache(string key)
        {
            _cache.Remove(key);
        }

        #endregion

        #region Helper Method


        #endregion
    }
}
