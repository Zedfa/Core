using System.Linq;
using System.Linq.Expressions;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Cmn.Extensions;
using System;
using System.Collections.Generic;
using Core.Repository.Interface;
using Core.Cmn.Attributes;
using Core.Cmn.Cache;
using System.Threading;
using System.Globalization;


namespace Core.Service
{
    public abstract class ServiceBase
    {
        public static AppBase appBase;

        static ServiceBase()
        {
            appBase = new AppBase();
        }

        private static IDependencyInjectionFactory _dependencyInjectionFactory;
        public static IDependencyInjectionFactory DependencyInjectionFactory
        {
            get
            {
                if (_dependencyInjectionFactory == null)
                    _dependencyInjectionFactory = new DependencyInjectionFactory();
                return _dependencyInjectionFactory;
            }
        }

    }

    [Injectable(InterfaceType = typeof(IServiceBase<>), DomainName = "Core")]
    public class ServiceBase<T> : ServiceBase, IServiceBase<T>, IServiceCache, IDisposable where T : EntityBase<T>, new()
    {
        public IDbContextBase ContextBase { get; private set; }
        protected IRepositoryBase<T> _repositoryBase;
       
        public CultureInfo CurrentCulture { get; set; }


        public virtual IQueryable<T> All(bool canUseCache = true)
        {
            return _repositoryBase.All(canUseCache);
        }

        public ServiceBase(IDbContextBase coreContextBase)
        {
            ContextBase = coreContextBase;
            _repositoryBase = GetRepository(ContextBase);
            CurrentCulture = Thread.CurrentThread.CurrentUICulture;


        }
        public ServiceBase()
        {
            ContextBase = DependencyInjectionFactory.CreateContextInstance();
            _repositoryBase = GetRepository();
            CurrentCulture = Thread.CurrentThread.CurrentUICulture;


        }

       
        public IRepositoryBase<T> RepositoryBase
        {
            get { return _repositoryBase; }
        }


        public virtual IRepositoryBase<T> GetRepository(IDbContextBase dbContext = null)
        {
            if (dbContext == null)
            {
                return new RepositoryBase<T>();

            }
            else
            {
                    return new RepositoryBase<T>(ContextBase);
                
            }
        }

        //public virtual IQueryable<T> All(bool canUseCacheIfPossible = true)
        //{
        //    if (CacheKey != null)
        //        return _repositoryBase.All().Cache(ExpireCacheSecondTime, canUseCacheIfPossible);
        //    else return _repositoryBase.All();
        //    //#if DEBUG
        //    //            string useUnlimitedCacheTime_JustForDebugTime = System.Configuration.ConfigurationManager.AppSettings["UseUnlimitedCacheTime_JustForDebugTime"];
        //    //            if (useUnlimitedCacheTime_JustForDebugTime == null || bool.Parse(useUnlimitedCacheTime_JustForDebugTime))
        //    //                ExpireCacheSecondTime = 1000000;
        //    //#endif
        //    //            if (canUseCacheIfPossible && CacheKey != null)
        //    //            {

        //    //                List<T> result = new List<T>();
        //    //                var fakeResult = string.Empty;
        //    //                if (CacheService.TryGetCache<List<T>>(CacheKey, out result))
        //    //                {
        //    //                    if (!CacheService.TryGetCache<string>(CacheKey + 1, out fakeResult))
        //    //                    {
        //    //                        CacheService.SetCache<string>(CacheKey + 1, "Fake value", ExpireCacheSecondTime);
        //    //#if DEBUG
        //    //                        var countOfFetchRecord_JustForDebugTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountOfFetchRecord_JustForDebugTime"]);
        //    //                        result = _repositoryBase.All().Take(countOfFetchRecord_JustForDebugTime).ToList();
        //    //#else
        //    //                        result = _repositoryBase.All().ToList();
        //    //#endif
        //    //                        CacheService.SetCache<List<T>>(CacheKey, result,  ExpireCacheSecondTime * (double)100000);
        //    //                    }
        //    //                }
        //    //                else
        //    //                {
        //    //                    result = _repositoryBase.All().ToList();
        //    //                    CacheService.SetCache<List<T>>(CacheKey, result, ExpireCacheSecondTime * (double)100000);
        //    //                    CacheService.SetCache<string>(CacheKey + 1, "Fake value", ExpireCacheSecondTime);
        //    //                }

        //    //                CacheService.TryGetCache<List<T>>(CacheKey, out result);
        //    //                return result.AsQueryable();
        //    //            }
        //    //            else
        //    //            {
        //    //                return _repositoryBase.All();

        //    //            }
        //}


        public virtual T Create(T entity, bool allowSaveChange = true)
        {
            return _repositoryBase.Create(entity, allowSaveChange);
        }

        public virtual List<T> Create(List<T> objectList, bool allowSaveChange = true)
        {
            return _repositoryBase.Create(objectList, allowSaveChange);
        }

        public virtual int Delete(T entity, bool allowSaveChange = true)
        {

            return _repositoryBase.Delete(entity, allowSaveChange);

        }

        public virtual int Delete(IQueryable<T> itemsForDeletion, bool allowSaveChange = true)
        {
            return _repositoryBase.Delete(itemsForDeletion, allowSaveChange);
        }

        public virtual int Delete(Expression<Func<T, bool>> predicate, bool allowSaveChange = true)
        {
            return _repositoryBase.Delete(predicate, allowSaveChange);
        }

        public virtual int Delete(List<T> i, bool allowSaveChange = true)
        {
            return _repositoryBase.Delete(i, allowSaveChange);
        }
        public virtual int Update(T entity, bool allowSaveChange = true)
        {
            return _repositoryBase.Update(entity, allowSaveChange);
        }

        public virtual int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updatepredicate, bool allowSaveChange = true)
        {

            return _repositoryBase.Update(predicate, updatepredicate, allowSaveChange);

        }

        public virtual int Count { get { return _repositoryBase.Count; } }

        public virtual void Attach(dynamic entity)
        {
            _repositoryBase.Update(entity);

        }

        public virtual T Find(params object[] keys)
        {
            return _repositoryBase.Find(keys);
        }

        public virtual T Find(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true)
        {
            return _repositoryBase.Find(predicate);
        }



        public AppBase AppBase { get { return appBase; } }

        public virtual void Dispose()
        {
            if (_repositoryBase != null)
                _repositoryBase.Dispose();
        }


        //Remark:
        //public T CreateAndAttach(T TObject, Type attchObject, List<ob> attachedIdList)
        //{
        //    return _repositoryBase.CreateAndAttach(TObject, attchObject, attachedIdList);

        //}



        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        public int Update(IQueryable<T> source, Expression<Func<T, T>> predicate)
        {
            return _repositoryBase.Update(source, predicate);
        }


        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true)
        {
            return _repositoryBase.Filter(predicate);
        }

        public virtual IQueryable<T> Filter(string expression, bool allowFilterDeleted = true, params object[] value)
        {
            return _repositoryBase.Filter(expression, allowFilterDeleted, value);
        }

        //public IQueryable<T> Filter(ExpressionInfo expressionInfo, out int total, bool allowFilterDeleted = true)
        //{
        //    total = 0;
        //    return null;
        //    //return _repositoryBase.Filter(expressionInfo,total,);
        //}


        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            return _repositoryBase.Filter(filter, out total, index, size);
        }

        public virtual bool Contains(Expression<Func<T, bool>> predicate)
        {
            return _repositoryBase.Contains(predicate);
        }

        public ReturnT Cache<ReturnT>(Func<ReturnT> func, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);

            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<ReturnT>(cacheInfo);
                if (canUseCacheIfPossible)
                {
                    ReturnT result = CacheBase.Cache<ReturnT>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        private void GetOrCreateCacheInfo(Delegate func, out string cacheKey, out CacheInfo cacheInfo)
        {
            cacheKey = func.Method.GetHashCode().ToString();
            cacheInfo = null;
            if (!CacheConfig.CacheInfoDic.TryGetValue(cacheKey, out cacheInfo))
            {
                var att = func.Method.GetAttributeValue<CacheableAttribute, CacheableAttribute>(item => item);
                CacheConfig.CacheInfoDic[cacheKey] = cacheInfo = CacheConfig.CreateCacheInfo(att, func.Method, func, cacheKey);
                cacheInfo.Service = this;
            }
        }

        public R Cache<P1, R>(Func<P1, R> func, P1 param1, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);
            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, R>(cacheInfo, func, param1);
                if (canUseCacheIfPossible)
                {
                    cacheKey = queryableCacheExecution.GenerateCacheKey();
                    R result = CacheBase.Cache<R>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        public R Cache<P1, P2, R>(Func<P1, P2, R> func, P1 param1, P2 param2, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);

            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, P2, R>(cacheInfo, func, param1, param2);
                if (canUseCacheIfPossible)
                {
                    cacheKey = queryableCacheExecution.GenerateCacheKey();
                    R result = CacheBase.Cache<R>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        public R Cache<P1, P2, P3, R>(Func<P1, P2, P3, R> func, P1 param1, P2 param2, P3 param3, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);

            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, P2, P3, R>(cacheInfo, func, param1, param2, param3);
                if (canUseCacheIfPossible)
                {
                    cacheKey = queryableCacheExecution.GenerateCacheKey();
                    R result = CacheBase.Cache<R>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        public R Cache<P1, P2, P3, P4, R>(Func<P1, P2, P3, P4, R> func, P1 param1, P2 param2, P3 param3, P4 param4, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);


            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, P2, P3, P4, R>(cacheInfo, func, param1, param2, param3, param4);
                cacheKey = queryableCacheExecution.GenerateCacheKey();
                if (canUseCacheIfPossible)
                {
                    R result = CacheBase.Cache<R>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        public R Cache<P1, P2, P3, P4, P5, R>(Func<P1, P2, P3, P4, P5, R> func, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, bool canUseCacheIfPossible = true)
        {
            string cacheKey;
            CacheInfo cacheInfo;
            GetOrCreateCacheInfo(func, out cacheKey, out cacheInfo);


            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, P2, P3, P4, P5, R>(cacheInfo, func, param1, param2, param3, param4, param5);
                cacheKey = queryableCacheExecution.GenerateCacheKey();
                if (canUseCacheIfPossible)
                {
                    R result = CacheBase.Cache<R>(queryableCacheExecution, cacheInfo, cacheInfo.RefreshCacheTimeSeconds, cacheKey, canUseCacheIfPossible);
                    return result;
                }
                else
                {
                    return queryableCacheExecution.GetFreshData();
                }
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }
        public R RefreshCache<R>(Func<R> func)
        {
            var cacheKey = func.Method.GetHashCode().ToString();
            var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];

            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<R>(cacheInfo);

                R result = CacheBase.RefreshCache<R>(queryableCacheExecution, cacheInfo);
                return result;
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }

        public R RefreshCache<P1, R>(Func<P1, R> func, P1 param1, string key = null, int expireCacheSecondTime = 60)
        {
            var cacheKey = func.Method.GetHashCode().ToString();
            var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
            if (func != null)
            {
                var queryableCacheExecution = new FunctionalCacheDataProvider<P1, R>(cacheInfo, func, param1);
                cacheKey = queryableCacheExecution.GenerateCacheKey();
                R result = CacheBase.RefreshCache<R>(queryableCacheExecution, cacheInfo);
                return result;
            }
            else
            {
                throw new ArgumentNullException("func");
            }

        }


    }
}
