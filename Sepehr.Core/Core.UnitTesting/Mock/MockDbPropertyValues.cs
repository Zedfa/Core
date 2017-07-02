using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Cmn;
using System.Linq;

namespace Core.UnitTesting.Mock
{
    public class MockDbPropertyValues : IDbPropertyValuesBase
    {
        private IDictionary<string, object> _properties = new Dictionary<string, object>();

        public object this[string propertyName]
        {
            get
            {
                return _properties[propertyName];
            }

            set
            {
                _properties[propertyName] = value;
            }
        }

        public IEnumerable<string> PropertyNames
        {
            get
            {
                return _properties.Keys.AsEnumerable();
            }
        }

        public IDbPropertyValuesBase Clone()
        {
            MockDbPropertyValues clone = new MockDbPropertyValues();

            foreach (string key in _properties.Keys)
            {
                clone[key] = _properties[key];
            }

            return clone;
        }

        public TValue GetValue<TValue>(string propertyName)
        {
            return (TValue)this[propertyName];
        }

        public void SetValues(IDbPropertyValuesBase propertyValues)
        {
            foreach (string key in propertyValues.PropertyNames)
            {
                _properties[key] = propertyValues[key];
            }
        }

        public void SetValues(object obj)
        {
            SetValues(obj as IDbPropertyValuesBase);
        }

        public object ToObject()
        {
            return this;
        }
    }
}
