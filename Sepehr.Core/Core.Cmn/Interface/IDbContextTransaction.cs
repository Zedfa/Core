using System;

using System.Data.Common;


namespace Core.Cmn
{
    public interface IDbContextTransactionBase : IDisposable
    {
        //
        // Summary:
        //     Gets the database (store) transaction that is underlying this context transaction.
         DbTransaction UnderlyingTransaction { get; }

        //
        // Summary:
        //     Commits the underlying store transaction
         void Commit();
        //
        // Summary:
        //     Cleans up this transaction object and ensures the Entity Framework is no longer
        //     using that transaction.
        //
        Type GetType();
        //
        // Summary:
        //     Rolls back the underlying store transaction
         void Rollback();
        //
      
       
    }
}
