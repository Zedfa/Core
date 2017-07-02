using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Cmn;
using System.Reflection;

namespace Core.UnitTesting.Mock
{
    public class MockDbEntityEntry<TEntity> : IDbEntityEntryBase<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        private MockDbPropertyValues _originalPropertyValues;
        private MockDbPropertyValues _currentPropertyValues;
        public IDbPropertyValuesBase CurrentValues
        {
            get
            {
                return _currentPropertyValues;
            }
        }

        private TEntity _entity;
        public object Entity
        {
            get
            {
                return _entity;
            }
        }

        public IDbPropertyValuesBase OriginalValues
        {
            get
            {
                return _originalPropertyValues;
            }
        }

        public EntityState State
        {
            get;set;
        }

        TEntity IDbEntityEntryBase<TEntity>.Entity
        {
            get
            {
                return _entity;
            }
        }

        public MockDbEntityEntry(TEntity entity)
        {
            _entity = entity;

            _originalPropertyValues = new MockDbPropertyValues();
            _currentPropertyValues = new MockDbPropertyValues();
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public);
            foreach(PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(_entity);
                _originalPropertyValues[propertyInfo.Name] = value;
                _currentPropertyValues[propertyInfo.Name] = value;
            }
        }
    }
}
