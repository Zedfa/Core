using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Cmn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cmn.Exceptions;

namespace Core.Rep
{
    [Injectable(InterfaceType = typeof(ICacheManagementRepository), DomainName = "Core")]
    public class CacheManagementRepository : RepositoryBase<DeletedCachedRecord>, ICacheManagementRepository
    {
        public static string QueryToCreateTriggerForDeletedRecords
        {
            get
            {
                return @"
                IF Not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'{0}_InsertDeletedRecordsTrigger'))
                EXEC dbo.sp_executesql @statement = N'
                -- =============================================
                -- Author:	 Core Framework
                -- Create date: {1}
                -- Description:	it''s create automatically by Core Framework.
                -- =============================================
                CREATE TRIGGER {0}_InsertDeletedRecordsTrigger
                   ON  {0}
                   AFTER DELETE
                AS 
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;
	                insert into Core.DeletedCachedRecords
                select  convert(nvarchar(max), {2} ) DeletedTimeStamp , ''{0}'' TableName , GETDATE() DeletionDateTime , null TimeStamp from deleted
                end
                '";

            }
        }
        public string CacheKey { get; set; }
        public CacheManagementRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {

            // _dbContext = dbContextBase;
        }

        [Cacheable(EnableSaveCacheOnHDD = true, ExpireCacheSecondTime = 1, EnableToFetchOnlyChangedDataFromDB = true, EnableUseCacheServer = false, DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB = true)]
        public static IQueryable<DeletedCachedRecord> AllCache(IQueryable<DeletedCachedRecord> query)
        {
            return query.AsNoTracking();
        }
        public override IQueryable<DeletedCachedRecord> All(bool canUseCacheIfPossible = true)
        {
            return Cache<DeletedCachedRecord>(AllCache, canUseCacheIfPossible);
        }
        public List<IDeletedCachedRecord> GetDeletedRecordsByTable(string tableName, UInt64 timeStampUInt)
        {
            return All().Where(rec => rec.TimeStampUnit > timeStampUInt && rec.TableName == tableName).Cast<IDeletedCachedRecord>().ToList();
        }
        public void CreateSqlTriggerForDetectingDeletedRecords(string tableName, string pK)
        {
            DependencyInjectionFactory.CreateContextInstance().Database.ExecuteSqlCommand(string.Format(QueryToCreateTriggerForDeletedRecords, tableName, DateTime.Now.ToString(), pK));
        }
        public void CheckServiceBrokerOnDb()
        {
            var dbName = DependencyInjectionFactory.CreateContextInstance().Database.Connection.Database;
            var query = $"Select is_broker_enabled from sys.databases where name = '{dbName}'";
            var isServiceBrokerEnabled = DependencyInjectionFactory.CreateContextInstance().Database.SqlQueryForSingleResult<bool>(query);
            if (!isServiceBrokerEnabled)
            {
                throw new ServiceBrokerIsNotEnabledException(dbName);
            }
        }

        public int Delete()
        {
            //return 1;
            var dateTime = DateTime.Now.AddHours(-1);
            var id = DependencyInjectionFactory.CreateContextInstance().Set<DeletedCachedRecord>().Where(item => item.DeletionDateTime < dateTime).Delete();
            List<DeletedCachedRecord> NetCache;
            if (string.IsNullOrWhiteSpace(CacheKey))
            {
                Func<IQueryable<DeletedCachedRecord>, IQueryable<DeletedCachedRecord>> funcCache = AllCache;
                CacheKey = funcCache.Method.GetHashCode().ToString();
            }

            if (CacheService.TryGetCache<List<DeletedCachedRecord>>(CacheKey, out NetCache))
            {
                NetCache.RemoveAll(item => item.DeletionDateTime < dateTime);
            }
            else
            {
                //throw new Exception("CacheManagementRepository on delete method has a exception...");
            }

            return id;
        }
    }
}
