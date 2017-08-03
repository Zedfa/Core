using Core.Cmn.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;

namespace Core.Cmn.DataSource
{
    public class DataSourceInfo
    {
        public DataSourceInfo()
        {
            MaxTimeStamesDic = new ConcurrentDictionary<string, ulong>();
            WhereClauseForFetchingOnlyChangedDataFromDB_Dic = new ConcurrentDictionary<string, string>();
            InfoAndEntityListForFillingNavigationPropertyDic = new ConcurrentDictionary<InfoForFillingNavigationProperty, ConcurrentBag<_EntityBase>>();
            NotYetGetCacheData = true;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int BasicKey { get; set; }
        [DataMember]
        public int ExpireCacheSecondTime { get; set; }
        [IgnoreDataMember]
        public MethodInfo MethodInfo { get; set; }
        [IgnoreDataMember]
        public IRepositoryCache Repository { get; set; }
        [IgnoreDataMember]
        public IServiceCache Service { get; set; }
        [IgnoreDataMember]
        public object Func { get; set; }
        [DataMember]
        public DateTime CreateDateTime { get; set; }
        [DataMember]
        public int FrequencyOfUsing { get; set; }
        [DataMember]
        public int FrequencyOfBuilding { get; set; }
        [DataMember]
        public TimeSpan BuildingTime { get; set; }
        [DataMember]
        public TimeSpan UsingTime { get; set; }
        [DataMember]
        public DateTime LastBuildDateTime { get; set; }
        [DataMember]
        public DateTime LastUseDateTime { get; set; }
        [DataMember]
        public TimeSpan AverageTimeToBuild
        {
            get
            {
                if (FrequencyOfBuilding == 0) return default(TimeSpan);
                return TimeSpan.FromMilliseconds(BuildingTime.TotalMilliseconds / FrequencyOfBuilding);
            }
            set
            {
                /// just for wcf serialization error.
            }
        }
        [DataMember]
        public TimeSpan AverageTimeToUse
        {
            get
            {
                if (FrequencyOfUsing == 0) return default(TimeSpan);
                return TimeSpan.FromMilliseconds(UsingTime.TotalMilliseconds / FrequencyOfUsing);
            }
            set
            {
                /// just for wcf serialization error.
            }
        }
        [DataMember]
        public int ErrorCount { get; set; }
        [DataMember]
        public bool EnableUseCacheServer { get; set; }

        [DataMember]
        public string UniqueKeyInServerLevel { get; set; }

        [DataMember]
        public UInt64 MaxTimeStampUintForDeletedRecord { get; set; }
        /// <summary>
        /// this copy of MaxTimeStampUintForDeletedRecord is for covering problem that has occurred when we had concurrent deletion and modification,
        ///in this situation cache first delete item from cache and added again for modifying it.
        /// </summary>
        [DataMember]
        public UInt64 MaxTimeStampUintForDeletedRecord2 { get; set; }
        [DataMember]
        [Timestamp]
        public byte[] MaxTimeStampCopy { get; set; }
        [DataMember]
        [Timestamp]
        public byte[] MaxTimeStamp { get; set; }
        [DataMember]
        public UInt64 MaxTimeStampUint { get; set; }
        [DataMember]
        public UInt64 MaxTimeStampUintCopy { get; set; }
        [DataMember]
        public ConcurrentDictionary<string, UInt64> MaxTimeStamesDic { get; set; }
        [DataMember]
        public bool EnableToFetchOnlyChangedDataFromDB { get; set; }
        [DataMember]
        public string NameOfNavigationPropsForFetchingOnlyChangedDataFromDB { get; set; }
        [DataMember]
        public ConcurrentDictionary<string, string> WhereClauseForFetchingOnlyChangedDataFromDB_Dic { get; set; }
        [DataMember]
        public string LastQueryStringOnlyForQueryableCache { get; set; }
        [DataMember]
        public bool IsAutomaticallyAndPeriodicallyRefreshCache { get; set; }
        [DataMember]
        public bool DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB { get; set; }
        public int CountOfWaitingThreads { get; set; }
        [IgnoreDataMember]
        public ConcurrentDictionary<InfoForFillingNavigationProperty, ConcurrentBag<_EntityBase>> InfoAndEntityListForFillingNavigationPropertyDic { get; set; }
        [DataMember]
        public bool DisableCache { get; set; }
        [DataMember]
        public bool IsFunctionalCache { get; set; }
        [DataMember]
        public bool EnableCoreSerialization { get; set; }
        [DataMember]
        public bool EnableSaveCacheOnHDD { get; set; }
        [DataMember]
        public int LastRecordCount { get; set; }
        public bool NotYetGetCacheData { get; internal set; }

        public event EventHandler<CacheChangeEventArgs> OnAddEntities;
        public event EventHandler<CacheChangeEventArgs> OnDeleteEntities;

        public void CallOnAddEntities(List<_EntityBase> changedEntities)
        {
            var tmp = OnAddEntities;
            if (tmp != null)
            {
                tmp(this, new CacheChangeEventArgs() { ChangedEntities = changedEntities });
            }
        }

        public void CallOnDeleteEntities(List<_EntityBase> changedEntities)
        {
            var tmp = OnDeleteEntities;
            if (tmp != null)
            {
                tmp(this, new CacheChangeEventArgs() { ChangedEntities = changedEntities });
            }
        }
    }
}