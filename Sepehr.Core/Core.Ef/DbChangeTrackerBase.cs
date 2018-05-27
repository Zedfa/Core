using Core.Cmn;
using System.Collections.Generic;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Core.Ef
{
    public class DbChangeTrackerBase : IDbChangeTrackerBase
    {
        private DbChangeTracker _changeTracker;
        public DbChangeTrackerBase(DbChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
        }
        public void DetectChanges()
        {
            _changeTracker.DetectChanges();
        }

        public IEnumerable<IDbEntityEntryBase> Entries()
        {
            return _changeTracker.Entries().Select(entry => new DbEntityEntryBase(entry)).AsEnumerable();
        }

        public IEnumerable<IDbEntityEntryBase<TEntity>> Entries<TEntity>() where TEntity : ObjectBase
        {
            return _changeTracker.Entries<TEntity>().Select(entry => new DbEntityEntryBase<TEntity>(entry)).AsEnumerable();
        }
        
        public bool HasChanges()
        {
            return _changeTracker.HasChanges();
        }
    }
}
