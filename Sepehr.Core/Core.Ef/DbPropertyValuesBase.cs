using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace Core.Ef
{
    public class DbPropertyValuesBase : IDbPropertyValuesBase
    {
        private DbPropertyValues _dbPropertyValues;
        public DbPropertyValuesBase(DbPropertyValues dbPropertyValues)
        {
            _dbPropertyValues = dbPropertyValues;
        }
        public object this[string propertyName]
        {
            get
            {
                return _dbPropertyValues[propertyName];
            }

            set
            {
                _dbPropertyValues[propertyName] = value;
            }
        }

        public IEnumerable<string> PropertyNames
        {
            get
            {
                return _dbPropertyValues.PropertyNames;
            }
        }

        public IDbPropertyValuesBase Clone()
        {
            return _dbPropertyValues.Clone() as IDbPropertyValuesBase;
        }

        public TValue GetValue<TValue>(string propertyName)
        {
            return _dbPropertyValues.GetValue<TValue>(propertyName);
        }

        public void SetValues(IDbPropertyValuesBase propertyValues)
        {
            _dbPropertyValues.SetValues(propertyValues);
        }

        public void SetValues(object obj)
        {
            _dbPropertyValues.SetValues(obj);
        }

        public object ToObject()
        {
            return _dbPropertyValues.ToObject();
        }
    }
}
