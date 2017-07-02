using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;


namespace Core.Cmn.Cache.Server
{
    [ServiceContract]
    public interface IServerSideCacheServerService
    {
        [OperationContract]
        [ServiceKnownType("GetKnownTypes", typeof(CacheWCFTypeHelper))]
        object GetCacheDataViaWcf(ICacheDataProvider cacheDataProvider);
    }

    public class ServerSideCacheServerService : IServerSideCacheServerService
    {
        public object GetCacheDataViaWcf(ICacheDataProvider cacheDataProvider)
        {
            var cacheDataProvider1 = cacheDataProvider as CacheDataProvider;
            var cacheInfo = CacheConfig.CacheInfoDic.Values.First(item => item.UniqueKeyInServerLevel == cacheDataProvider1.CacheInfo.UniqueKeyInServerLevel);
            cacheDataProvider1.SetFunc(cacheInfo.Func);
            var c = cacheDataProvider1.CacheInfo;
            cacheDataProvider1.CacheInfo = cacheInfo;
            var cacheKey = cacheDataProvider1.GenerateCacheKey();
            Type genericType = cacheInfo.MethodInfo.ReturnType;
            if (typeof(IQueryable).IsAssignableFrom(cacheInfo.MethodInfo.ReturnType))
                genericType = typeof(List<>).MakeGenericType(cacheInfo.MethodInfo.ReturnType.GenericTypeArguments[0]);
            else
                genericType = cacheInfo.MethodInfo.ReturnType;

            var result = (typeof(CacheBase)).GetMethod("Cache").MakeGenericMethod(genericType).Invoke(null, new object[] { cacheDataProvider, cacheInfo, cacheInfo.ExpireCacheSecondTime, cacheKey, true });
            if (cacheDataProvider1.CacheInfo.EnableToFetchOnlyChangedDataFromDB)
            {
                var resultlist = (result as IList);
                var resultQueryable = QueryableCacheDataProvider<string>.MakeQueryableForFetchingOnleyChangedDataFromDB(resultlist.AsQueryable(), c, false);
                List<object> lsttmp = new System.Collections.Generic.List<object>();
                System.Diagnostics.Debug.WriteLine("server :" + resultlist.Count);
                foreach (var item in resultQueryable)
                {
                    lsttmp.Add(item);
                }

                var newlst = Activator.CreateInstance(result.GetType()) as IList;

                foreach (var item in lsttmp)
                {
                    newlst.Add(item);
                }

                System.Diagnostics.Debug.WriteLine("server after filter:" + resultlist.Count);

                result = newlst;
            }

            if (cacheInfo.EnableCoreSerialization)
            {
                result = SerializeCacheData(result);
            }
            return result;

        }

        private object SerializeCacheData(object result)
        {
            throw new NotImplementedException();
        }
    }
}
