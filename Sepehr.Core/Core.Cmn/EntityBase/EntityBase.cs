

namespace Core.Cmn
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using System.Web.Script.Serialization;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Core.Cmn.Attributes;
    using Core.Cmn.Cache;
    using System.Linq.Dynamic;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Collections;
    using System.Collections.Concurrent;
    using Serialization;

    [Serializable]
    [DataContract(IsReference = true)]
    public  class _EntityBase : SerializableEntity
    {
        private ConcurrentDictionary<string, object> _navigationPropertyDataDic;
        public ConcurrentDictionary<string, object> NavigationPropertyDataDic => _navigationPropertyDataDic ??
                                                                                (_navigationPropertyDataDic = new ConcurrentDictionary<string, object>());        
        /// <summary>
        /// It just works right for Single Pirimarykey, e.g: Id, not Id, name as PrimaryKey.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.EntityInfo().KeyColumns.Count > 0 ?
                this.EntityInfo().KeyColumns.Values.First().GetValue(this).GetHashCode() :
                base.GetHashCode();
        }

        protected _EntityBase()
        {
        }
        public virtual void UpdateAllProps(_EntityBase entity)
        {
            var props = this.EntityInfo().WritableMappedProperties;
            foreach (var prop in props)
            {
                this[prop.Key] = prop.Value.GetValue(entity);
            }
        }
        public virtual void CallPropertyChangedByCache(IList<_EntityBase> senders, string propertyName)
        {

        }

        private UInt64 _timeStampStr;
        [NotMapped]
        //[NonSerialized]
        [IgnoreDataMember]
        [ScriptIgnore]
        [JsonIgnore]
        public virtual UInt64 TimeStampUnit
        {
            get
            {
                if (TimeStamp == null)
                    return 0;
                if (_timeStampStr == 0)
                {
                    if (TimeStamp.Count() == 8)
                        _timeStampStr = BitConverter.ToUInt64(TimeStamp.Reverse().ToArray(), 0);
                    else
                        //Todo:The serializer set TimeStamp property by a zero byte so we had a exception to TimeStamp.Reverse() at above , we fixed it temporary ...
                        return 0;
                }

                return _timeStampStr;
            }
        }

        private long _longId;
        [NotMapped]
        //[NonSerialized]
        [IgnoreDataMember]
        [ScriptIgnore]
        [JsonIgnore]
        public virtual long LongId
        {
            get
            {
                if (_longId == default(long))
                {
                    var keyColumnName = EntityInfo().KeyColumns.Keys.FirstOrDefault();
                    if (keyColumnName != null)
                    {
                        var id = this[keyColumnName];
                        long.TryParse(id.ToString(), out _longId);
                    }
                }
                return _longId;
            }
        }

        private bool _enableFillNavigationProperyByCache;
        [NotMapped]
        public bool IsEnableFillNavigationProperyByCache => _enableFillNavigationProperyByCache;

        public void EnableFillNavigationProperyByCache()
        {
            _enableFillNavigationProperyByCache = true;
        }

        private EntityInfo _entityInfo;
        public EntityInfo EntityInfo()
        {
            if (_entityInfo == null)
            {
                _entityInfo = _EntityBase.GetEntityInfo(this.GetType());
            }

            return _entityInfo;
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

        [IndexerName("CustomIndexerProperty")]
        [IgnoreDataMember]
        public object this[string propertyName]
        {
            get
            {
                //SetEntityInfo();
                if (EntityInfo().Properties.ContainsKey(propertyName))
                {
                    return EntityInfo().Properties[propertyName].GetValue(this); ;
                }
                else
                    throw new ArgumentException("PropertyName Is not exist!");
            }
            set
            {
                if (EntityInfo().Properties.ContainsKey(propertyName))
                {
                    var propValue = this[propertyName];
                    if (
                        (propValue != null && !propValue.Equals(value))
                        ||
                        (propValue == null && value != null)
                        )
                    {
                        OnColumnChanging(propertyName, ref value);
                        EntityInfo().Properties[propertyName].SetValue(this, value);
                    }
                }
                else
                    throw new ArgumentException("PropertyName Is not exist!");
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(_EntityBase)) return false;
            return Equals((_EntityBase)obj);
        }

        protected ICollection<T> GetNavigationPropertyDataListFromCache<T>(string navigationPropertyName) where T : IEntity
        {
            ICollection<T> result = null;
            if (IsEnableFillNavigationProperyByCache)
            {
                if (!NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                {
                    object tmp = null;
                    IEnumerable<T> result1 = GetNavigationPropertyDataAtFirstTime<T>(navigationPropertyName, out tmp);
                    if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                    {
                        result = NavigationPropertyDataDic[navigationPropertyName] as List<T>;
                    }
                }
                else
                    result = NavigationPropertyDataDic[navigationPropertyName] as List<T>;
            }
            else
            {
                if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName + 1))
                    result = NavigationPropertyDataDic[navigationPropertyName + 1] as ICollection<T>;
            }

            return result;
        }

        private IEnumerable<T> GetNavigationPropertyDataAtFirstTime<T>(string navigationPropertyName, out object resultForSecondLevelDataSource) where T : IEntity
        {
            resultForSecondLevelDataSource = null;
            var infoForFillingNavigationProperty = EntityInfo().InfoForFillingNavigationPropertyDic[navigationPropertyName];

            InitForFillingNavigationProperty_JustOneTime(infoForFillingNavigationProperty);


            IEnumerable<T> result = null;
            if (!NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
            {
                var thisPropertyValue = this[infoForFillingNavigationProperty.ThisEntityRefrencePropertyName];
                var propertyType = EntityInfo().Properties[infoForFillingNavigationProperty.ThisEntityRefrencePropertyName].PropertyType;
                var cacheInfo = CacheConfig.CacheInfoDic.First(ci => ci.Value.Name == infoForFillingNavigationProperty.CacheName).Value;
                if (thisPropertyValue != Activator.CreateInstance(propertyType))
                {
                    lock (cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic)
                    {
                        if (!NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                        {

                            if (thisPropertyValue != Activator.CreateInstance(propertyType))
                            {

                                if (cacheInfo.DisableCache)
                                {
                                    _EntityBase resultItem;
                                    result = (cacheInfo.MethodInfo.Invoke(null, new object[] { cacheInfo.Repository.GetQueryableForCahce() }) as IQueryable<T>)
                                            .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue));
                                    resultItem = result.FirstOrDefault() as _EntityBase;
                                    if (resultItem != null)
                                        resultItem.EnableFillNavigationProperyByCache();

                                }
                                else
                                {
                                    var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
                                    //  _stopwatch.Restart();
                                    result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheInfo.BasicKey.ToString(), true).
                                        Where(item =>
                                            {
                                                var otherPropertyValue = item[infoForFillingNavigationProperty.OtherEntityRefrencePropertyName];
                                                return otherPropertyValue != null && otherPropertyValue.Equals(thisPropertyValue);
                                            });
                                    // result.ToList().ForEach(item => (item as _EntityBase).EnableFillNavigationProperyByCache = true);
                                    // _stopwatch.Stop();
                                    // cacheInfo.UsingTime += TimeSpan.FromTicks(_stopwatch.ElapsedTicks);
                                }

                                if (!infoForFillingNavigationProperty.IsEnumerable)
                                {


                                    if (result.Count() == 0 && !string.IsNullOrEmpty(infoForFillingNavigationProperty.SecondLevelDataSourceName))
                                    {
                                        var dataSourceInfo = CacheConfig.CacheInfoDic.First(ci => ci.Value.Name == infoForFillingNavigationProperty.SecondLevelDataSourceName).Value;
                                        resultForSecondLevelDataSource = ((dataSourceInfo.MethodInfo.Invoke(null, new object[] { dataSourceInfo.Repository.GetQueryableForCahce() }) as IQueryable)
                                           .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue)) as IEnumerable).Cast<IEntity>().FirstOrDefault();
                                        if (resultForSecondLevelDataSource != null)
                                            ((_EntityBase)resultForSecondLevelDataSource).EnableFillNavigationProperyByCache();
                                    }

                                    _EntityBase resultItem;
                                    if (!cacheInfo.DisableCache && result.Count() == 0)
                                    {
                                        result = (cacheInfo.MethodInfo.Invoke(null, new object[] { cacheInfo.Repository.GetQueryableForCahce() }) as IQueryable<T>)
                                            .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue));
                                        resultItem = result.FirstOrDefault() as _EntityBase;
                                        if (resultItem != null)
                                            resultItem.EnableFillNavigationProperyByCache();
                                        //item => item[infoForFillingNavigationProperty.OtherEntityRefrencePropertyName].Equals(thisPropertyValue));
                                    }
                                }
                            }
                            else
                            {
                                result = new List<T>();
                            }

                            if (infoForFillingNavigationProperty.IsEnumerable)
                            {

                                NavigationPropertyDataDic[navigationPropertyName] = result.ToList<T>();
                            }
                            else
                            {
                                if (resultForSecondLevelDataSource == null)
                                {
                                    if (result != null)
                                    {
                                        NavigationPropertyDataDic[navigationPropertyName] = result.FirstOrDefault();
                                    }
                                }
                                else
                                    NavigationPropertyDataDic[navigationPropertyName] = resultForSecondLevelDataSource;

                            }
                        }
                        else
                        {
                            result = NavigationPropertyDataDic[navigationPropertyName] as List<T>;
                        }

                        var entities = cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty];
                        lock (entities)
                        {
                            cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty].Add(this);
                        }
                    }
                }
                else
                {
                    result = new List<T>();
                }
            }

            else
            {
                result = NavigationPropertyDataDic[navigationPropertyName] as List<T>;

            }

            return result;
        }

        private void InitForFillingNavigationProperty_JustOneTime(InfoForFillingNavigationProperty infoForFillingNavigationProperty)
        {
            if (!infoForFillingNavigationProperty.IsInitForFillingNavigationProperty)
            {
                var cacheInfo = CacheConfig.CacheInfoDic.First(ci => ci.Value.Name == infoForFillingNavigationProperty.CacheName).Value;
                lock (cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic)
                {
                    if (!infoForFillingNavigationProperty.IsInitForFillingNavigationProperty)
                    {
                        cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty] = new ConcurrentBag<_EntityBase>();
                        EntityInfo().AllNavigationPropertyDataList.Add(cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty]);
                        infoForFillingNavigationProperty.IsInitForFillingNavigationProperty = true;
                    }
                }
            }
        }

        protected void SetNavigationPropertyDataList(string navigationPropertyName, object value)
        {
            NavigationPropertyDataDic[navigationPropertyName + 1] = value;
        }


        protected T GetNavigationPropertyDataItemFromCache<T>(string navigationPropertyName) where T : IEntity
        {
            T result = default(T);
            if (IsEnableFillNavigationProperyByCache)
            {
                if (!NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                {
                    object resultFromSecondLevelCache = null;
                    IEnumerable<T> result1 = GetNavigationPropertyDataAtFirstTime<T>(navigationPropertyName, out resultFromSecondLevelCache);
                    if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                        result = (T)NavigationPropertyDataDic[navigationPropertyName];
                }
                else
                {

                    //System.Diagnostics.Debug.WriteLine(NavigationPropertyDataDic[navigationPropertyName]);
                    result = (T)NavigationPropertyDataDic[navigationPropertyName];
                }
            }
            else
            {
                if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName + 1))
                {
                    result = (T)NavigationPropertyDataDic[navigationPropertyName + 1];
                }
            }

            return result;
        }

        protected IT GetNavigationPropertyDataItemFromCache<T, IT>(string navigationPropertyName) where T : IEntity
        {

            IT result = default(IT);
            if (IsEnableFillNavigationProperyByCache)
            {
                if (!NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                {
                    object resultFromSecondLevelCache = null;
                    IEnumerable<T> result1 = GetNavigationPropertyDataAtFirstTime<T>(navigationPropertyName, out resultFromSecondLevelCache);
                    if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                        result = (IT)NavigationPropertyDataDic[navigationPropertyName];
                }
                else
                {
                    if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName))
                        result = (IT)NavigationPropertyDataDic[navigationPropertyName];
                }
            }
            else
            {
                if (NavigationPropertyDataDic.ContainsKey(navigationPropertyName + 1))
                {
                    result = (IT)NavigationPropertyDataDic[navigationPropertyName + 1];
                }
            }

            return result;
        }

        public bool Equals(_EntityBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            bool result = true;
            foreach (var key in this.EntityInfo().KeyColumns.Values)
            {

                if (!key.GetValue(other).Equals(key.GetValue(this)))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public virtual void OnColumnChanging(string columnName, ref object value)
        {
            if (NavigationPropertyDataDic.Count > 0 && EntityInfo().InfoForFillingNavigationPropertyDic.Count > 0)
            {
                var infoForFillingNavigationPropertyList = EntityInfo().InfoForFillingNavigationPropertyDic.Where(item => item.Value.ThisEntityRefrencePropertyName == columnName).ToList();
                foreach (var infoForFillingNavigationProperty in infoForFillingNavigationPropertyList)
                {
                    object tmp;
                    NavigationPropertyDataDic.TryRemove(infoForFillingNavigationProperty.Key, out tmp);
                }
            }
        }

        public void CallNavigationPropertyChangedByCache(_EntityBase sender, string propertyName)
        {
            var propertyChangedByCache = NavigationPropertyCahngedByCache;
            if (propertyChangedByCache != null)
            {
                propertyChangedByCache(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static bool operator ==(_EntityBase left, _EntityBase right)
        {
            if (ReferenceEquals(null, left))
            {
                if (ReferenceEquals(null, right))
                    return true;
                else
                    return false;
            }
            else
                return left.Equals(right);
        }

        public static bool operator !=(_EntityBase left, _EntityBase right)
        {
            if (ReferenceEquals(null, left))
            {
                if (ReferenceEquals(null, right))
                    return false;
                else
                    return true;
            }
            else
                return !left.Equals(right);
        }

        // protected abstract void SetEntityInfo();

        private Byte[] _timeStamp;
        [Timestamp]
        [DataMember]
        public virtual Byte[] TimeStamp
        {
            get { return _timeStamp; }
            set
            {

                _timeStamp = value;
                //when TimeStamp reset must reset TimeStampUnit too, so set _timeStampStr = 0;
                _timeStampStr = 0;
            }
        }

        public event PropertyChangedEventHandler NavigationPropertyCahngedByCache;

        private bool _isDeletedForCache;

        [NotMapped]
        public bool IsDeletedForCache
        {
            get { return _isDeletedForCache; }
            set
            {
                _isDeletedForCache = value;
                ///TODO: Must delete Item from  EntityInfo.AllNavigationPropertyDataList
                //foreach (var list in EntityInfo.AllNavigationPropertyDataList)
                //{
                //    list.Where(en => !en.Equals(this)).ToList().ForEach((item) =>
                //    {
                //        list.TryTake(out item);
                //    });
                //}
            }
        }
    }


    [Serializable]
    [DataContract(IsReference = true)]
    public  class EntityBase<T> : _EntityBase, IEntity, System.IEquatable<_EntityBase> where T : IEntity
    {

        public EntityBase()
        {
            //  dicValue = new Dictionary<string, object>();
            // SetEntityInfo();
        }


        private static object _lockObject = new object();

        public override void CallPropertyChangedByCache(IList<_EntityBase> senders, string propertyName)
        {
            CallPropertyChangedByCache(senders, propertyName);
        }


        public static void CallPropertyChangedByCache(List<T> senders, string propertyName)
        {
            var propertyChangedByCache = PropertyChangedByCache;
            if (propertyChangedByCache != null)
            {
                propertyChangedByCache(senders, new PropertyChangedEventArgs(propertyName));
            }
        }

        public EntityBase<T> ShallowCopy()
        {
            return MemberwiseClone() as EntityBase<T>;
        }


        public static event PropertyChangedEventHandler<T> PropertyChangedByCache;
    }

    public delegate bool PropertyChangedEventHandler<Entity>(List<Entity> sender, PropertyChangedEventArgs e);
    public delegate bool PropertyChangedEventHandler(_EntityBase senders, PropertyChangedEventArgs e);

}

