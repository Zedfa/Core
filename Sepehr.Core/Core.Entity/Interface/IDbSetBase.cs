using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Core.Cmn;
namespace Core.Entity.Interface
{
    // Summary:
    //     An IDbSet represents the collection of all entities in the context, or that
    //     can be queried from the database, of a given type. DbSet is a concrete implementation
    //     of IDbSet.
    //
    // Type parameters:
    //   TEntity:
    //     The type that defines the set.
    public interface IDbSetBase<TEntity>
        : IQueryable<TEntity>
        where TEntity : EntityBase<TEntity>
    {
        // Summary:
        //     Gets an System.Collections.ObjectModel.ObservableCollection<T> that represents
        //     a local view of all Added, Unchanged, and Modified entities in this set.
        //     This local view will stay in sync as entities are added or removed from the
        //     context. Likewise, entities added to or removed from the local view will
        //     automatically be added to or removed from the context.
        //
        // Remarks:
        //     This property can be used for data binding by populating the set with data,
        //     for example by using the Load extension method, and then binding to the local
        //     data through this property. For WPF bind to this property directly. For Windows
        //     Forms bind to the result of calling ToBindingList on this property
        ObservableCollection<TEntity> Local { get; }

        // Summary:
        //     Adds the given entity to the context underlying the set in the Added state
        //     such that it will be inserted into the database when SaveChanges is called.
        //
        // Parameters:
        //   entity:
        //     The entity to add.
        //
        // Returns:
        //     The entity.
        //
        // Remarks:
        //     Note that entities that are already in the context in some other state will
        //     have their state set to Added. Add is a no-op if the entity is already in
        //     the context in the Added state.
        TEntity Add(TEntity entity);
        //
        // Summary:
        //     Attaches the given entity to the context underlying the set. That is, the
        //     entity is placed into the context in the Unchanged state, just as if it had
        //     been read from the database.
        //
        // Parameters:
        //   entity:
        //     The entity to attach.
        //
        // Returns:
        //     The entity.
        //
        // Remarks:
        //     Attach is used to repopulate a context with an entity that is known to already
        //     exist in the database.  SaveChanges will therefore not attempt to insert
        //     an attached entity into the database because it is assumed to already be
        //     there.  Note that entities that are already in the context in some other
        //     state will have their state set to Unchanged. Attach is a no-op if the entity
        //     is already in the context in the Unchanged state.
        TEntity Attach(TEntity entity);
        //
        // Summary:
        //     Creates a new instance of an entity for the type of this set.  Note that
        //     this instance is NOT added or attached to the set.  The instance returned
        //     will be a proxy if the underlying context is configured to create proxies
        //     and the entity type meets the requirements for creating a proxy.
        //
        // Returns:
        //     The entity instance, which may be a proxy.
        TEntity Create();
        //
        // Summary:
        //     Creates a new instance of an entity for the type of this set or for a type
        //     derived from the type of this set.  Note that this instance is NOT added
        //     or attached to the set.  The instance returned will be a proxy if the underlying
        //     context is configured to create proxies and the entity type meets the requirements
        //     for creating a proxy.
        //
        // Type parameters:
        //   TDerivedEntity:
        //     The type of entity to create.
        //
        // Returns:
        //     The entity instance, which may be a proxy.
        TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity;
        //
        // Summary:
        //     Finds an entity with the given primary key values.  If an entity with the
        //     given primary key values exists in the context, then it is returned immediately
        //     without making a request to the store. Otherwise, a request is made to the
        //     store for an entity with the given primary key values and this entity, if
        //     found, is attached to the context and returned. If no entity is found in
        //     the context or the store, then null is returned.
        //
        // Parameters:
        //   keyValues:
        //     The values of the primary key for the entity to be found.
        //
        // Returns:
        //     The entity found, or null.
        //
        // Remarks:
        //     The ordering of composite key values is as defined in the EDM, which is in
        //     turn as defined in the designer, by the Code First fluent API, or by the
        //     DataMember attribute.
        TEntity Find(params object[] keyValues);
        //
        // Summary:
        //     Marks the given entity as Deleted such that it will be deleted from the database
        //     when SaveChanges is called. Note that the entity must exist in the context
        //     in some other state before this method is called.
        //
        // Parameters:
        //   entity:
        //     The entity to remove.
        //
        // Returns:
        //     The entity.
        //
        // Remarks:
        //     Note that if the entity exists in the context in the Added state, then this
        //     method will cause it to be detached from the context. This is because an
        //     Added entity is assumed not to exist in the database such that trying to
        //     delete it does not make sense.
        TEntity Remove(TEntity entity);
       
       // void OrderBy(Expression<Func<object, object>> keySelector);

        IQueryable<TEntity> AsNoTrackingAll();

        int Delete(IQueryable<TEntity> entititesPredicateToRemove);
        int Update(IQueryable<TEntity> source, Expression<Func<TEntity, TEntity>> updateExpression);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> source);

        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
    }
}
