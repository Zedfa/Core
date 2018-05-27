namespace Core.Cmn
{
    using Core.Cmn.Cache;
    using Newtonsoft.Json;
    using Serialization;
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Web.Script.Serialization;

    [Serializable]
    [DataContract(IsReference = true)]
    public class ObjectBase : IEntity
    {
        private ConcurrentDictionary<string, object> _navigationPropertyDataDic;

        internal ConcurrentDictionary<string, object> NavigationPropertyDataDic => _navigationPropertyDataDic ??
                                                                                (_navigationPropertyDataDic = new ConcurrentDictionary<string, object>());

        /// <summary>
        /// It just works right for Single Pirimarykey, e.g: Id, not Id, name as PrimaryKey.
        /// </summary>
        /// <returns></returns>
        //public override int GetHashCode()
        //{
        //    // var key = this.EntityInfo().KeyColumns.Values.First();
        //    if (func == null)
        //    {
        //        var md = Core.Serialization.ObjectMetaData.GetEntityMetaData(this.GetType());
        //        func = md.ReflectionEmitPropertyAccessor.EmittedPropertyGetters[md.WritablePropertyNames["ID"]];
        //    }

        //    // return rnd.Next(0, int.MaxValue);
        //    return func(this).GetHashCode();
        //}

        private static Random rnd = new Random();

        protected ObjectBase()
        {
        }

        public virtual void UpdateAllProps(ObjectBase entity)
        {
            var props = this.EntityInfo().WritableMappedProperties;
            foreach (var prop in props)
            {
                this[prop.Key] = prop.Value.GetValue(entity);
            }
        }
        public ObjectBase ShallowCopy()
        {
            return MemberwiseClone() as ObjectBase;
        }
        public virtual void CallPropertyChangedByCache(IList<ObjectBase> senders, string propertyName)
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

        [NotMapped]
        //[NonSerialized]
        [IgnoreDataMember]
        [ScriptIgnore]
        [JsonIgnore]
        public ConcurrentDictionary<int, int[]> IndexOfIndexableProperties { get; set; }

        [NotMapped]
        //[NonSerialized]
        [IgnoreDataMember]
        [ScriptIgnore]
        [JsonIgnore]
        public ConcurrentDictionary<int, int[]> IndexOfGroupedIndexableProperties { get; set; }

        private int _cacheId;

        [NotMapped]
        [IgnoreDataMember]
        [ScriptIgnore]
        [JsonIgnore]
        internal virtual int CacheId
        {
            get
            {
                if (_cacheId == default(int))
                {
                    var keyColumnName = EntityInfo().KeyColumns.Keys.FirstOrDefault();
                    if (keyColumnName != null)
                    {
                        try
                        {
                            _cacheId = Convert.ToInt32(this[keyColumnName]);
                        }
                        catch (Exception ex)
                        {
                            throw AppBase.LogService.Handle(ex, $"On casting PK for EntityName: '{this.GetType().Name}' and PropertyName: '{keyColumnName}' an error has occured.");
                        }
                    }
                }
                return _cacheId;
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
                _entityInfo = Core.Cmn.EntityInfo.GetEntityInfo(this.GetType());
            }

            return _entityInfo;
        }

        [IndexerName("CustomIndexerProperty")]
        [IgnoreDataMember]
        public object this[string propertyName]
        {
            get
            {
                Func<object, object> getPropertyFunc;
                if (ObjectMetaData.ReflectionEmitPropertyAccessor.EmittedAllPropertyGetters.TryGetValue(propertyName, out getPropertyFunc))
                {
                    return getPropertyFunc(this);
                }
                else
                    throw new ArgumentException("PropertyName Is not exist!");
            }
            set
            {
                int setterFuncIndex;
                if (ObjectMetaData.WritablePropertyNames.TryGetValue(propertyName, out setterFuncIndex))
                {
                    var propValue = ObjectMetaData.ReflectionEmitPropertyAccessor.EmittedWritablePropertySetters[setterFuncIndex];
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

        private ObjectMetaData _objectMetaData;

        [IgnoreDataMember]
        internal ObjectMetaData ObjectMetaData
        {
            get
            {
                if (_objectMetaData == null)
                    _objectMetaData = Core.Serialization.ObjectMetaData.GetEntityMetaData(this.GetType());
                return _objectMetaData;
            }
        }

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != typeof(_EntityBase)) return false;
        //    return Equals((_EntityBase)obj);
        //}

        protected ICollection<T> GetNavigationPropertyDataListFromCache<T>([CallerMemberName]string navigationPropertyName = null) where T : ObjectBase
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
                object value;
                if (NavigationPropertyDataDic.TryGetValue(string.Concat(navigationPropertyName, "1"), out value))
                    result = (ICollection<T>)value;
            }

            return result;
        }

        private IEnumerable<T> GetNavigationPropertyDataAtFirstTime<T>(string navigationPropertyName, out object resultForSecondLevelDataSource) where T : ObjectBase
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
                                    ObjectBase resultItem;
                                    result = (cacheInfo.MethodInfo.Invoke(null, new object[] { cacheInfo.Repository.GetQueryableForCahce(AppBase.DependencyInjectionFactory.CreateContextInstance()) }) as IQueryable<T>)
                                            .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue));
                                    resultItem = result.FirstOrDefault() as ObjectBase;
                                    if (resultItem != null)
                                        resultItem.EnableFillNavigationProperyByCache();
                                }
                                else
                                {
                                    var queryableCacheExecution = new QueryableCacheDataProvider<T>(cacheInfo);
                                    //  _stopwatch.Restart();
                                    result = queryableCacheExecution.Cache<List<T>>(cacheInfo, cacheInfo.AutoRefreshInterval, cacheInfo.BasicKey.ToString(), true).
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
                                        resultForSecondLevelDataSource = ((dataSourceInfo.MethodInfo.Invoke(null, new object[] { dataSourceInfo.Repository.GetQueryableForCahce(AppBase.DependencyInjectionFactory.CreateContextInstance()) }) as IQueryable)
                                           .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue)) as IEnumerable).Cast<IEntity>().FirstOrDefault();
                                        if (resultForSecondLevelDataSource != null)
                                            ((ObjectBase)resultForSecondLevelDataSource).EnableFillNavigationProperyByCache();
                                    }

                                    ObjectBase resultItem;
                                    if (!cacheInfo.DisableCache && result.Count() == 0)
                                    {
                                        //ToDo: Change invoke method by excact delegate for better performance.
                                        result = (cacheInfo.MethodInfo.Invoke(null, new object[] { cacheInfo.Repository.GetQueryableForCahce(AppBase.DependencyInjectionFactory.CreateContextInstance()) }) as IQueryable<T>)
                                            .Where(string.Format("{0} == {1}", infoForFillingNavigationProperty.OtherEntityRefrencePropertyName, thisPropertyValue));
                                        resultItem = result.FirstOrDefault() as ObjectBase;
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
                        cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty] = new ConcurrentBag<ObjectBase>();
                        EntityInfo().AllNavigationPropertyDataList.Add(cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic[infoForFillingNavigationProperty]);
                        infoForFillingNavigationProperty.IsInitForFillingNavigationProperty = true;
                    }
                }
            }
        }

        protected void SetNavigationPropertyDataList(object value, [CallerMemberName]string navigationPropertyName = null)
        {
            NavigationPropertyDataDic[string.Concat(navigationPropertyName, "1")] = value;
        }

        protected T GetNavigationPropertyDataItemFromCache<T>([System.Runtime.CompilerServices.CallerMemberName]string navigationPropertyName = null) where T : ObjectBase
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
                object value;
                if (NavigationPropertyDataDic.TryGetValue(string.Concat(navigationPropertyName, "1"), out value))
                {
                    result = (T)value;
                }
            }

            return result;
        }

        protected IT GetNavigationPropertyDataItemFromCache<T, IT>([CallerMemberName]string navigationPropertyName = null) where T : ObjectBase
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
                object value;
                if (NavigationPropertyDataDic.TryGetValue(string.Concat(navigationPropertyName, "1"), out value))
                {
                    result = (IT)value;
                }
            }

            return result;
        }

        public bool Equals(ObjectBase other)
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

        public void CallNavigationPropertyChangedByCache(ObjectBase sender, string propertyName)
        {
            NavigationPropertyChangedByCache?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        //public static bool operator ==(_EntityBase left, _EntityBase right)
        //{
        //    if (ReferenceEquals(null, left))
        //    {
        //        if (ReferenceEquals(null, right))
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //        return left.Equals(right);
        //}

        //public static bool operator !=(_EntityBase left, _EntityBase right)
        //{
        //    if (ReferenceEquals(null, left))
        //    {
        //        if (ReferenceEquals(null, right))
        //            return false;
        //        else
        //            return true;
        //    }
        //    else
        //        return !left.Equals(right);
        //}

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

        public event PropertyChangedEventHandler NavigationPropertyChangedByCache;

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
    public class EntityBase<T> : ObjectBase, System.IEquatable<ObjectBase> where T : IEntity
    {
        public EntityBase()
        {
            //  dicValue = new Dictionary<string, object>();
            // SetEntityInfo();
        }

        private static object _lockObject = new object();

        public override void CallPropertyChangedByCache(IList<ObjectBase> senders, string propertyName)
        {
            CallPropertyChangedByCache(senders, propertyName);
        }

        public static void CallPropertyChangedByCache(List<T> senders, string propertyName)
        {
            PropertyChangedByCache?.Invoke(senders, new PropertyChangedEventArgs(propertyName));
        }

        public static event PropertyChangedEventHandler<T> PropertyChangedByCache;
    }

    public delegate bool PropertyChangedEventHandler<Entity>(List<Entity> sender, PropertyChangedEventArgs e);

    public delegate bool PropertyChangedEventHandler(ObjectBase senders, PropertyChangedEventArgs e);
}