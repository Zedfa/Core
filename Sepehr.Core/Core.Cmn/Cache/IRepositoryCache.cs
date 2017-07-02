using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Cmn
{
    public interface IRepositoryCache
    {
        //object BuildCache(IDbContextBase db, string cacheKey);
        //object BuildCache<TParam1>(IDbContextBase db, string cacheKey, TParam1 param1);
        //object BuildCache<TParam1, TParam2>(IDbContextBase db, string cacheKey, TParam1 param1, TParam2 param2);
        IQueryable GetQueryableForCahce();
        Type GetDomainModelType();
        string TableName { get; }
        string Schema { get; }
        string KeyName { get; }
        byte[] GetMaxTimeStamp();
    }
}
