
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Cmn.Extensions
{
    public static class QueryableExt
    {

        private static IQueryableExtensions _queryableExtensions;
        private static IQueryableExtensions QueryExtensions
        {
            get
            {
                if (_queryableExtensions == null)
                {
                    _queryableExtensions = AppBase.DependencyInjectionManager.Resolve<IQueryableExtensions>();
                }
                return _queryableExtensions;
            }
        }

        private static IBatchExtensions _batchExtensions;
        private static IBatchExtensions BatchExtensions
        {
            get
            {
                if (_batchExtensions == null)
                {
                    _batchExtensions = AppBase.DependencyInjectionManager.Resolve<IBatchExtensions>();
                }
                return _batchExtensions;
            }
        }
      
       
        public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> source) where T : class
        {
            return QueryExtensions.AsNoTracking<T>(source);
        }
        public static IQueryable AsNoTracking(this IQueryable source)
        {
            return source.AsNoTracking();
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
        {
            return QueryExtensions.Include<T>(source, path);
        }
        public static IQueryable Include(this IQueryable source, string path)
        {
            return QueryExtensions.Include(source, path);
        }
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path)
        {
            return QueryExtensions.Include<T, TProperty>(source, path);
        }

        public static int Delete<T>(this IQueryable<T> source) where T : class
        {
            return BatchExtensions.Delete<T>(source);
        }
        public static int Update<T>(this IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class
        {
            return BatchExtensions.Update<T>(source, updateExpression);
        }
       
    }
}
