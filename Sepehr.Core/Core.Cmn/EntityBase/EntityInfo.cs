
///chegini 1391/04/24
namespace Core.Cmn
{
    using Core.Cmn.Cache;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Cmn.Extensions;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using Attributes;
    using EntityBase;

    public class EntityInfo
    {
        static EntityInfo()
        {
            EntityInfoDic = new Dictionary<Type, Cmn.EntityInfo>();
        }
        internal static Dictionary<Type, EntityInfo> EntityInfoDic { get; set; }
        public EntityInfo(Type entityType)
        {
            EntityType = entityType;
        }

        public Type EntityType { get; private set; }
        private Dictionary<string, PropertyInfo> _properties;
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
                            foreach (PropertyInfo propInfo in EntityType.GetProperties())
                            {
                                if (propInfo.Name != "CustomIndexerProperty")
                                    _properties.Add(propInfo.Name, propInfo);
                            }
                        }
                    }
                }

                return _properties;
            }
        }
        private Dictionary<string, PropertyInfo> _writableMappedProperties;
        private Dictionary<string, PropertyInfo> _writableProperties;
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
                            WritableProperties.Where(prop => prop.Value.CanWrite).ToList().ForEach(item =>
                            {
                                var notMappedAttribute = Attribute.GetCustomAttribute(item.Value, typeof(NotMappedAttribute));
                                var fillNavigationProperyByCache = Attribute.GetCustomAttribute(item.Value, typeof(FillNavigationProperyByCacheAttribute));
                                if (notMappedAttribute == null && fillNavigationProperyByCache == null)
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
        private Dictionary<string, PropertyInfo> _keyColumns;
        public Dictionary<string, PropertyInfo> KeyColumns
        {
            get
            {
                if (_keyColumns == null)
                {
                    lock (this)
                    {
                        if (_keyColumns == null)
                        {
                            _keyColumns = new Dictionary<string, PropertyInfo>();
                            Properties.ToList().ForEach(propInfo =>
                           {
                               var keyAtt = Attribute.GetCustomAttribute(propInfo.Value, typeof(KeyAttribute));

                               if (keyAtt != null)
                                   _keyColumns.Add(propInfo.Value.Name, propInfo.Value);
                               else
                               {
                                   if (propInfo.Value.Name.ToLower().Equals("id"))
                                       _keyColumns.Add(propInfo.Value.Name, propInfo.Value);
                               }
                           });
                        }
                    }
                }

                return _keyColumns;
            }
        }
        ConcurrentBag<ConcurrentBag<_EntityBase>> _allNavigationPropertyDataList;
        internal ConcurrentBag<ConcurrentBag<_EntityBase>> AllNavigationPropertyDataList
        {
            get
            {
                if (_allNavigationPropertyDataList == null)
                {
                    _allNavigationPropertyDataList = new ConcurrentBag<ConcurrentBag<_EntityBase>>();
                }
                return _allNavigationPropertyDataList;
            }
        }
        private Dictionary<string, Cache.InfoForFillingNavigationProperty> _infoForFillingNavigationPropertyDic;
        public Dictionary<string, Cache.InfoForFillingNavigationProperty> InfoForFillingNavigationPropertyDic
        {
            get
            {
                if (_infoForFillingNavigationPropertyDic == null)
                {
                    lock (this)
                    {
                        if (_infoForFillingNavigationPropertyDic == null)
                        {
                            _infoForFillingNavigationPropertyDic = new Dictionary<string, InfoForFillingNavigationProperty>();
                            Properties.ToList().ForEach(propInfo =>
                            {
                                var fillNavigationProperyByCacheAttribute =
                              Attribute.GetCustomAttribute(propInfo.Value, typeof(FillNavigationProperyByCacheAttribute)) as
                                  FillNavigationProperyByCacheAttribute;
                                if (fillNavigationProperyByCacheAttribute != null)
                                {
                                    _infoForFillingNavigationPropertyDic.Add(propInfo.Value.Name,
                                        new Cache.InfoForFillingNavigationProperty
                                        {
                                            PropertyInfo = propInfo.Value,
                                            CacheName = fillNavigationProperyByCacheAttribute.CacheName,
                                            OtherEntityRefrencePropertyName =
                                                fillNavigationProperyByCacheAttribute.OtherEntityRefrencePropertyName,
                                            ThisEntityRefrencePropertyName =
                                                fillNavigationProperyByCacheAttribute.ThisEntityRefrencePropertyName,
                                            ParentEntityType = EntityType,
                                            NavigationPropertyType = propInfo.Value.PropertyType,
                                            SecondLevelDataSourceName =
                                                fillNavigationProperyByCacheAttribute.SecondLevelDataSourceName,
                                        });
                                }
                            });
                        }
                    }
                }

                return _infoForFillingNavigationPropertyDic;
            }
        }
        public static void BuildEntityInfoDic(IList<Type> allTypes)
        {
            EntityInfo.EntityInfoDic = new Dictionary<Type, EntityInfo>();
            Type entityBaseType = typeof(_EntityBase);
            List<Type> allTypeOfEntities = allTypes.Where(type => entityBaseType.IsAssignableFrom(type)).ToList();
            allTypeOfEntities.ForEach(type =>
            {
                var typeKey = type.GetHashCode();
                if (EntityInfo.EntityInfoDic.ContainsKey(type))
                {
                    throw new Exception($"The key for entity type {type.Name} is duplicate!");
                }
                else
                {
                    EntityInfo.EntityInfoDic[type] = _EntityBase.GetEntityInfo(type);
                }
            });
        }
    }
}
