using Core.Cmn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.UnitTesting.Mock
{
    public class MockDbSet<TEntity> : IDbSetBase<TEntity>, IQueryable<TEntity> where TEntity : ObjectBase, new()
    {
        public IList<TEntity> MockEntityList { get; set; }

        private PropertyInfo GetIdProperty(Type entityType)
        {
            string idPropertyName = MockDbContextBase.EntityIdPropertyNameMap.ContainsKey(entityType) ? MockDbContextBase.EntityIdPropertyNameMap[entityType] : null;
            return !string.IsNullOrEmpty(idPropertyName) ? entityType.GetProperty(idPropertyName) : (entityType.GetProperty("ID") ?? entityType.GetProperty("Id"));
        }

        public Type ElementType { get { return typeof(TEntity); } }

        public Expression Expression
        {
            get
            {
                return MockEntityList.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return MockEntityList.AsQueryable().Provider;
            }
        }

        public ObservableCollection<TEntity> Local
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MockDbSet()
        {
            MockEntityList = new ObservableCollection<TEntity>();
            ((ObservableCollection<TEntity>)MockEntityList).CollectionChanged += MockDbSet_CollectionChanged;
        }

        private void MockDbSet_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return MockEntityList.GetEnumerator();
        }

        public TEntity Add(TEntity entity)
        {
            Type entityType = entity.GetType();
            PropertyInfo idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                object key = idProperty.GetValue(entity);
                TEntity found = FindByKey(key);
                //if (found != null)
                //{
                //    throw new DbUpdateExceptionBase();
                //}
            }
            MockEntityList.Add(entity);
            return entity;
        }

        private void SetEntityKey(PropertyInfo keyProperty, TEntity entity, ref long longId)
        {
            if (keyProperty != null)
            {
                if (keyProperty.PropertyType == typeof(int))
                {
                    keyProperty.SetValue(entity, (int)longId);
                    longId++;
                }
                else if (keyProperty.PropertyType == typeof(long))
                {
                    keyProperty.SetValue(entity, longId);
                    longId++;
                }
                else if (keyProperty.PropertyType == typeof(Guid))
                {
                    keyProperty.SetValue(entity, Guid.NewGuid());
                }
            }
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            long longId = this.Max<TEntity, long>(e => e.CacheId) + 1;

            Type entityType = typeof(TEntity);
            PropertyInfo idProperty = GetIdProperty(entityType);

            foreach (TEntity entity in entities)
            {
                SetEntityKey(idProperty, entity, ref longId);
                MockEntityList.Add(entity);
            }
            return MockEntityList.ToList();
        }

        public TEntity Create()
        {
            return new TEntity();
        }

        public TEntity Remove(TEntity entity)
        {
            Type entityType = entity.GetType();
            PropertyInfo idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                object key = idProperty.GetValue(entity);
                TEntity found = FindByKey(key);
                if (found != null)
                {
                    MockEntityList.Remove(entity);
                }
            }
            return entity;
        }

        public TEntity Find(params object[] keyValues)
        {
            TEntity found = null;

            if (keyValues == null || keyValues.Length != 1)
                found = null;

            found = FindByKey(keyValues[0]);
            return found;
        }

        private TEntity FindByKey(object key)
        {
            TEntity found = null;
            Type entityType = typeof(TEntity);
            PropertyInfo idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                foreach (TEntity entity in MockEntityList)
                {
                    if (CheckKey(idProperty, key, entity))
                    {
                        found = entity;
                        break;
                    }
                }
            }

            return found;
        }

        //public override TEntity Find(params object[] keyValues)
        //{
        //    if (keyValues == null || keyValues.Length != 1)
        //        return null;

        //    return mockEntityList.SingleOrDefault(e=>e.LongId == (int)keyValues[0]);
        //}

        private bool CheckEntityKey<TKeyType>(object key, PropertyInfo keyProperty, object entity)
        {
            bool result = false;
            TKeyType entityId = (TKeyType)key;
            TKeyType id = (TKeyType)keyProperty.GetValue(entity);
            if (entityId.Equals(id))
            {
                result = true;
            }

            return result;
        }

        private bool CheckKey(PropertyInfo keyProperty, object key, object entity)
        {
            bool result = false;

            if (keyProperty.PropertyType == typeof(int))
            {
                result = CheckEntityKey<int>(key, keyProperty, entity);
            }
            else if (keyProperty.PropertyType == typeof(long))
            {
                result = CheckEntityKey<long>(key, keyProperty, entity);
            }
            else if (keyProperty.PropertyType == typeof(Guid))
            {
                result = CheckEntityKey<Guid>(key, keyProperty, entity);
            }

            return result;
        }

        public TEntity Attach(TEntity entity)
        {
            return entity;
        }

        TDerivedEntity IDbSetBase<TEntity>.Create<TDerivedEntity>()
        {
            return new TDerivedEntity();
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            return this.AsQueryable<TEntity>();
        }

        public int Delete(IQueryable<TEntity> entititesPredicateToRemove)
        {
            foreach (TEntity e in entititesPredicateToRemove)
            {
                Remove(e);
            }

            return entititesPredicateToRemove.Count();
        }

        public int Update(IQueryable<TEntity> source, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return 0;
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity e in entities)
            {
                Remove(e);
            }

            return MockEntityList.AsEnumerable<TEntity>();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return MockEntityList.AsQueryable<TEntity>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.MockEntityList.GetEnumerator();
        }
    }
}