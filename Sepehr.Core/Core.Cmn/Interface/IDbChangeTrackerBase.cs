using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Cmn
{
   public interface IDbChangeTrackerBase
    {
       
            //
            // Summary:
            //     Detects changes made to the properties and relationships of POCO entities. Note
            //     that some types of entity (such as change tracking proxies and entities that
            //     derive from System.Data.Entity.Core.Objects.DataClasses.EntityObject) report
            //     changes automatically and a call to DetectChanges is not normally needed for
            //     these types of entities. Also note that normally DetectChanges is called automatically
            //     by many of the methods of System.Data.Entity.DbContext and its related classes
            //     such that it is rare that this method will need to be called explicitly. However,
            //     it may be desirable, usually for performance reasons, to turn off this automatic
            //     calling of DetectChanges using the AutoDetectChangesEnabled flag from System.Data.Entity.DbContext.Configuration.
             void DetectChanges();
            //
            // Summary:
            //     Gets System.Data.Entity.Infrastructure.DbEntityEntry objects for all the entities
            //     tracked by this context.
            //
            // Returns:
            //     The entries.
             IEnumerable<IDbEntityEntryBase> Entries();
            //
            // Summary:
            //     Gets System.Data.Entity.Infrastructure.DbEntityEntry objects for all the entities
            //     of the given type tracked by this context.
            //
            // Type parameters:
            //   TEntity:
            //     The type of the entity.
            //
            // Returns:
            //     The entries.
       
             IEnumerable<IDbEntityEntryBase<TEntity>> Entries<TEntity>() where TEntity : ObjectBase;
           
            //
            // Summary:
            //     Checks if the System.Data.Entity.DbContext is tracking any new, deleted, or changed
            //     entities or relationships that will be sent to the database if System.Data.Entity.DbContext.SaveChanges
            //     is called.
            //
            // Returns:
            //     True if underlying System.Data.Entity.DbContext have changes, else false.
            //
            // Remarks:
            //     Functionally, calling this method is equivalent to checking if there are any
            //     entities or relationships in the Added, Updated, or Deleted state. Note that
            //     this method calls System.Data.Entity.Infrastructure.DbChangeTracker.DetectChanges
            //     unless System.Data.Entity.Infrastructure.DbContextConfiguration.AutoDetectChangesEnabled
            //     has been set to false.
             bool HasChanges();
          
        }
    
}
