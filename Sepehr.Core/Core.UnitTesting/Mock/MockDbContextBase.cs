using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Ef;
using Core.Entity;
using Core.UnitTesting.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.UnitTesting.Mock
{
    [Injectable(InterfaceType = typeof(IDbContextBase), Version = 1000)]
    public class MockDbContextBase : IDbContextBase
    {
        private static IDictionary<Type, string> _entityIdPropertyNameMap;

        private static IDictionary<object, EntityState> _stateMap = new Dictionary<object, EntityState>();

        private static Type[] ENTITY_TYPES =
                        {
            typeof(Company),
            typeof(CompanyChart),
            typeof(Constant),
            typeof(ConstantCategory),
            typeof(ExceptionLog),
            typeof(Log),
        };
        private MethodInfo _addEntityMethod;
        private IDbContextConfigurationBase _configuration;
        private IDatabase _database;
        private MethodInfo _deleteEntityMethod;
        private MethodInfo _updateEntityMethod;
        public MockDbContextBase()
        {
            DisableExceptionLogger = true;
        }

        public static IDictionary<Type, string> EntityIdPropertyNameMap
        {
            get { return _entityIdPropertyNameMap ?? (_entityIdPropertyNameMap = new Dictionary<Type, string>()); }
        }

        public IDbChangeTrackerBase ChangeTracker
        {
            get
            {
                return null;
            }
        }

        public IDbContextConfigurationBase Configuration
        {
            get
            {
                return _configuration ?? (_configuration = MockHelperBase.BuildMockDbContextConfiguration());
            }
        }

        public IDatabase Database
        {
            get
            {
                return _database ?? (_database = MockHelperBase.BuildMockDatabase());
            }
        }

        public bool DisableExceptionLogger
        {
            get; set;
        }

        public IDictionary<object, EntityState> StateMap { get { return _stateMap; } }
        protected MethodInfo AddEntityMethod
        {
            get
            {
                return _addEntityMethod ?? (_addEntityMethod = typeof(MockDbContextBase).GetMethod("AddEntity", BindingFlags.Instance | BindingFlags.NonPublic));
            }
        }
        protected MethodInfo DeleteEntityMethod
        {
            get
            {
                return _deleteEntityMethod ?? (_deleteEntityMethod = typeof(MockDbContextBase).GetMethod("DeleteEntity", BindingFlags.Instance | BindingFlags.NonPublic));
            }
        }

        protected MethodInfo UpdateEntityMethod
        {
            get
            {
                return _updateEntityMethod ?? (_updateEntityMethod = typeof(MockDbContextBase).GetMethod("UpdateEntity", BindingFlags.Instance | BindingFlags.NonPublic));
            }
        }
        public void Dispose()
        {
        }

        public DbEntityEntryBase<TEntity> Entry<TEntity>(TEntity entity) where TEntity : _EntityBase
        {
            return null;
        }

        public int SaveChanges()
        {
            int count = 0;
            foreach (object obj in _stateMap.Keys)
            {
                switch (_stateMap[obj])
                {
                    case EntityState.Added:
                        count++;
                        HandleAddEntity(obj);
                        break;

                    case EntityState.Modified:
                        count++;
                        HandleModifyEntity(obj);
                        break;

                    case EntityState.Deleted:
                        count++;
                        HandleDeleteEntity(obj);
                        break;
                }
            }

            foreach (object obj in _stateMap.Keys.ToList())
            {
                _stateMap[obj] = EntityState.Unchanged;
            }

            return count;
        }

        public IDbSetBase<TEntity> Set<TEntity>() where TEntity : EntityBase<TEntity>, new()
        {
            // check core entities
            IDbSetBase<TEntity> result = GetEntitySetCore<TEntity>();

            // check project entities
            if (result == null)
            {
                result = GetEntitySetProject<TEntity>();
            }

            return result;
        }

        public void SetContextState<T>(EntityBase<T> entity, EntityState entityState) where T : EntityBase<T>, new()
        {
            _stateMap[entity] = entityState;
        }

        protected void AddEntity<TEntity>(TEntity entity) where TEntity : EntityBase<TEntity>, new()
        {
            long longId = 1;
            if (Set<TEntity>().Count() > 0)
                longId = Set<TEntity>().Max<TEntity, long>(e => e.CacheId) + 1;

            Type entityType = entity.GetType();
            PropertyInfo idProperty = GetIdProperty(entityType);

            if (idProperty != null)
            {
                if (idProperty.PropertyType == typeof(int))
                {
                    idProperty.SetValue(entity, (int)longId);
                }
                else if (idProperty.PropertyType == typeof(long))
                {
                    idProperty.SetValue(entity, longId);
                }
                else if (idProperty.PropertyType == typeof(Guid))
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }

                Set<TEntity>().Add(entity);
            }
        }

        protected void DeleteEntity<TEntity>(TEntity entity) where TEntity : EntityBase<TEntity>, new()
        {
            entity = FindEntity(entity);
            if (entity != null)
            {
                Set<TEntity>().Remove(entity);
            }
        }

        protected TEntity FindEntity<TEntity>(TEntity entity) where TEntity : EntityBase<TEntity>, new()
        {
            TEntity found = null;

            Type entityType = entity.GetType();
            PropertyInfo idProperty = GetIdProperty(entityType);

            if (idProperty != null)
            {
                foreach (TEntity e in Set<TEntity>())
                {
                    if (idProperty.PropertyType == typeof(int))
                    {
                        int entityId = (int)idProperty.GetValue(entity);
                        int id = (int)idProperty.GetValue(e);
                        if (entityId == id)
                        {
                            found = e;
                            break;
                        }
                    }
                    else if (idProperty.PropertyType == typeof(long))
                    {
                        long entityId = (long)idProperty.GetValue(entity);
                        long id = (long)idProperty.GetValue(e);
                        if (entityId == id)
                        {
                            found = e;
                            break;
                        }
                    }
                    else if (idProperty.PropertyType == typeof(Guid))
                    {
                        Guid entityId = (Guid)idProperty.GetValue(entity);
                        Guid id = (Guid)idProperty.GetValue(e);
                        if (entityId == id)
                        {
                            found = e;
                            break;
                        }
                    }
                }
            }

            return found;
        }

        protected virtual IDbSetBase<TEntity> GetEntitySetProject<TEntity>() where TEntity : EntityBase<TEntity>, new()
        {
            return EntityUnitTestHelperBase<TEntity>.GetMockData();
            // will be overridden in child classes
          //  return null;
        }

        protected virtual bool HandleAddEntity(object entity)
        {
            bool handled = false;

            foreach (Type entityType in ENTITY_TYPES)
            {
                if (entity.GetType() == entityType)
                {
                    MethodInfo generic = AddEntityMethod.MakeGenericMethod(entityType);
                    generic.Invoke(this, new object[] { entity });
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        protected virtual bool HandleDeleteEntity(object entity)
        {
            bool handled = false;

            foreach (Type entityType in ENTITY_TYPES)
            {
                if (entity.GetType() == entityType)
                {
                    MethodInfo generic = DeleteEntityMethod.MakeGenericMethod(entityType);
                    generic.Invoke(this, new object[] { entity });
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        protected virtual bool HandleModifyEntity(object entity)
        {
            bool handled = false;

            foreach (Type entityType in ENTITY_TYPES)
            {
                if (entity.GetType() == entityType)
                {
                    MethodInfo generic = UpdateEntityMethod.MakeGenericMethod(entityType);
                    generic.Invoke(this, new object[] { entity });
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        protected void UpdateEntity<TEntity>(TEntity entity) where TEntity : EntityBase<TEntity>, new()
        {
            TEntity old = FindEntity(entity);
            if (old != null)
            {
                Set<TEntity>().Remove(old);
                Set<TEntity>().Add(entity);
            }
        }

        private IDbSetBase<TEntity> GetEntitySetCore<TEntity>() where TEntity : EntityBase<TEntity>, new()
        {
            IDbSetBase<TEntity> result = null;

            if (ENTITY_TYPES.Contains(typeof(TEntity)))
            {
                result = EntityUnitTestHelperBase<TEntity>.GetMockDataMethod.Invoke(null, new object[] { }) as IDbSetBase<TEntity>;
            }

            return result;
        }

        private PropertyInfo GetIdProperty(Type entityType)
        {
            string idPropertyName = EntityIdPropertyNameMap.ContainsKey(entityType) ? EntityIdPropertyNameMap[entityType] : null;
            return !string.IsNullOrEmpty(idPropertyName) ? entityType.GetProperty(idPropertyName) : (entityType.GetProperty("ID") ?? entityType.GetProperty("Id"));
        }
    }
}