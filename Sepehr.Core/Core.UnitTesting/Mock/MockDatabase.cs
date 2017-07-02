using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Cmn;
using System.Data;

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

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : _EntityBase
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
    }
}
