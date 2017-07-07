using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CacheableAttribute : Attribute
    {
        public CacheableAttribute()
            : base()
        {

        }
        /// <summary>
        /// this could be true when EnableToFetchOnlyChangedDataFromDB is true and we don't want to sync deleted record in cache.
        /// </summary>
        public bool DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB { get; set; }
        public int ExpireCacheSecondTime { get; set; }

        public bool EnableUseCacheServer { get; set; }
        /// <summary>
        /// If this be true cache get all data at the first time and get changed data for every period of expire time.
        /// </summary>
        public bool EnableToFetchOnlyChangedDataFromDB { get; set; }
        /// <summary>
        /// Set it just if EnableToFetchOnlyChangedDataFromDB is true.
        /// </summary>
        public string NameOfNavigationPropsForFetchingOnlyChangedDataFromDB { get; set; }

        public bool EnableAutomaticallyAndPeriodicallyRefreshCache { get; set; }

        public bool DisableCache { get; set; }
        public bool EnableSaveCacheOnHDD { get; set; }
        public bool EnableCoreSerialization { get; set; }
    }
}
