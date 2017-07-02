using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{
    [Serializable]
    public abstract class SerializableEntity : INotifyPropertyChanged
    {
        public SerializableEntity()
        {
        }

        private object[] _propertyValues;
        internal object[] PropertyValues
        {
            get
            {
                if (_propertyValues == null)
                {
                    _propertyValues = new object[GetEntityMetaData.WritableProperties.Count];
                }

                return _propertyValues;
            }
        }

        private ObjectMetaData _entityMetaData;

        public event PropertyChangedEventHandler PropertyChanged;

        internal ObjectMetaData GetEntityMetaData
        {
            get
            {
                if (_entityMetaData == null)
                {

                    _entityMetaData = ObjectMetaData.GetEntityMetaData(this.GetType());
                }

                return _entityMetaData;
            }
        }

        public virtual T GetValue<T>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (PropertyValues[_entityMetaData.WritablePropertyNames[propertyName]] == null)
            {
                PropertyValues[_entityMetaData.WritablePropertyNames[propertyName]] = default(T);
            }

            return (T)PropertyValues[_entityMetaData.WritablePropertyNames[propertyName]];
        }

        public object GetValue(int index)
        {
            if (PropertyValues[index] == null)
            {
                PropertyValues[index] = GetEntityMetaData.DefaultValueForWritablePropertyList[index];
            }

            return PropertyValues[index];
        }

        internal object GetValue(int index, ObjectMetaData metaData)
        {
            if (PropertyValues[index] == null)
            {
                PropertyValues[index] = metaData.DefaultValueForWritablePropertyList[index];
            }

            return PropertyValues[index];
        }
        public virtual void SetValue<T>(T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyValues[_entityMetaData.WritablePropertyNames[propertyName]] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract SerializableEntity CreateInstance();
    }
}
