using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Cmn;

namespace Core.UnitTesting.Mock
{
    public class MockDbChangeTracker : IDbChangeTrackerBase
    {
        private MockDbContextBase _context;

        public MockDbChangeTracker(MockDbContextBase context)
        {
            _context = context;
        }

        public void DetectChanges()
        {
            
        }

        public IEnumerable<IDbEntityEntryBase> Entries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDbEntityEntryBase<TEntity>> Entries<TEntity>() where TEntity : _EntityBase
        {
            throw new NotImplementedException();
        }

        public bool HasChanges()
        {
            foreach (object obj in _context.StateMap.Keys)
            {
                EntityState state = _context.StateMap[obj];
                if (state == EntityState.Added || state == EntityState.Modified || state == EntityState.Deleted)
                {
                    return true;                    
                }
            }

            return false;
        }
    }
}
