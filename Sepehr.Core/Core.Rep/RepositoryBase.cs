using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Core.Entity;
using Core.Repository.Interface;
using EntityState = Core.Cmn.EntityState;
using Core.Cmn.Extensions;
using Core.Cmn;
using Core.Cmn.EntityBase;
using Core.Cmn.Cache;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Text;
using Core.Cmn.Attributes;
using Core.Ef.Extensions;
using System.Linq.Dynamic;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using EntityFramework.MappingAPI.Extensions;

namespace Core.Rep
{//, IDTOQueryableBuilder<TObject>

    public class RepositoryBase
    {
        static RepositoryBase()
        {
            DefualtMaxRetriesCountForDeadlockRetry = 3;
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
        public static int DefualtMaxRetriesCountForDeadlockRetry { get; set; }
    }

    [Injectable(InterfaceType = typeof(IRepositoryBase<>), DomainName = "Core")]
    public class RepositoryBase<TObject> : RepositoryBase, IRepositoryBase<TObject>, IRepositoryCache where TObject : EntityBase<TObject>, new()

    {
        public CultureInfo CurrentCulture { get; set; }

        protected IDbContextBase ContextBase;

        protected IUserLog UserLog;
        private IDbSetBase<TObject> dbSet;

        //public virtual IDbContextBase ContextGenerator()
        //{
        //    return null;
        //}

        Stopwatch Stopwatch { get; set; }

        public RepositoryBase(IDbContextBase dbContextBase, IUserLog userLog)
        {
            Stopwatch = new Stopwatch();
            ContextBase = dbContextBase;
            UserLog = userLog;
        }

        public RepositoryBase(IDbContextBase dbContextBase)
        {
            Stopwatch = new Stopwatch();
            ContextBase = dbContextBase;
        }

        public RepositoryBase()
        {
            Stopwatch = new Stopwatch();
            ContextBase = DependencyInjectionFactory.CreateContextInstance();
        }

        //protected Func<IQueryable<TObject>, IQueryable<IDTO>> DtoQueryableDelegate;

        protected virtual void SetQueryable(IQueryable<TObject> queryable) { }

        protected IQueryable<IDto> QueryableDtos { get; set; }

        public IQueryable<IDto> GetDtoQueryable(IQueryable<TObject> queryable)
        {
            this.SetQueryable(queryable);

            return QueryableDtos;
        }


        public virtual IQueryable<TObject> All(bool canUseCacheIfPossible = true)
        {
            return DbSet.AsQueryable();
        }

        private static string _tableName;
        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = ContextBase.GetTableName<TObject>();
                    //ObjectContext objectContext = ((IObjectContextAdapter)ContextBase).ObjectContext;
                    //Type entityType = typeof(TObject);

                    //if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    //    entityType = entityType.BaseType;

                    //string entityTypeName = entityType.Name;

                    //EntityContainer container =
                    //    objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
                    //var entitySet = (from meta in container.BaseEntitySets
                    //                 where meta.ElementType.Name == entityTypeName
                    //                 select meta).First();
                    //if (entitySet.MetadataProperties.Contains("Configuration") && entitySet.MetadataProperties["Configuration"].Value != null)
                    //{
                    //    var tableName = ((entitySet.MetadataProperties["Configuration"].Value)).ToDictionary()["TableName"];
                    //    if (tableName == null)
                    //        _tableName = entitySet.Name;
                    //    else
                    //        _tableName = tableName.ToString();
                    //}
                    //else
                    //{
                    //    _tableName = entityType.Name;

                }
                return _tableName;
            }
        }

        private string _schema;
        public string Schema
        {
            get
            {
                if (string.IsNullOrEmpty(_schema))
                {
                    _schema = ContextBase.GetSchemaName<TObject>();
                    //ObjectContext objectContext = ((IObjectContextAdapter)ContextBase).ObjectContext;
                    //Type entityType = typeof(TObject);

                    //if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    //    entityType = entityType.BaseType;

                    //string entityTypeName = entityType.Name;

                    //EntityContainer container =
                    //    objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
                    //var entitySetName = (from meta in container.BaseEntitySets
                    //                     where meta.ElementType.Name == entityTypeName
                    //                     select meta).First();
                    //if (entitySetName.MetadataProperties.Contains("Configuration") && entitySetName.MetadataProperties["Configuration"].Value != null)
                    //{
                    //    var schema = ((entitySetName.MetadataProperties["Configuration"].Value)).ToDictionary()["SchemaName"];
                    //    if (schema == null)
                    //        _schema = "dbo";
                    //    else
                    //        _schema = schema.ToString();
                    //}
                    //else
                    //{
                    //    _schema = "dbo";
                    //}
                }
                return _schema;
            }
        }

        public static string _keyName;
        public string KeyName
        {
            get
            {
                if (string.IsNullOrEmpty(_keyName))
                {
                    ObjectContext objectContext = ((IObjectContextAdapter)ContextBase).ObjectContext;
                    var set = objectContext.CreateObjectSet<TObject>();
                    _keyName = (ContextBase as DbContext).Db(typeof(TObject)).Pks.Select(x => x.ColumnName).First();
                }
                return _keyName;
            }
        }




        public IDbSetBase<TObject> DbSet
        {
            get
            {
                dbSet = ContextBase.Set<TObject>();

                return dbSet;
            }
        }

        public void Dispose()
        {
            if (ContextBase != null)
                ContextBase.Dispose();
        }




        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate, bool allowFilterDeleted = true)
        {
            return DbSet.Where(predicate);
        }

        public virtual IQueryable<TObject> Filter(string expression, bool allowFilterDeleted = true, params object[] value)
        {
            return DbSet.Where(expression, value).AsQueryable();

        }


        public IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter,
         out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() :
                DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) :
                _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public IEnumerable<TObject> Filter(ExpressionInfo expressionInfo, out int total, bool allowFilterDeleted = true)
        {
            int skipCount = expressionInfo.CurrentPage * expressionInfo.PageSize;
            var _resetSet = expressionInfo.Expression != null ? DbSet.Where(((KeyValuePair<string, string>)expressionInfo.Expression).Key, expressionInfo.Expression.Value).AsQueryable() :
                DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(expressionInfo.PageSize) :
                _resetSet.Skip(skipCount).Take(expressionInfo.PageSize);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();

        }


        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);

        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate, bool allowFilterDeleted = true)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject t, bool allowSaveChange = true)
        {
            ContextBase.SetContextState(t, EntityState.Added);
            if (allowSaveChange)
                SaveChanges();
            return t;
        }

        public virtual List<TObject> Create(List<TObject> objectList, bool allowSaveChange = true)
        {

            var result = DbSet.AddRange(objectList);
            if (allowSaveChange)
                SaveChanges();

            return result.ToList();
        }

        public virtual int SaveChanges()
        {
            //try
            //{


            return ContextBase.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    var objContext = ((IObjectContextAdapter)ContextBase).ObjectContext;
            //    // Get failed entry
            //    var entry = ex.Entries.Select(e => e.Entity);
            //    // Now call refresh on ObjectContext
            //    objContext.Refresh(RefreshMode.ClientWins, entry);
            //    return entry.Count();

            //}


        }


        public virtual int Delete(TObject t, bool allowSaveChange = true)
        {
            ContextBase.SetContextState(t, EntityState.Deleted);
            if (allowSaveChange)
                return SaveChanges();
            return 0;

        }



        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Update(TObject t, bool allowSaveChange = true)
        {

            //example of using an IQueryable as the filter for the update
            //var users = context.Users.Where(u => u.FirstName == "firstname");
            // context.Users.Update(users, u => new User { FirstName = "newfirstname" });
            //CreateExpression(typeof(bool), _condition);
            // context.YourEntities.Local.Any(e => e.Id == id);

            //Func<TObject, bool> func = x => t[KeyName].ToString() == t[KeyName].ToString();
            //if (ContextBase.Set<TObject>().Local.Any(func))
            //{
            //    var foundObject = Find(t[KeyName]);
            //    var attachedEntry = ContextBase.Entry(foundObject);
            //    attachedEntry.CurrentValues.SetValues(t);
            //}

            //else
            ContextBase.SetContextState(t, EntityState.Modified);

            if (allowSaveChange)
                return SaveChanges();
            return 0;
        }



        //private LambdaExpression CreateExpression(Type objectType, string expression)
        //{

        //    LambdaExpression lambdaExpression =
        //        System.Linq.Dynamic.DynamicExpression.ParseLambda(
        //            objectType, typeof(bool), expression);
        //    return lambdaExpression;
        //}

        public virtual int Update(Expression<Func<TObject, bool>> predicate, Expression<Func<TObject, TObject>> updatepredicate, bool allowSaveChange = true)
        {

            var res = DbSet.Where(predicate).Update(updatepredicate);
            if (allowSaveChange)
                return SaveChanges();
            return res;
        }


        public virtual int Delete(Expression<Func<TObject, bool>> predicate, bool allowSaveChange = true)
        {
            //Remark:Use Extended For Delete
            //var objects = Filter(predicate);
            //foreach (var obj in objects)
            //  DbSet.Remove(obj);
            var res = DbSet.Where(predicate).Delete();
            if (allowSaveChange)
                return SaveChanges();
            return res;
        }

        //Remark:
        ////public virtual TObject CreateAndAttach<T>(TObject tObject, Type attachObject, List<object> attachedList, T t)
        ////{
        ////    //var role = new PegahRole() { Guid = pegahRole };

        ////    //agar in kar ra anjam dahim row jadid dar pegahrole inser nemikonad.
        ////    // ctx.PegahRoles.Attach(role);
        ////    // role.PegahUsers.Add(PegahUser);

        ////    var attacheDbset = ContextBase.Set(attachObject);
        ////    foreach (var item in attachedList)
        ////    {
        ////        var finded = attacheDbset.Find(item);
        ////        attacheDbset.Attach(finded);
        ////        (tObject["Roles"] as ICollection<T>).Add((T)finded);
        ////    }

        ////    var newEntry = DbSet.Add(tObject);
        ////    ContextBase.SaveChanges();
        ////    return newEntry;
        ////}

        ////public virtual TObject UpdateAndAttach<T>(TObject t, List<object> attachedList, List<T> removedList, Expression<Func<TObject, bool>> predicate)
        ////{
        ////    var attacheDbset = ContextBase.Set(typeof(T));

        ////    //  var removedList1= (t["Roles"] as ICollection<T>).ToList();


        ////    foreach (var item in attachedList)
        ////    {
        ////        var finded = attacheDbset.Find(item);
        ////        attacheDbset.Attach(finded);
        ////        (t["Roles"] as ICollection<T>).Add((T)finded);
        ////        // attacheDbset.PegahUsers.Add(PegahUser);
        ////    }

        ////    // (T["Users"] as t).Add(t);
        ////    DbSet.Add(t);

        ////    var users = DbSet.Include("Roles").Single(predicate);
        ////    var newRoles = attacheDbset.Where(r => selectedRoles.Contains(r.Id)).ToList();
        ////    foreach (var item in removedList)
        ////    {

        ////        (t["Roles"] as ICollection<T>).Clear();




        ////    }
        ////    ContextBase.SaveChanges();
        ////    return t;
        ////}
        public int Delete(List<TObject> itemsForDeletion, bool allowSaveChange = true)
        {

            itemsForDeletion.ForEach(item => ContextBase.SetContextState(item, EntityState.Deleted));
            if (allowSaveChange)
            {
                return SaveChanges();
            }
            return 0;
        }

        public int Delete(IQueryable<TObject> itemsForDeletion, bool allowSaveChange = true)
        {
            var res = DbSet.Delete(itemsForDeletion);
            if (allowSaveChange)
                return SaveChanges();
            return res;
        }

        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        public int Update(IQueryable<TObject> source, Expression<Func<TObject, TObject>> predicate)
        {
            var res = DbSet.Update(source, predicate);
            return res;
        }

        public IQueryable<T> Cache<T>(Func<IQueryable<TObject>, IQueryable<T>> func, bool canUseCacheIfPossible = true) where T : IEntity, new()
        {
            if (canUseCacheIfPossible)
            {
                Stopwatch.Restart();
                var cacheKey = func.Method.GetHashCode().ToString();
                var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
                var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
                IQueryable<T> result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheKey, canUseCacheIfPossible).AsQueryable();
                Stopwatch.Stop();
                cacheInfo.UsingTime += TimeSpan.FromTicks(Stopwatch.ElapsedTicks);
                return result;
            }
            else
            {
                return func.Invoke(GetQueryableForCahce() as IQueryable<TObject>);
            }
        }

        public IQueryable<T> Cache<T, P1>(Func<IQueryable<TObject>, P1, IQueryable<T>> func, P1 param1, bool canUseCacheIfPossible = true) where T : IEntity, new()
        {
            if (canUseCacheIfPossible)
            {
                Stopwatch.Restart();
                var cacheKey = func.Method.GetHashCode().ToString();
                var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
                cacheKey += param1;
                var queryableCacheExecution = new QueryableCacheDataProvider<T, P1>(cacheInfo, param1);
                IQueryable<T> result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheKey, canUseCacheIfPossible).AsQueryable();
                Stopwatch.Stop();
                cacheInfo.UsingTime += TimeSpan.FromTicks(Stopwatch.ElapsedTicks);
                return result;
            }
            else
            {
                return func.Invoke(GetQueryableForCahce() as IQueryable<TObject>, param1);
            }
        }
        public IQueryable<T> Cache<T, P1, P2>(Func<IQueryable<TObject>, P1, P2, IQueryable<T>> func, P1 param1, P2 param2, bool canUseCacheIfPossible = true) where T : IEntity, new()
        {
            if (canUseCacheIfPossible)
            {
                Stopwatch.Restart();
                var cacheKey = func.Method.GetHashCode().ToString();
                var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
                cacheKey = cacheKey + param1 + param2;
                var queryableCacheExecution = new QueryableCacheDataProvider<T, P1, P2>(cacheInfo, param1, param2);
                IQueryable<T> result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheKey, canUseCacheIfPossible).AsQueryable();
                Stopwatch.Stop();
                cacheInfo.UsingTime += TimeSpan.FromTicks(Stopwatch.ElapsedTicks);
                return result;
            }
            else
            {
                return func.Invoke(GetQueryableForCahce() as IQueryable<TObject>, param1, param2);
            }
        }

        public IQueryable<T> Cache<T, P1, P2, P3>(Func<IQueryable<TObject>, P1, P2, P3, IQueryable<T>> func, P1 param1, P2 param2, P3 param3, bool canUseCacheIfPossible = true) where T : IEntity, new()
        {
            if (canUseCacheIfPossible)
            {
                Stopwatch.Restart();
                var cacheKey = func.Method.GetHashCode().ToString();
                var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
                cacheKey = cacheKey + param1 + param2 + param3;
                var queryableCacheExecution = new QueryableCacheDataProvider<T, P1, P2, P3>(cacheInfo, param1, param2, param3);
                IQueryable<T> result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheKey, canUseCacheIfPossible).AsQueryable();
                Stopwatch.Stop();
                cacheInfo.UsingTime += TimeSpan.FromTicks(Stopwatch.ElapsedTicks);
                return result;
            }
            else
            {
                return func.Invoke(GetQueryableForCahce() as IQueryable<TObject>, param1, param2, param3);
            }

        }

        public IQueryable<T> Cache<T, P1, P2, P3, P4>(Func<IQueryable<TObject>, P1, P2, P3, P4, IQueryable<T>> func, P1 param1, P2 param2, P3 param3, P4 param4, bool canUseCacheIfPossible = true) where T : IEntity, new()
        {
            if (canUseCacheIfPossible)
            {
                Stopwatch.Restart();
                var cacheKey = func.Method.GetHashCode().ToString();
                var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
                // cacheKey = cacheKey + param1 + param2 + param3 + param4;
                var queryableCacheExecution = new QueryableCacheDataProvider<T, P1, P2, P3, P4>(cacheInfo, param1, param2, param3, param4);
                IQueryable<T> result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, queryableCacheExecution.GenerateCacheKey(), canUseCacheIfPossible).AsQueryable();
                Stopwatch.Stop();
                cacheInfo.UsingTime += TimeSpan.FromTicks(Stopwatch.ElapsedTicks);
                return result;
            }
            else
            {
                return func.Invoke(GetQueryableForCahce() as IQueryable<TObject>, param1, param2, param3, param4);
            }
        }



        public IQueryable<T> RefreshCache<T>(Func<IQueryable<TObject>, IQueryable<T>> func) where T : EntityBase<T>, new()
        {
            var cacheKey = func.Method.GetHashCode().ToString();
            var cacheInfo = CacheConfig.CacheInfoDic[cacheKey];
            var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
            var result = CacheBase.RefreshCache<List<T>>(queryableCacheExecution, cacheInfo).AsQueryable();
            return result;
        }

        public IQueryable GetQueryableForCahce()
        {
            return (Activator.CreateInstance(ContextBase.GetType()) as IDbContextBase).Set<TObject>().AsNoTracking();
        }

        public Type GetDomainModelType()
        {
            return typeof(TObject);
        }

        public List<CheckRelationBeforeDeleteResult> CheckRelationBeforeDelete(string tableName, string keyName, string keyValue)
        {
            var sqlParams = new List<SqlParameter>();
            //todo: change it...(for all sp..method get spname,sqlparams..)...
            sqlParams.Add(new SqlParameter()
            {
                ParameterName = "@tableName",
                SqlValue = tableName
            });

            sqlParams.Add(new SqlParameter()
            {
                ParameterName = "@keyName",
                SqlValue = keyName
            });

            sqlParams.Add(new SqlParameter()
            {
                ParameterName = "@keyValue",
                SqlValue = keyValue
            });
            var context = (IDbContextBase)ContextBase;

            var sqlParamNames = new StringBuilder("exec " + "core.CheckRelationBeforeDelete" + " ");
            var sqlParametersValue = new List<SqlParameter>();

            foreach (var sqlParameter in sqlParams)
            {
                sqlParamNames.Append(sqlParameter.ParameterName + ",");
                sqlParametersValue.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.SqlValue));
            }

            sqlParamNames.Remove(sqlParamNames.Length - 1, 1);
            var result = context.Database.SqlQuery<CheckRelationBeforeDeleteResult>(sqlParamNames.ToString(), sqlParametersValue.ToArray()).ToList();

            return result;
        }

        public int LogicalDelete(TObject entity, bool allowSaveChange = true)
        {
            //..change key value....important...

            var dtResult = CheckRelationBeforeDelete(TableName, KeyName, entity[KeyName].ToString());

            if (dtResult.Count >= 1)
            {
                var s = new StringBuilder();
                s.AppendLine(" این رکورد در جداول");
                foreach (var item in dtResult)
                {
                    s.Append("<br/>");
                    s.Append(item.inUsedTbName + "," + " ");


                }
                s.Append("در حال استفاده می باشد");
                throw new Exception(s.ToString(), new Exception(s.ToString(), null));
            }

            ContextBase.SetContextState(entity, EntityState.Deleted);


            if (allowSaveChange)
                return SaveChanges();
            return 0;
        }


        public byte[] GetMaxTimeStamp()
        {
            return (Activator.CreateInstance(ContextBase.GetType()) as IDbContextBase).Set<TObject>().Max(item => item.TimeStamp);
        }
    }
}