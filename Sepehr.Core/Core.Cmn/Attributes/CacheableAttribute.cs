using Core.Cmn.Cache;
using System;

namespace Core.Cmn.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CacheableAttribute : Attribute
    {
        public CacheableAttribute()
            : base()
        {
        }

        public CacheRefreshingKind CacheRefreshingKind { get; set; }

        public bool DisableCache { get; set; }

        /// <summary>
        /// this could be true when EnableToFetchOnlyChangedDataFromDB is true and we don't want to sync deleted record in cache.
        /// </summary>
        public bool DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB { get; set; }

        /// <summary>
        /// This is depricated, please use 'CacheRefreshingKind = CacheRefreshingKind.Slide;' instead.
        /// </summary>
        [Obsolete(message: "'EnableAutomaticallyAndPeriodicallyRefreshCache' property in 'Core.Cmn.Attributes.CacheableAttribute' is deprecated, please use 'CacheRefreshingKind = CacheRefreshingKind.Slide;' instead.", error: false)]
        public bool EnableAutomaticallyAndPeriodicallyRefreshCache { get; set; }

        public bool EnableCoreSerialization { get; set; }
        public bool EnableSaveCacheOnHDD { get; set; }

        /// <summary>
        /// If this be true cache get all data at the first time and get changed data for every period of expire time.
        /// </summary>
        public bool EnableToFetchOnlyChangedDataFromDB { get; set; }

        public bool EnableUseCacheServer { get; set; }
        public int ExpireCacheSecondTime { get; set; }

        /// <summary>
        /// Set it just if EnableToFetchOnlyChangedDataFromDB is true.
        /// </summary>
        public string NameOfNavigationPropsForFetchingOnlyChangedDataFromDB { get; set; }
    }
}