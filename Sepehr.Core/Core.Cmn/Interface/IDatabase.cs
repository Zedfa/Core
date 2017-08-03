using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Core.Cmn
{
    public interface IDatabase
    {
        int? CommandTimeout { get; set; }
        DbConnection Connection { get; }
        int ExecuteSqlCommand(string sql, params object[] parameters);
        int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters);

        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
        T SqlQueryForSingleResult<T>(string sql, params object[] parameters);

        IDbContextTransactionBase BeginTransaction(IsolationLevel isolationLevel);
        IDbContextTransactionBase BeginTransaction();
    }


}
