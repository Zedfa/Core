using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data;

using System.ServiceModel;
using System.Runtime.Serialization;

using System.Collections;
using System.Collections.Concurrent;
using Core.Serialization;

namespace Core.Cmn.Cache
{
    public class CustomCacheFaultedException : Exception
    {
        public CustomCacheFaultedException(ExceptionDetail exceptionDetail)
            : base(exceptionDetail.Message, exceptionDetail.InnerException == null ?
            new CustomCacheFaultedException() : new CustomCacheFaultedException(exceptionDetail.InnerException))
        {
            _stackTrace = exceptionDetail.StackTrace;
        }

        private CustomCacheFaultedException()
            : base()
        {
            _stackTrace = string.Empty;
        }

        private string _stackTrace;
        public override string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }

    }


    [DataContract]
    public abstract class CacheDataProvider : ICacheDataProvider
    {
        public static ConcurrentDictionary<int, string[]> TimeStamps;

        static CacheDataProvider()
        {
            TimeStamps = new ConcurrentDictionary<int, string[]>();
        }
        public CacheDataProvider(CacheInfo cacheInfo)
        {
            CacheInfo = cacheInfo;
        }

        [DataMember]
        public CacheInfo CacheInfo { get; set; }
        public object GetDataFromCacheServer()
        {
            if (CacheInfo.EnableUseCacheServer)
            {
                if (!ConfigHelper.GetConfigValue<bool>("IsCacheServer"))
                {
                    return GetDataFromCacheServerViaWCF();
                }
            }
            return null;
        }

        private object GetDataFromCacheServerViaWCF()
        {
            EndpointAddress endpointAddress = new EndpointAddress(ConfigHelper.GetConfigValue<string>("CacheClientWebServiceUrl"));
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            // binding.TransferMode = TransferMode.Streamed;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.CloseTimeout = new TimeSpan(0, 20, 0);
            binding.OpenTimeout = new TimeSpan(0, 20, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 20, 0);
            binding.SendTimeout = new TimeSpan(0, 20, 0);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            using (ServiceClient proxy = new ServiceClient(binding, endpointAddress))
            {
                var logerService = AppBase.LogService;
                try
                {
                    var result = proxy.GetCacheDataViaWcf(this);
                    if (CacheInfo.EnableCoreSerialization)
                    {
                        result = DeserializeCacheData((byte[])result);
                    }

                    proxy.Close();
                    if (CacheInfo.EnableToFetchOnlyChangedDataFromDB)
                        QueryableCacheDataProvider<_EntityBase>.CalcAllTimeStampAndSet(result as IEnumerable, CacheInfo, false);
                    return result;
                }
                catch (FaultException exc)
                {
                    if (exc is System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>)
                    {
                        var customEx = new CustomCacheFaultedException((exc as FaultException<ExceptionDetail>).Detail);
                        var eLog = logerService.GetEventLogObj();
                        eLog.OccuredException = customEx;
                        eLog.UserId = "Cache";
                        eLog.CustomMessage = "cache server error report in client side!";
                        logerService.Handle(eLog);
                        proxy.Abort();
                        throw customEx;
                    }
                    else
                    {
                        var eLog = logerService.GetEventLogObj();
                        eLog.OccuredException = exc;
                        eLog.UserId = "Cache";
                        eLog.CustomMessage = "cache server error report in client side!";
                        logerService.Handle(eLog);
                        proxy.Abort();
                        throw exc;
                    }

                }
                catch (Exception ex)
                {
                    var eLog = logerService.GetEventLogObj();
                    eLog.OccuredException = ex;
                    eLog.UserId = "Cache";
                    eLog.CustomMessage = "cache server error report in client side!";
                    logerService.Handle(eLog);
                    proxy.Abort();
                    throw ex;
                }
            }
        }

        protected abstract object DeserializeCacheData(byte[] result);

        public virtual string GenerateCacheKey()
        {
            return CacheInfo.BasicKey.ToString();
        }
        public virtual void SetFunc(object func)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cacheInfo"></param>
        /// <param name="isForServerSide">this is 'true' if must excecute in cache server...</param>
        /// <returns></returns>
        public static IQueryable MakeQueryableForFetchingOnleyChangedDataFromDB(IQueryable result, CacheInfo cacheInfo, bool isForServerSide)
        {
            if (cacheInfo.EnableToFetchOnlyChangedDataFromDB && cacheInfo.MaxTimeStamp != null)
            {
                var parames = new List<object>();
                string whereConditionString;
                if (isForServerSide)
                    whereConditionString = "@0 == TimeStamp";
                else
                    whereConditionString = "@0 < TimeStampUnit";


                var counter = 0;

                if (isForServerSide)
                    parames.Add(cacheInfo.MaxTimeStamp);
                else
                    parames.Add(cacheInfo.MaxTimeStampUint);

                var strTimeStampForDic = ConvertToStringFromTimeStamp(cacheInfo.MaxTimeStamp);
                var strTimeStampLst = new List<string>();
                strTimeStampLst.Add(strTimeStampForDic);
                if (!string.IsNullOrWhiteSpace(cacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                {
                    foreach (var propStr in cacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB.Split(','))
                    {

                        ulong timeStamp;
                        if (cacheInfo.MaxTimeStamesDic.TryGetValue(propStr, out timeStamp))
                        {
                            counter++;
                            whereConditionString += string.Format(cacheInfo.WhereClauseForFetchingOnlyChangedDataFromDB_Dic[propStr], counter);
                            var ts = BitConverter.GetBytes(timeStamp).Reverse().ToArray();
                            if (isForServerSide)
                                parames.Add(ts);
                            else
                                parames.Add(timeStamp);

                            var strTimeStampForDic1 = ConvertToStringFromTimeStamp(ts);
                            strTimeStampLst.Add(strTimeStampForDic1);
                        }
                    }

                    //  var result1 = result.ToList().AsQueryable();
                    //  var a = result1.Where(whereConditionString, parames.ToArray());
                }

                result = result.Where(whereConditionString, parames.ToArray());

                if (isForServerSide)
                    TimeStamps[result.ToString().GetHashCode()] = strTimeStampLst.ToArray();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="cacheInfo"></param>
        /// <param name="isForServerSide">this is 'true' if must excecute in cache server...</param>
        public static void CalcAllTimeStampAndSet(IEnumerable resultList, CacheInfo cacheInfo, bool isForServerSide)
        {
            var dataLst = resultList.Cast<_EntityBase>().ToList();
            if (dataLst.Count > 0 && cacheInfo.EnableToFetchOnlyChangedDataFromDB)
            {
                CalcTimeStampAndSet(cacheInfo, resultList.Cast<_EntityBase>().ToList(), null, isForServerSide);
                if (!string.IsNullOrWhiteSpace(cacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                {
                    var navigationPropsForGettingChangedData = cacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB.Split(',');
                    foreach (var prop in navigationPropsForGettingChangedData)
                    {
                        CalcTimeStampAndSet(cacheInfo, resultList.Cast<_EntityBase>().ToList(), prop, isForServerSide);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheInfo"></param>
        /// <param name="resultList"></param>
        /// <param name="key"></param>
        /// <param name="isForServerSide">this is 'true' if must excecute in cache server...</param>
        public static void CalcTimeStampAndSet(CacheInfo cacheInfo, List<_EntityBase> resultList, string key, bool isForServerSide)
        {

            ulong maxTimeStampUnit = 0;
            byte[] maxTimeStamp = null;
            if (key == null)
            {
                CalcTimeStamp(resultList, out maxTimeStampUnit, out maxTimeStamp);

                if (cacheInfo.MaxTimeStampUint < maxTimeStampUnit)
                {
                    cacheInfo.MaxTimeStampUint = maxTimeStampUnit;
                    cacheInfo.MaxTimeStamp = maxTimeStamp;
                }

            }
            else
            {
                IEnumerable<_EntityBase> navData = resultList;
                var WhereClauseForFetchingOnlyChangedDataFromDB = " or ({0} {1}) ";
                var props = string.Empty;
                foreach (var prop in key.Split('.'))
                {

                    if (navData.Count() == 0)
                    {
                        WhereClauseForFetchingOnlyChangedDataFromDB = null;
                        return;
                    }

                    var firstItem = navData.FirstOrDefault(item => item[prop] != null);

                    if (firstItem == null)
                    {
                        WhereClauseForFetchingOnlyChangedDataFromDB = null;
                        return;
                    }

                    var whereClause_CheckingNotNullPart = string.Empty;

                    if ((typeof(IEnumerable)).IsAssignableFrom(firstItem[prop].GetType()))
                    {
                        props = prop;
                        navData = navData.Where(itm => itm[prop] != null).SelectMany(item => ((IEnumerable)item[prop]).Cast<_EntityBase>());
                        //  WhereClauseForFetchingOnlyChangedDataFromDB = string.Format(WhereClauseForFetchingOnlyChangedDataFromDB, props + " != null and ", prop + ".Any( {0}  {1} )");
                        WhereClauseForFetchingOnlyChangedDataFromDB = string.Format(WhereClauseForFetchingOnlyChangedDataFromDB, "", prop + ".Any( {0}  {1} )");

                        props = string.Empty;

                    }
                    else
                    {
                        props += prop;
                        navData = navData.Where(itm => itm[prop] != null).Select(item => (_EntityBase)item[prop]);
                        WhereClauseForFetchingOnlyChangedDataFromDB = string.Format(WhereClauseForFetchingOnlyChangedDataFromDB, props + " != null and {0}", prop + ".{1}");

                    }

                    if (!string.IsNullOrEmpty(props))
                        props += ".";
                }

                string timeSatmpCondition;
                if (isForServerSide)
                    timeSatmpCondition = "TimeStamp == @{0}";
                else
                    timeSatmpCondition = "TimeStampUnit > @{0}";

                if (WhereClauseForFetchingOnlyChangedDataFromDB != null)
                {
                    //WhereClauseForFetchingOnlyChangedDataFromDB = WhereClauseForFetchingOnlyChangedDataFromDB.Replace(".{0}", "{0}");
                    WhereClauseForFetchingOnlyChangedDataFromDB = string.Format(WhereClauseForFetchingOnlyChangedDataFromDB, string.Empty, timeSatmpCondition);
                    if (navData.Count() == 0)
                        return;
                    CalcTimeStamp(navData.ToList(), out maxTimeStampUnit, out maxTimeStamp);
                    maxTimeStamp = BitConverter.GetBytes(maxTimeStampUnit).Reverse().ToArray();
                    ulong oldMaxTimeStampUnit;
                    if (cacheInfo.MaxTimeStamesDic.TryGetValue(key, out oldMaxTimeStampUnit))
                    {
                        if (maxTimeStampUnit > oldMaxTimeStampUnit)
                            cacheInfo.MaxTimeStamesDic[key] = maxTimeStampUnit;
                    }
                    else
                    {
                        cacheInfo.MaxTimeStamesDic[key] = maxTimeStampUnit;
                    }

                    cacheInfo.WhereClauseForFetchingOnlyChangedDataFromDB_Dic[key] = WhereClauseForFetchingOnlyChangedDataFromDB;
                }
            }
        }

        private static void CalcTimeStamp(List<_EntityBase> resultList, out ulong maxTimeStampUnit, out byte[] maxTimeStamp)
        {
            maxTimeStampUnit = resultList.Max(item => item.TimeStampUnit);
            maxTimeStamp = BitConverter.GetBytes(maxTimeStampUnit).Reverse().ToArray();
        }

        private static string ConvertToStringFromTimeStamp(byte[] timeStamp)
        {
            return "0x" + BitConverter.ToString(timeStamp).Replace("-", string.Empty);
        }
    }

    [DataContract]
    public abstract class CacheDataProvider<T> : CacheDataProvider, ICacheDataProvider<T>
    {
        public CacheDataProvider(CacheInfo cacheInfo) : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
        }
        protected override object DeserializeCacheData(byte[] result)
        {
            return BinaryConverter.Deserialize<T>(result);
        }
        public T GetFreshData()
        {
            T resultList;
            T hDDResult;

            if (CacheInfo.EnableUseCacheServer && !ConfigHelper.GetConfigValue<bool>("IsCacheServer"))
            {
                resultList = (T)GetDataFromCacheServer();
            }
            else
            if (CacheInfo.EnableSaveCacheOnHDD && CacheInfo.NotYetGetCacheData && TryGetDataFromHDD(out hDDResult))
            {
                resultList = hDDResult;
            }
            else
            {
                resultList = ExcecuteCacheMethod();
            }

            if (resultList is IList)
            {
                CacheInfo.LastRecordCount = (resultList as IList).Count;
            }

            return resultList;
        }

        protected virtual bool TryGetDataFromHDD(out T cacheData)
        {
            try
            {
                var text = System.IO.File.ReadAllText(string.Format(@".../Cache/{0}.Cache", CacheInfo.Name));
                var obj = Core.Cmn.Extensions.SerializationExtensions.DeSerializeJSONToObject<T>(text);
                cacheData = obj;
                return true;
            }
            catch
            {
                cacheData = default(T);
                return false;
            }
        }

        protected abstract T ExcecuteCacheMethod();
    }


    [DataContract]
    public class QueryableCacheDataProvider<T> : CacheDataProvider<List<T>>
    {

        public QueryableCacheDataProvider(CacheInfo cacheInfo)
            : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
        }


        protected override List<T> ExcecuteCacheMethod()
        {
            List<T> resultList;
            var result = CacheInfo.MethodInfo.Invoke(null, new object[] { CacheInfo.Repository.GetQueryableForCahce() }) as IQueryable<T>;
            result = MakeQueryableForFetchingOnleyChangedDataFromDB(result, CacheInfo, true) as IQueryable<T>;
            CacheInfo.LastQueryStringOnlyForQueryableCache = result.ToString();
            CacheInfo.MaxTimeStampCopy = CacheInfo.Repository.GetMaxTimeStamp();
            if (CacheInfo.MaxTimeStampCopy.Count() < 8)
                CacheInfo.MaxTimeStampUintCopy = 0;
            else
                CacheInfo.MaxTimeStampUintCopy = BitConverter.ToUInt64(CacheInfo.MaxTimeStampCopy.Reverse().ToArray(), 0);
            if (CacheInfo.MaxTimeStampUintCopy == CacheInfo.MaxTimeStampUint && string.IsNullOrEmpty(CacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                resultList = new List<T>();
            else
                resultList = result.ToList();
            return resultList;
        }

        protected override bool TryGetDataFromHDD(out List<T> cacheData)
        {
            if (base.TryGetDataFromHDD(out cacheData))
            {

                return true;
            }
            else
                return false;
        }
    }

    [DataContract]
    public class QueryableCacheDataProvider<T, P1> : CacheDataProvider<List<T>>
    {
        [DataMember]
        public P1 Param1 { get; set; }
        public QueryableCacheDataProvider(CacheInfo cacheInfo, P1 param1)
            : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
            Param1 = param1;
        }
        public override string GenerateCacheKey()
        {
            return $"{base.GenerateCacheKey()}_{Param1}";
        }

        protected override List<T> ExcecuteCacheMethod()
        {
            List<T> resultList;
            var result = CacheInfo.MethodInfo.Invoke(null, new object[] { CacheInfo.Repository.GetQueryableForCahce(), Param1 }) as IQueryable<T>;
            result = MakeQueryableForFetchingOnleyChangedDataFromDB(result, CacheInfo, true) as IQueryable<T>;
            CacheInfo.LastQueryStringOnlyForQueryableCache = result.ToString();
            CacheInfo.MaxTimeStampCopy = CacheInfo.Repository.GetMaxTimeStamp();
            if (CacheInfo.MaxTimeStampCopy.Count() < 8)
                CacheInfo.MaxTimeStampUintCopy = 0;
            else
                CacheInfo.MaxTimeStampUintCopy = BitConverter.ToUInt64(CacheInfo.MaxTimeStampCopy.Reverse().ToArray(), 0);
            if (CacheInfo.MaxTimeStampUintCopy == CacheInfo.MaxTimeStampUint && string.IsNullOrEmpty(CacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                resultList = new List<T>();
            else
                resultList = result.ToList();
            return resultList;
        }
    }

    [DataContract]
    public class QueryableCacheDataProvider<T, P1, P2> : CacheDataProvider<List<T>>
    {
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        public QueryableCacheDataProvider(CacheInfo cacheInfo, P1 param1, P2 param2)
            : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
            Param1 = param1;
            Param2 = param2;
        }
        protected override List<T> ExcecuteCacheMethod()
        {
            List<T> resultList;
            var result = CacheInfo.MethodInfo.Invoke(null, new object[] { CacheInfo.Repository.GetQueryableForCahce(), Param1, Param2 }) as IQueryable<T>;
            result = MakeQueryableForFetchingOnleyChangedDataFromDB(result, CacheInfo, true) as IQueryable<T>;
            CacheInfo.LastQueryStringOnlyForQueryableCache = result.ToString();
            CacheInfo.MaxTimeStampCopy = CacheInfo.Repository.GetMaxTimeStamp();
            if (CacheInfo.MaxTimeStampCopy.Count() < 8)
                CacheInfo.MaxTimeStampUintCopy = 0;
            else
                CacheInfo.MaxTimeStampUintCopy = BitConverter.ToUInt64(CacheInfo.MaxTimeStampCopy.Reverse().ToArray(), 0);
            if (CacheInfo.MaxTimeStampUintCopy == CacheInfo.MaxTimeStampUint && string.IsNullOrEmpty(CacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                resultList = new List<T>();
            else
                resultList = result.ToList();
            return resultList;
        }
        public override string GenerateCacheKey()
        {
            return $"{base.GenerateCacheKey()}_{Param1}_{Param2}";
        }
    }

    [DataContract]
    public class QueryableCacheDataProvider<T, P1, P2, P3> : CacheDataProvider<List<T>>
    {
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        [DataMember]
        public P3 Param3 { get; set; }
        public QueryableCacheDataProvider(CacheInfo cacheInfo, P1 param1, P2 param2, P3 param3)
            : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
        }
        protected override List<T> ExcecuteCacheMethod()
        {
            List<T> resultList;
            var result = CacheInfo.MethodInfo.Invoke(null, new object[] { CacheInfo.Repository.GetQueryableForCahce(), Param1, Param2, Param3 }) as IQueryable<T>;
            result = MakeQueryableForFetchingOnleyChangedDataFromDB(result, CacheInfo, true) as IQueryable<T>;
            CacheInfo.LastQueryStringOnlyForQueryableCache = result.ToString();
            CacheInfo.MaxTimeStampCopy = CacheInfo.Repository.GetMaxTimeStamp();
            if (CacheInfo.MaxTimeStampCopy.Count() < 8)
                CacheInfo.MaxTimeStampUintCopy = 0;
            else
                CacheInfo.MaxTimeStampUintCopy = BitConverter.ToUInt64(CacheInfo.MaxTimeStampCopy.Reverse().ToArray(), 0);
            if (CacheInfo.MaxTimeStampUintCopy == CacheInfo.MaxTimeStampUint && string.IsNullOrEmpty(CacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                resultList = new List<T>();
            else
                resultList = result.ToList();
            return resultList;
        }

        public override string GenerateCacheKey()
        {
            return $"{base.GenerateCacheKey()}_{Param1}_{Param2}_{Param3}";
        }
    }

    [DataContract]
    public class QueryableCacheDataProvider<T, P1, P2, P3, P4> : CacheDataProvider<List<T>>
    {
        [DataMember]
        public P1 Param1 { get; set; }
        [DataMember]
        public P2 Param2 { get; set; }
        [DataMember]
        public P3 Param3 { get; set; }
        [DataMember]
        public P4 Param4 { get; set; }
        public QueryableCacheDataProvider(CacheInfo cacheInfo, P1 param1, P2 param2, P3 param3, P4 param4)
            : base(cacheInfo)
        {
            CacheInfo = cacheInfo;
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            Param4 = param4;
        }
        protected override List<T> ExcecuteCacheMethod()
        {
            List<T> resultList;
            var result = CacheInfo.MethodInfo.Invoke(null, new object[] { CacheInfo.Repository.GetQueryableForCahce(), Param1, Param2, Param3, Param4 }) as IQueryable<T>;
            result = MakeQueryableForFetchingOnleyChangedDataFromDB(result, CacheInfo, true) as IQueryable<T>;
            CacheInfo.LastQueryStringOnlyForQueryableCache = result.ToString();
            CacheInfo.MaxTimeStampCopy = CacheInfo.Repository.GetMaxTimeStamp();
            if (CacheInfo.MaxTimeStampCopy.Count() < 8)
                CacheInfo.MaxTimeStampUintCopy = 0;
            else
                CacheInfo.MaxTimeStampUintCopy = BitConverter.ToUInt64(CacheInfo.MaxTimeStampCopy.Reverse().ToArray(), 0);
            if (CacheInfo.MaxTimeStampUintCopy == CacheInfo.MaxTimeStampUint && string.IsNullOrEmpty(CacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB))
                resultList = new List<T>();
            else
                resultList = result.ToList();
            return resultList;
        }

        public override string GenerateCacheKey()
        {
            return $"{base.GenerateCacheKey()}_{Param1}_{Param2}_{Param3}_{Param4}";
        }
    }

}
