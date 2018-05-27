
using System;

namespace Core.Cmn
{
    // Summary:
    //     The state of an entity object.
    [Flags]
    public enum EntityState
    {
        // Summary:
        //     The object exists but is not being tracked. An entity is in this state immediately
        //     after it has been created and before it is added to the object context. An
        //     entity is also in this state after it has been removed from the context by
        //     calling the System.Data.Objects.ObjectContext.Detach(System.Object) method
        //     or if it is loaded by using a System.Data.Objects.MergeOption.NoTrackingSystem.Data.Objects.MergeOption.
        //     There is no System.Data.Objects.ObjectStateEntry instance associated with
        //     objects in the System.Data.EntityState.Detached state.
        Detached = 1,
        //
        // Summary:
        //     The object has not been modified since it was attached to the context or
        //     since the last time that the System.Data.Objects.ObjectContext.SaveChanges()
        //     method was called.
        Unchanged = 2,
        //
        // Summary:
        //     The object is new, has been added to the object context, and the System.Data.Objects.ObjectContext.SaveChanges()
        //     method has not been called. After the changes are saved, the object state
        //     changes to System.Data.EntityState.Unchanged. Objects in the System.Data.EntityState.Added
        //     state do not have original values in the System.Data.Objects.ObjectStateEntry.
        Added = 4,
        //
        // Summary:
        //     The object has been deleted from the object context. After the changes are
        //     saved, the object state changes to System.Data.EntityState.Detached.
        Deleted = 8,
        //
        // Summary:
        //     One of the scalar properties on the object was modified and the System.Data.Objects.ObjectContext.SaveChanges()
        //     method has not been called. In POCO entities without change-tracking proxies,
        //     the state of the modified properties changes to System.Data.EntityState.Modified
        //     when the System.Data.Objects.ObjectContext.DetectChanges() method is called.
        //     After the changes are saved, the object state changes to System.Data.EntityState.Unchanged.
        Modified = 16,
    }

    //
    // Summary:
    //     Controls the transaction creation behavior while executing a database command
    //     or query.
    [Flags]
    public enum TransactionalBehavior
    {
        //
        // Summary:
        //     If no transaction is present then a new transaction will be used for the operation.
        EnsureTransaction = 0,
        //
        // Summary:
        //     If an existing transaction is present then use it, otherwise execute the command
        //     or query without a transaction.
        DoNotEnsureTransaction = 1
    }
    public interface IDbContextBase : IDisposable
    {
        IDatabase Database { get; }
        IDbContextConfigurationBase Configuration { get; }
        IDbSetBase<T> Set<T>() where T : ObjectBase , new();
        // DbSetBase<T> Set<T>() where T : ObjectBase , new();
        ///remark:DbSet Must be Implemented.
        //DbSet<T> Set<T>() where T : class;

        int SaveChanges();
        //DbChangeTracker ChangeTracker;
        //IEnumerable<DbEntityValidationResult> GetValidationErrors();
        void SetContextState<T>(T entity, EntityState entityState) where T : ObjectBase , new();
        // DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        bool DisableExceptionLogger { get; set; }
        IDbChangeTrackerBase ChangeTracker { get; }

    }

    internal interface IDbContextInternal
    {
        string ConnectionString { get; }
    }
}
