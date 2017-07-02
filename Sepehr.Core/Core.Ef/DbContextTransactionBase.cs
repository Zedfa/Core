using Core.Cmn;
using System;
using System.Data.Common;
using System.Data.Entity;

namespace Core.Ef
{
    public class DbContextTransactionBase : IDbContextTransactionBase
    {
        private DbContextTransaction _dbContextTransaction;
        public DbContextTransactionBase(DbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public DbTransaction UnderlyingTransaction
        {
            get
            {
                return _dbContextTransaction.UnderlyingTransaction;
            }
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();

        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();

        }
    }
}
