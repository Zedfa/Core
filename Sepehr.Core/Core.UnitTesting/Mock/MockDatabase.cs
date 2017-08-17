using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Core.UnitTesting.Mock
{
    public class MockDatabase : IDatabase
    {
        public int? CommandTimeout
        {
            get; set;
        }

        public DbConnection Connection
        {
            get
            {
                return null;
            }
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return 0;
        }

        public int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            return 0;
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return null;
        }

        public IDbContextTransactionBase BeginTransaction(IsolationLevel isolationLevel)
        {
            return null;
        }

        public IDbContextTransactionBase BeginTransaction()
        {
            return null;
        }

        public T SqlQueryForSingleResult<T>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}