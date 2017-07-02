using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using Core.Ef.Exceptions;
using System.Collections.ObjectModel;

namespace Core.UnitTesting.Mock
{
    public class MockDbSet<TEntity> : IDbSetBase<TEntity>, IQueryable<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        IList<TEntity> mockEntityList;

        private PropertyInfo GetIdProperty(Type entityType)
        {
            string idPropertyName = MockDbContextBase.EntityIdPropertyNameMap.ContainsKey(entityType) ? MockDbContextBase.EntityIdPropertyNameMap[entityType] : null;
            return !string.IsNullOrEmpty(idPropertyName) ? entityType.GetProperty(idPropertyName) : (entityType.GetProperty("ID") ?? entityType.GetProperty("Id"));
        }


        public Type ElementType { get { return typeof(TEntity); } }
        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }

        public IList<TEntity> MockEntityList
        {
            get { return mockEntityList; }
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
            mockEntityList = new List<TEntity>();

            Provider = mockEntityList.AsQueryable().Provider;
            Expression = mockEntityList.AsQueryable().Expression;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return mockEntityList.GetEnumerator();
        }

        public TEntity Add(TEntity entity)
        {
            Type entityType = entity.GetType();
            PropertyInfo idProperty = GetIdProperty(entityType);
            if (idProperty != null)
            {
                object key = idProperty.GetValue(entity);
                TEntity found = FindByKey(key);
                if (found != null)
                {
                    throw new DbUpdateExceptionBase();
                }
            }
            mockEntityList.Add(entity);
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
            long longId = this.Max<TEntity, long>(e => e.LongId) + 1;

            Type entityType = typeof(TEntity);
            PropertyInfo idProperty = GetIdProperty(entityType);

            foreach (TEntity entity in entities)
            {
                SetEntityKey(idProperty, entity, ref longId);
                mockEntityList.Add(entity);
            }
            return mockEntityList.ToList();
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
                    mockEntityList.Remove(entity);
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
                foreach (TEntity entity in mockEntityList)
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
            foreach(TEntity e in entities)
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
