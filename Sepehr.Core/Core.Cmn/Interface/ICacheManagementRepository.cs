using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IDeletedCachedRecord
    {
        int Id { get; set; }
        string DeletedTimeStamp { get; set; }
        int DeletedEntityId { get; }
        string TableName { get; set; }
        DateTime DeletionDateTime { get; set; }

    }
    public interface ICacheManagementRepository
    {
        void CreateSqlTriggerForDetectingDeletedRecords(string tableName, string keyName);
        List<IDeletedCachedRecord> GetDeletedRecordsByTable(string tableName, byte[] timeStamp, bool canUseCacheIfPossible = true);
        int Delete();
    }
}
