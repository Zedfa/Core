using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    public interface ICacheDataProvider<T> : ICacheDataProvider
    {
        T GetFreshData();
    }
    public interface ICacheDataProvider
    {
        CacheInfo CacheInfo { get; set; }
        string GenerateCacheKey();
        void SetFunc(object func);
        object GetDataFromCacheServer();
    }
}
