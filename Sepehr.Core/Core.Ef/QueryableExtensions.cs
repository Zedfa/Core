using Core.Cmn;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        //public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> queryable, String path)
        //    where TEntity : EntityBase<TEntity>
        //{
        //    if (queryable == null)
        //        throw new ArgumentNullException("queryable");

        //    if (path == null)
        //        throw new ArgumentNullException("path");

        //    queryable = ((System.Data.Entity.Infrastructure.DbQuery<TEntity>)queryable).Include(path);

        //    // NOTE: The approach which is being used here is not compatible with our dependency injection policies, and you should consider changing it when you change repository and set implementations in Ioc settings.

        //    return queryable;
        //}
    }
}
