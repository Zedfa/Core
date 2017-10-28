
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
    using Exceptions;
    using DataSource;

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
                            var properties = new Dictionary<string, PropertyInfo>();
                            foreach (PropertyInfo propInfo in EntityType.GetProperties())
                            {
                                if (propInfo.Name != "CustomIndexerProperty")
                                    properties.Add(propInfo.Name, propInfo);
                            }
                            _properties = properties;
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
                            var writableMappedProperties = new Dictionary<string, PropertyInfo>();
                            WritableProperties.Where(prop => prop.Value.CanWrite).ToList().ForEach(item =>
                            {
                                var notMappedAttribute = Attribute.GetCustomAttribute(item.Value, typeof(NotMappedAttribute));
                                var fillNavigationProperyByCache = Attribute.GetCustomAttribute(item.Value, typeof(FillNavigationProperyByCacheAttribute));
                                if (notMappedAttribute == null && fillNavigationProperyByCache == null)
                                    writableMappedProperties.Add(item.Key, item.Value);
                            });

                            _writableMappedProperties = writableMappedProperties;
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
                            var writableProperties = new Dictionary<string, PropertyInfo>();
                            Properties.Where(prop => prop.Value.CanWrite).ToList().ForEach(item =>
                            {
                                writableProperties.Add(item.Key, item.Value);
                            });
                            _writableProperties = writableProperties;
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
                            var keyColumns = new Dictionary<string, PropertyInfo>();
                            Properties.ToList().ForEach(propInfo =>
                           {
                               var keyAtt = Attribute.GetCustomAttribute(propInfo.Value, typeof(KeyAttribute));

                               if (keyAtt != null)
                                   keyColumns.Add(propInfo.Value.Name, propInfo.Value);
                               else
                               {
                                   if (propInfo.Value.Name.ToLower().Equals("id"))
                                       keyColumns.Add(propInfo.Value.Name, propInfo.Value);
                               }
                           });
                            _keyColumns = keyColumns;
                        }
                    }
                }

                return _keyColumns;
            }
        }

        private Dictionary<string, int> _indexableProps;
        private List<IndexablePropertyData> _indexableGroupedItemList;
        private List<GroupedIndexablePropertyData> _indexableGroupedProps;
        public Dictionary<string, int> IndexableProps
        {
            get
            {
                if (_indexableProps == null)
                {
                    RetrieveIndexablePropertyData();
                }

                return _indexableProps;
            }
        }
        public List<GroupedIndexablePropertyData> IndexableGroupedProps
        {
            get
            {
                if (_indexableGroupedProps == null)
                {
                    lock (this)
                    {
                        if (_indexableGroupedProps == null)
                        {
                            RetrieveIndexablePropertyData();
                            var grouped = _indexableGroupedItemList.GroupBy(item => item.GroupName);
                            List<GroupedIndexablePropertyData> result = new List<GroupedIndexablePropertyData>();
                            grouped.ToList().Select(g => g.OrderBy(item => item.IndexOrder).ToList()).ToList().ForEach(g => result.Add(new GroupedIndexablePropertyData(g.First().GroupName, g.First().NavigationPath, result.Count, g.ToList())));
                            _indexableGroupedProps = result;
                        }
                    }
                }

                return _indexableGroupedProps;
            }
        }

        private void RetrieveIndexablePropertyData()
        {
            lock (this)
            {
                if (_indexableProps == null)
                {
                    var indexableProps = new Dictionary<string, int>();
                    var indexableGroupedItemList = new List<IndexablePropertyData>();
                    Properties.ToList().ForEach(propInfo =>
                    {
                        List<IndexablePropertyAttribute> indexableAttList = Attribute.GetCustomAttributes(propInfo.Value, typeof(IndexablePropertyAttribute)).Cast<IndexablePropertyAttribute>().ToList();
                        if (indexableAttList != null && indexableAttList.Count > 0)
                        {
                            indexableAttList.ForEach((att) =>
                            {
                                if (!string.IsNullOrEmpty(att.GroupName))
                                {
                                    if (att.IndexOrder == -1)
                                        throw new InvalidIndexablePropertyDataException(propInfo.Value.DeclaringType.Name, propInfo.Value.Name);
                                    indexableGroupedItemList.Add(new IndexablePropertyData(att.GroupName, propInfo.Value.Name, att.IndexOrder));
                                }
                                else
                                    indexableProps.Add(propInfo.Value.Name, indexableProps.Count);
                            });
                        }
                        else
                        {
                            List<IndexableNavigationPropertyAttribute> indexableNavigationAttList = Attribute.GetCustomAttributes(propInfo.Value, typeof(IndexableNavigationPropertyAttribute)).Cast<IndexableNavigationPropertyAttribute>().ToList();
                            if (indexableNavigationAttList != null && indexableNavigationAttList.Count() > 0)
                            {
                                indexableNavigationAttList.ForEach((att) =>
                                {
                                    if (!string.IsNullOrEmpty(att.GroupName))
                                    {
                                        if (att.IndexOrder == -1)
                                            throw new InvalidIndexablePropertyDataException(propInfo.Value.DeclaringType.Name, propInfo.Value.Name);
                                        indexableGroupedItemList.Add(new IndexablePropertyData(att.GroupName, att.NavigationPath, att.IndexOrder));
                                    }
                                    else
                                        indexableProps.Add(att.NavigationPath, indexableProps.Count);
                                });
                            }
                        }
                    });

                    _indexableProps = indexableProps;
                    _indexableGroupedItemList = indexableGroupedItemList;
                }
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
                            var infoForFillingNavigationPropertyDic = new Dictionary<string, InfoForFillingNavigationProperty>();
                            Properties.ToList().ForEach(propInfo =>
                            {
                                var fillNavigationProperyByCacheAttribute =
                              Attribute.GetCustomAttribute(propInfo.Value, typeof(FillNavigationProperyByCacheAttribute)) as
                                  FillNavigationProperyByCacheAttribute;
                                if (fillNavigationProperyByCacheAttribute != null)
                                {
                                    infoForFillingNavigationPropertyDic.Add(propInfo.Value.Name,
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

                            _infoForFillingNavigationPropertyDic = infoForFillingNavigationPropertyDic;
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
                    EntityInfo.EntityInfoDic[type] = GetEntityInfo(type);
                }
            });
        }
        private static object _lockObjectForEntityInfo = new object();
        public static EntityInfo GetEntityInfo(Type entityType)
        {
            EntityInfo entityInfo = null;
            if (!Core.Cmn.EntityInfo.EntityInfoDic.TryGetValue(entityType, out entityInfo))
            {
                lock (_lockObjectForEntityInfo)
                {
                    if (!Core.Cmn.EntityInfo.EntityInfoDic.TryGetValue(entityType, out entityInfo))
                    {
                        entityInfo = new EntityInfo(entityType);
                        Core.Cmn.EntityInfo.EntityInfoDic[entityType] = entityInfo;
                    }
                }
            }

            return entityInfo;
        }
    }
}
