using Core.Cmn.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class QueryableCache
    {
        public static IQueryable<T> Cache<T>(string cacheKey, int expireCacheSecondTime = 60, bool canUseCacheIfPossible = true)
        {
            var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
            var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
            IQueryable<T> result = queryableCacheExecution.Cache<IQueryable>(cacheInfo, expireCacheSecondTime, cacheKey, canUseCacheIfPossible) as IQueryable<T>;
            return result;
        }

        public static IQueryable<T> RefreshCache<T>(string cacheKey)
        {
            var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
            var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
            IQueryable<T> result = CacheBase.RefreshCache<IQueryable>(queryableCacheExecution, cacheInfo) as IQueryable<T>;
            return result;
        }

    }
}
