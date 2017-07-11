namespace Core.Serialization
{
    using Core.Serialization.BinaryConverters;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class ObjectMetaData
    {
        private static object _lockObjectForEntityInfo = new object();

        private int? _countOfWritableProperties;

        private List<object> _defaultValueForWritablePropertyList;

        private bool[] _isSerializablePropertyByIndexList;

        private Dictionary<string, PropertyInfo> _properties;

        private BinaryConverterBase[] _serializeItemList;

        private Dictionary<string, PropertyInfo> _writableMappedProperties;

        private Dictionary<string, PropertyInfo> _writableProperties;

        private List<PropertyInfo> _writablePropertyList;

        private Dictionary<string, int> _writablePropertyNames;

        static ObjectMetaData()
        {
            ObjectMetaData.EntityMetaDataDic = new ConcurrentDictionary<Type, Core.Serialization.ObjectMetaData>();
        }

        public ObjectMetaData(Type objectType)
        {
            ObjectType = objectType;
            ReflectionEmitPropertyAccessor = new Serialization.ReflectionEmitPropertyAccessor(objectType, this);
        }

        public BinaryConverterBase[] BinaryConverterList
        {
            get
            {
                if (_serializeItemList == null)
                {
                    lock (Properties)
                    {
                        if (_serializeItemList == null)
                        {
                            int count = CountOfWritableProperties;
                            _serializeItemList = new BinaryConverterBase[count];
                            var writableProperties = WritableProperties.ToList();
                            for (int i = 0; i < count; i++)
                            {
                                if (IsSerializablePropertyByIndexList[i])
                                    _serializeItemList[i] = BinaryConverterBase.GetBinaryConverter(writableProperties[i].Value.PropertyType);
                            };
                        }
                    }
                }
                return _serializeItemList;
            }
        }

        public int CountOfWritableProperties
        {
            get
            {
                if (_countOfWritableProperties == null)
                {
                    _countOfWritableProperties = WritableProperties.Count();
                }

                return _countOfWritableProperties.Value;
            }
        }

        public List<object> DefaultValueForWritablePropertyList
        {
            get
            {
                if (_defaultValueForWritablePropertyList == null)
                {
                    lock (Properties)
                    {
                        if (_defaultValueForWritablePropertyList == null)
                        {
                            _defaultValueForWritablePropertyList = new List<object>();
                            WritableProperties.ToList().ForEach(item =>
                            {
                                _defaultValueForWritablePropertyList.Add(item.Value.PropertyType.GetDefaultValue());
                            });
                        }
                    }
                }

                return _defaultValueForWritablePropertyList;
            }
        }

        public bool[] IsSerializablePropertyByIndexList
        {
            get
            {
                if (_isSerializablePropertyByIndexList == null)
                {
                    lock (Properties)
                    {
                        if (_isSerializablePropertyByIndexList == null)
                        {
                            int count = CountOfWritableProperties;
                            _isSerializablePropertyByIndexList = new bool[count];
                            var hasDataContractAtt = Attribute.IsDefined(ObjectType, typeof(DataContractAttribute));
                            var writableProperties = WritableProperties.ToList();

                            for (int i = 0; i < count; i++)
                            {
                                if (hasDataContractAtt)
                                {
                                    var dataMemberAttribute = Attribute.GetCustomAttribute(writableProperties[i].Value, typeof(DataMemberAttribute));
                                    _isSerializablePropertyByIndexList[i] = dataMemberAttribute != null;
                                }
                                else
                                {
                                    _isSerializablePropertyByIndexList[i] = true;
                                }
                            };
                        }
                    }
                }

                return _isSerializablePropertyByIndexList;
            }
        }

        //        propInfo.SetValue(entity, value);
        //        if (!value.Equals(entity.PropertyValues[i]) || !value.Equals(propInfo.GetValue(entity)))
        //            throw new BadPropertyImplementationException(entity.GetEntityMetaData.WritablePropertyList[i].Name, entity.GetEntityMetaData.ObjectType.Name);
        //    }
        //}
        public Type ObjectType { get; private set; }

        //private static void ValidateImplementationProperties(Type entityType)
        //{
        //    var entity = Activator.CreateInstance(entityType) as SerializableEntity;
        //    for (var i = 0; i < entity.GetEntityMetaData.WritablePropertyList.Count; i++)
        //    {
        //        object value;
        //        var deb = entity.GetEntityMetaData.WritablePropertyList[i].Name.Contains("Null");
        //        var propInfo = entity.GetEntityMetaData.WritablePropertyList[i];
        //        if (propInfo.PropertyType.IsSimple())
        //        {
        //            if (propInfo.PropertyType.IsGenericType)
        //            {
        //                var type = propInfo.PropertyType.GetGenericArguments().First();
        //                value = Activator.CreateInstance(type);
        //            }
        //            else
        //            {
        //                if (propInfo.PropertyType == typeof(string))
        //                    value = string.Empty;
        //                else
        //                    value = Activator.CreateInstance(propInfo.PropertyType);
        //            }
        //        }
        //        else
        //        {
        //            if (typeof(Array).IsAssignableFrom(propInfo.PropertyType))
        //                value = Activator.CreateInstance(propInfo.PropertyType, 1);
        //            else
        //                value = Activator.CreateInstance(propInfo.PropertyType);
        //        }
        public Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                if (_properties == null)
                {
                    lock (this)
                    {
                        if (_properties == null)
                        {
                            _properties = new Dictionary<string, PropertyInfo>();
                            foreach (PropertyInfo propInfo in
                                ObjectType.GetProperties()
                                .Where(prop => prop.GetIndexParameters().Count() == 0)
                                .OrderBy(prop => prop.Name).ToList())
                            {
                                _properties.Add(propInfo.Name, propInfo);
                            }
                        }
                    }
                }

                return _properties;
            }
        }

        public ReflectionEmitPropertyAccessor ReflectionEmitPropertyAccessor { get; private set; }
        public Dictionary<string, PropertyInfo> WritableMappedProperties
        {
            get
            {
                if (_writableMappedProperties == null)
                {
                    lock (Properties)
                    {
                        if (_writableMappedProperties == null)
                        {
                            _writableMappedProperties = new Dictionary<string, PropertyInfo>();
                            WritableProperties.ToList().ForEach(item =>
                            {
                                var notMappedAttribute = Attribute.GetCustomAttribute(item.Value, typeof(NotMappedAttribute));
                                if (notMappedAttribute == null)
                                    _writableMappedProperties.Add(item.Key, item.Value);
                            });
                        }
                    }
                }

                return _writableMappedProperties;
            }
        }

        public Dictionary<string, PropertyInfo> WritableProperties
        {
            get
            {
                if (_writableProperties == null)
                {
                    lock (Properties)
                    {
                        if (_writableProperties == null)
                        {
                            _writableProperties = new Dictionary<string, PropertyInfo>();
                            Properties.Where(prop => prop.Value.CanWrite).ToList().ForEach(item =>
                              {
                                  _writableProperties.Add(item.Key, item.Value);
                              });
                        }
                    }
                }

                return _writableProperties;
            }
        }

        public List<PropertyInfo> WritablePropertyList
        {
            get
            {
                if (_writablePropertyList == null)
                {
                    lock (Properties)
                    {
                        if (_writablePropertyList == null)
                        {
                            _writablePropertyList = new List<PropertyInfo>();
                            WritableProperties.ToList().ForEach(item =>
                            {
                                _writablePropertyList.Add(item.Value);
                            });
                        }
                    }
                }

                return _writablePropertyList;
            }
        }

        public Dictionary<string, int> WritablePropertyNames
        {
            get
            {
                if (_writablePropertyNames == null)
                {
                    lock (Properties)
                    {
                        if (_writablePropertyNames == null)
                        {
                            _writablePropertyNames = new Dictionary<string, int>();
                            var i = 0;
                            WritableProperties.ToList().ForEach(item =>
                          {
                              _writablePropertyNames.Add(item.Key, i);
                              i++;
                          });
                        }
                    }
                }

                return _writablePropertyNames;
            }
        }

        private static ConcurrentDictionary<Type, ObjectMetaData> EntityMetaDataDic { get; set; }
        public static ObjectMetaData GetEntityMetaData(Type entityType)
        {
            ObjectMetaData entityInfo = null;
            if (!ObjectMetaData.EntityMetaDataDic.TryGetValue(entityType, out entityInfo))
            {
                lock (EntityMetaDataDic)
                {
                    if (!ObjectMetaData.EntityMetaDataDic.TryGetValue(entityType, out entityInfo))
                    {
                        entityInfo = new ObjectMetaData(entityType);
                        ObjectMetaData.EntityMetaDataDic[entityType] = entityInfo;
                    }
                }
            }

            return entityInfo;
        }
    }
}