using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IDbEntityEntryBase
    {
        

        //
        // Summary:
        //     Gets the current property values for the tracked entity represented by this object.
         IDbPropertyValuesBase CurrentValues { get; }
        //
        // Summary:
        //     Gets the entity.
         object Entity { get; }
        //
        // Summary:
        //     Gets the original property values for the tracked entity represented by this
        //     object. The original values are usually the entity's property values as they
        //     were when last queried from the database.
        IDbPropertyValuesBase OriginalValues { get; }
        //
        // Summary:
        //     Gets or sets the state of the entity.
         EntityState State { get; set; }

        //
        // Summary:
        //     Returns a new instance of the generic System.Data.Entity.Infrastructure.DbEntityEntry`1
        //     class for the given generic type for the tracked entity represented by this object.
        //     Note that the type of the tracked entity must be compatible with the generic
        //     type or an exception will be thrown.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     A generic version.
        //DbEntityEntry<TEntity> Cast<TEntity>() where TEntity : class;
        ////
        //// Summary:
        ////     Gets an object that represents the collection navigation property from this entity
        ////     to a collection of related entities.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        // DbCollectionEntry Collection(string navigationProperty);
        ////
        //// Summary:
        ////     Gets an object that represents a complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the complex property.
        ////
        //// Returns:
        ////     An object representing the complex property.
        // DbComplexPropertyEntry ComplexProperty(string propertyName);
        ////
        //// Summary:
        ////     Determines whether the specified System.Data.Entity.Infrastructure.DbEntityEntry
        ////     is equal to this instance. Two System.Data.Entity.Infrastructure.DbEntityEntry
        ////     instances are considered equal if they are both entries for the same entity on
        ////     the same System.Data.Entity.DbContext.
        ////
        //// Parameters:
        ////   other:
        ////     The System.Data.Entity.Infrastructure.DbEntityEntry to compare with this instance.
        ////
        //// Returns:
        ////     true if the specified System.Data.Entity.Infrastructure.DbEntityEntry is equal
        ////     to this instance; otherwise, false .
      
        
        // DbPropertyValues GetDatabaseValues();
        ////
        //// Summary:
        ////     Asynchronously queries the database for copies of the values of the tracked entity
        ////     as they currently exist in the database. Note that changing the values in the
        ////     returned dictionary will not update the values in the database. If the entity
        ////     is not found in the database then null is returned.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation. The task result contains the
        ////     store values.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        // Task<DbPropertyValues> GetDatabaseValuesAsync();
        ////
        //// Summary:
        ////     Asynchronously queries the database for copies of the values of the tracked entity
        ////     as they currently exist in the database. Note that changing the values in the
        ////     returned dictionary will not update the values in the database. If the entity
        ////     is not found in the database then null is returned.
        ////
        //// Parameters:
        ////   cancellationToken:
        ////     A System.Threading.CancellationToken to observe while waiting for the task to
        ////     complete.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation. The task result contains the
        ////     store values.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
       
        // Task<DbPropertyValues> GetDatabaseValuesAsync(CancellationToken cancellationToken);
        ////
        //// Summary:
        ////     Returns a hash code for this instance.
        ////
        //// Returns:
        ////     A hash code for this instance, suitable for use in hashing algorithms and data
        ////     structures like a hash table.
       
        // DbEntityValidationResult GetValidationResult();
        ////
        //// Summary:
        ////     Gets an object that represents a member of the entity. The runtime type of the
        ////     returned object will vary depending on what kind of member is asked for. The
        ////     currently supported member types and their return types are: Reference navigation
        ////     property: System.Data.Entity.Infrastructure.DbReferenceEntry. Collection navigation
        ////     property: System.Data.Entity.Infrastructure.DbCollectionEntry. Primitive/scalar
        ////     property: System.Data.Entity.Infrastructure.DbPropertyEntry. Complex property:
        ////     System.Data.Entity.Infrastructure.DbComplexPropertyEntry.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the member.
        ////
        //// Returns:
        ////     An object representing the member.
        // DbMemberEntry Member(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a scalar or complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the property.
        ////
        //// Returns:
        ////     An object representing the property.
        // DbPropertyEntry Property(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents the reference (i.e. non-collection) navigation
        ////     property from this entity to another entity.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        // DbReferenceEntry Reference(string navigationProperty);
        ////
        //// Summary:
        ////     Reloads the entity from the database overwriting any property values with values
        ////     from the database. The entity will be in the Unchanged state after calling this
        ////     method.
        // void Reload();
        ////
        //// Summary:
        ////     Asynchronously reloads the entity from the database overwriting any property
        ////     values with values from the database. The entity will be in the Unchanged state
        ////     after calling this method.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        // Task ReloadAsync();
        ////
        //// Summary:
        ////     Asynchronously reloads the entity from the database overwriting any property
        ////     values with values from the database. The entity will be in the Unchanged state
        ////     after calling this method.
        ////
        //// Parameters:
        ////   cancellationToken:
        ////     A System.Threading.CancellationToken to observe while waiting for the task to
        ////     complete.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        // Task ReloadAsync(CancellationToken cancellationToken);
    
        
    }

    //
    // Summary:
    //     Instances of this class provide access to information about and control of entities
    //     that are being tracked by the System.Data.Entity.DbContext. Use the Entity or
    //     Entities methods of the context to obtain objects of this type.
    //
    // Type parameters:
    //   TEntity:
    //     The type of the entity.
    public interface IDbEntityEntryBase<TEntity> where TEntity : _EntityBase
    {
        //
        // Summary:
        //     Gets the current property values for the tracked entity represented by this object.
         IDbPropertyValuesBase CurrentValues { get; }
        //
        // Summary:
        //     Gets the entity.
         TEntity Entity { get; }
        //
        // Summary:
        //     Gets the original property values for the tracked entity represented by this
        //     object. The original values are usually the entity's property values as they
        //     were when last queried from the database.
        IDbPropertyValuesBase OriginalValues { get; }
        //
        // Summary:
        //     Gets or sets the state of the entity.
         EntityState State { get; set; }

        ////
        //// Summary:
        ////     Gets an object that represents the collection navigation property from this entity
        ////     to a collection of related entities.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //public DbCollectionEntry Collection(string navigationProperty);
        ////
        //// Summary:
        ////     Gets an object that represents the collection navigation property from this entity
        ////     to a collection of related entities.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     An expression representing the navigation property.
        ////
        //// Type parameters:
        ////   TElement:
        ////     The type of elements in the collection.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //public DbCollectionEntry<TEntity, TElement> Collection<TElement>(Expression<Func<TEntity, ICollection<TElement>>> navigationProperty) where TElement : class;
        ////
        //// Summary:
        ////     Gets an object that represents the collection navigation property from this entity
        ////     to a collection of related entities.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Type parameters:
        ////   TElement:
        ////     The type of elements in the collection.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //public DbCollectionEntry<TEntity, TElement> Collection<TElement>(string navigationProperty) where TElement : class;
        ////
        //// Summary:
        ////     Gets an object that represents a complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the complex property.
        ////
        //// Returns:
        ////     An object representing the complex property.
        //public DbComplexPropertyEntry ComplexProperty(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the complex property.
        ////
        //// Type parameters:
        ////   TComplexProperty:
        ////     The type of the complex property.
        ////
        //// Returns:
        ////     An object representing the complex property.
        //public DbComplexPropertyEntry<TEntity, TComplexProperty> ComplexProperty<TComplexProperty>(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a complex property of this entity.
        ////
        //// Parameters:
        ////   property:
        ////     An expression representing the complex property.
        ////
        //// Type parameters:
        ////   TComplexProperty:
        ////     The type of the complex property.
        ////
        //// Returns:
        ////     An object representing the complex property.
        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //[SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#", Justification = "Rule predates more fluent naming conventions.")]
        //public DbComplexPropertyEntry<TEntity, TComplexProperty> ComplexProperty<TComplexProperty>(Expression<Func<TEntity, TComplexProperty>> property);
        ////
        //// Summary:
        ////     Determines whether the specified System.Data.Entity.Infrastructure.DbEntityEntry`1
        ////     is equal to this instance. Two System.Data.Entity.Infrastructure.DbEntityEntry`1
        ////     instances are considered equal if they are both entries for the same entity on
        ////     the same System.Data.Entity.DbContext.
        ////
        //// Parameters:
        ////   other:
        ////     The System.Data.Entity.Infrastructure.DbEntityEntry`1 to compare with this instance.
        ////
        //// Returns:
        ////     true if the specified System.Data.Entity.Infrastructure.DbEntityEntry`1 is equal
        ////     to this instance; otherwise, false .
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool Equals(DbEntityEntry<TEntity> other);
        ////
        //// Summary:
        ////     Determines whether the specified System.Object is equal to this instance. Two
        ////     System.Data.Entity.Infrastructure.DbEntityEntry`1 instances are considered equal
        ////     if they are both entries for the same entity on the same System.Data.Entity.DbContext.
        ////
        //// Parameters:
        ////   obj:
        ////     The System.Object to compare with this instance.
        ////
        //// Returns:
        ////     true if the specified System.Object is equal to this instance; otherwise, false
        ////     .
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override bool Equals(object obj);
        ////
        //// Summary:
        ////     Queries the database for copies of the values of the tracked entity as they currently
        ////     exist in the database. Note that changing the values in the returned dictionary
        ////     will not update the values in the database. If the entity is not found in the
        ////     database then null is returned.
        ////
        //// Returns:
        ////     The store values.
        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //public DbPropertyValues GetDatabaseValues();
        ////
        //// Summary:
        ////     Asynchronously queries the database for copies of the values of the tracked entity
        ////     as they currently exist in the database. Note that changing the values in the
        ////     returned dictionary will not update the values in the database. If the entity
        ////     is not found in the database then null is returned.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation. The task result contains the
        ////     store values.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        //public Task<DbPropertyValues> GetDatabaseValuesAsync();
        ////
        //// Summary:
        ////     Asynchronously queries the database for copies of the values of the tracked entity
        ////     as they currently exist in the database. Note that changing the values in the
        ////     returned dictionary will not update the values in the database. If the entity
        ////     is not found in the database then null is returned.
        ////
        //// Parameters:
        ////   cancellationToken:
        ////     A System.Threading.CancellationToken to observe while waiting for the task to
        ////     complete.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation. The task result contains the
        ////     store values.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        //[AsyncStateMachine(typeof(DbEntityEntry<>.< GetDatabaseValuesAsync > d__0))]
        //[DebuggerStepThrough]
        //public Task<DbPropertyValues> GetDatabaseValuesAsync(CancellationToken cancellationToken);
        ////
        //// Summary:
        ////     Returns a hash code for this instance.
        ////
        //// Returns:
        ////     A hash code for this instance, suitable for use in hashing algorithms and data
        ////     structures like a hash table.
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override int GetHashCode();
        ////
        //// Summary:
        ////     Gets the System.Type of the current instance.
        ////
        //// Returns:
        ////     The exact runtime type of the current instance.
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //public Type GetType();
        ////
        //// Summary:
        ////     Validates this System.Data.Entity.Infrastructure.DbEntityEntry`1 instance and
        ////     returns validation result.
        ////
        //// Returns:
        ////     Entity validation result. Possibly null if DbContext.ValidateEntity(DbEntityEntry,
        ////     IDictionary{object, object}) method is overridden.
        //[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //public DbEntityValidationResult GetValidationResult();
        ////
        //// Summary:
        ////     Gets an object that represents a member of the entity. The runtime type of the
        ////     returned object will vary depending on what kind of member is asked for. The
        ////     currently supported member types and their return types are: Reference navigation
        ////     property: System.Data.Entity.Infrastructure.DbReferenceEntry. Collection navigation
        ////     property: System.Data.Entity.Infrastructure.DbCollectionEntry. Primitive/scalar
        ////     property: System.Data.Entity.Infrastructure.DbPropertyEntry. Complex property:
        ////     System.Data.Entity.Infrastructure.DbComplexPropertyEntry.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the member.
        ////
        //// Returns:
        ////     An object representing the member.
        //public DbMemberEntry Member(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a member of the entity. The runtime type of the
        ////     returned object will vary depending on what kind of member is asked for. The
        ////     currently supported member types and their return types are: Reference navigation
        ////     property: System.Data.Entity.Infrastructure.DbReferenceEntry`2. Collection navigation
        ////     property: System.Data.Entity.Infrastructure.DbCollectionEntry`2. Primitive/scalar
        ////     property: System.Data.Entity.Infrastructure.DbPropertyEntry`2. Complex property:
        ////     System.Data.Entity.Infrastructure.DbComplexPropertyEntry`2.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the member.
        ////
        //// Type parameters:
        ////   TMember:
        ////     The type of the member.
        ////
        //// Returns:
        ////     An object representing the member.
        //public DbMemberEntry<TEntity, TMember> Member<TMember>(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a scalar or complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the property.
        ////
        //// Returns:
        ////     An object representing the property.
        //public DbPropertyEntry Property(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents a scalar or complex property of this entity.
        ////
        //// Parameters:
        ////   property:
        ////     An expression representing the property.
        ////
        //// Type parameters:
        ////   TProperty:
        ////     The type of the property.
        ////
        //// Returns:
        ////     An object representing the property.
        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //[SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#", Justification = "Rule predates more fluent naming conventions.")]
        //public DbPropertyEntry<TEntity, TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> property);
        ////
        //// Summary:
        ////     Gets an object that represents a scalar or complex property of this entity.
        ////
        //// Parameters:
        ////   propertyName:
        ////     The name of the property.
        ////
        //// Type parameters:
        ////   TProperty:
        ////     The type of the property.
        ////
        //// Returns:
        ////     An object representing the property.
        //public DbPropertyEntry<TEntity, TProperty> Property<TProperty>(string propertyName);
        ////
        //// Summary:
        ////     Gets an object that represents the reference (i.e. non-collection) navigation
        ////     property from this entity to another entity.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //public DbReferenceEntry Reference(string navigationProperty);
        ////
        //// Summary:
        ////     Gets an object that represents the reference (i.e. non-collection) navigation
        ////     property from this entity to another entity.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     The name of the navigation property.
        ////
        //// Type parameters:
        ////   TProperty:
        ////     The type of the property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //public DbReferenceEntry<TEntity, TProperty> Reference<TProperty>(string navigationProperty) where TProperty : class;
        ////
        //// Summary:
        ////     Gets an object that represents the reference (i.e. non-collection) navigation
        ////     property from this entity to another entity.
        ////
        //// Parameters:
        ////   navigationProperty:
        ////     An expression representing the navigation property.
        ////
        //// Type parameters:
        ////   TProperty:
        ////     The type of the property.
        ////
        //// Returns:
        ////     An object representing the navigation property.
        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //public DbReferenceEntry<TEntity, TProperty> Reference<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty) where TProperty : class;
        ////
        //// Summary:
        ////     Reloads the entity from the database overwriting any property values with values
        ////     from the database. The entity will be in the Unchanged state after calling this
        ////     method.
        //public void Reload();
        ////
        //// Summary:
        ////     Asynchronously reloads the entity from the database overwriting any property
        ////     values with values from the database. The entity will be in the Unchanged state
        ////     after calling this method.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        //public Task ReloadAsync();
        ////
        //// Summary:
        ////     Asynchronously reloads the entity from the database overwriting any property
        ////     values with values from the database. The entity will be in the Unchanged state
        ////     after calling this method.
        ////
        //// Parameters:
        ////   cancellationToken:
        ////     A System.Threading.CancellationToken to observe while waiting for the task to
        ////     complete.
        ////
        //// Returns:
        ////     A task that represents the asynchronous operation.
        ////
        //// Remarks:
        ////     Multiple active operations on the same context instance are not supported. Use
        ////     'await' to ensure that any asynchronous operations have completed before calling
        ////     another method on this context.
        //public Task ReloadAsync(CancellationToken cancellationToken);
        ////
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public override string ToString();

        ////
        //// Summary:
        ////     Returns a new instance of the non-generic System.Data.Entity.Infrastructure.DbEntityEntry
        ////     class for the tracked entity represented by this object.
        ////
        //// Parameters:
        ////   entry:
        ////     The object representing the tracked entity.
        ////
        //// Returns:
        ////     A non-generic version.
        //[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Intentionally just implicit to reduce API clutter.")]
        //public static implicit operator DbEntityEntry(DbEntityEntry<TEntity> entry);
    }

}
