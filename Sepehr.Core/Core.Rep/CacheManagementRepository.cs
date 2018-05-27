using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Cache;
using Core.Cmn.Exceptions;
using Core.Cmn.Extensions;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Rep
{
    [Injectable(InterfaceType = typeof(ICacheManagementRepository), DomainName = "Core")]
    public class CacheManagementRepository : RepositoryBase<DeletedCachedRecord>, ICacheManagementRepository
    {
        public CacheManagementRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
            // _dbContext = dbContextBase;
        }

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
        [Cacheable(
            EnableSaveCacheOnHDD = true,
            AutoRefreshInterval = 3,
            EnableToFetchOnlyChangedDataFromDB = true,
            EnableUseCacheServer = false,
            DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB = true,
            EnableCoreSerialization = true
            )]
        public static IQueryable<DeletedCachedRecord> AllCache(IQueryable<DeletedCachedRecord> query)
        {
            return query.AsNoTracking();
        }

        public override IQueryable<DeletedCachedRecord> All(bool canUseCacheIfPossible = true)
        {
            return Cache<DeletedCachedRecord>(AllCache, canUseCacheIfPossible);
        }

        public void CreateSqlTriggerForDetectingDeletedRecords(string tableName, string pK)
        {
            DependencyInjectionFactory.CreateContextInstance().Database.ExecuteSqlCommand(string.Format(QueryToCreateTriggerForDeletedRecords, tableName, DateTime.Now.ToString(), pK));
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

        public List<IDeletedCachedRecord> GetDeletedRecordsByTable(string tableName, byte[] timeStamp, bool canUseCacheIfPossible = true)
        {
            var result = (IQueryable<DeletedCachedRecord>)CacheDataProvider.MakeQueryableForFetchingOnlyChangedDataFromDB(All(canUseCacheIfPossible).Where(rec => rec.TableName == tableName), new CacheInfo() { MaxTimeStamp = timeStamp, EnableToFetchOnlyChangedDataFromDB = true }, !canUseCacheIfPossible);
            return result.ToList<IDeletedCachedRecord>();
        }
    }
}