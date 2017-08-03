using Core.Serialization;
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
            var clientCacheDataProvider = cacheDataProvider as CacheDataProvider;
            var serverCacheInfo = CacheConfig.CacheInfoDic.Values.First(item => item.UniqueKeyInServerLevel == clientCacheDataProvider.CacheInfo.UniqueKeyInServerLevel);
            clientCacheDataProvider.SetFunc(serverCacheInfo.Func);
            var c = clientCacheDataProvider.CacheInfo;
            clientCacheDataProvider.CacheInfo = serverCacheInfo;
            var cacheKey = clientCacheDataProvider.GenerateCacheKey();
            Type genericType = serverCacheInfo.MethodInfo.ReturnType;
            if (typeof(IQueryable).IsAssignableFrom(serverCacheInfo.MethodInfo.ReturnType))
                genericType = typeof(List<>).MakeGenericType(serverCacheInfo.MethodInfo.ReturnType.GenericTypeArguments[0]);
            else
                genericType = serverCacheInfo.MethodInfo.ReturnType;

            var result = (typeof(CacheBase)).GetMethod("Cache").MakeGenericMethod(genericType).Invoke(null, new object[] { cacheDataProvider, serverCacheInfo, serverCacheInfo.RefreshCacheTimeSeconds, cacheKey, true });
            if (clientCacheDataProvider.CacheInfo.EnableToFetchOnlyChangedDataFromDB)
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

            if (serverCacheInfo.EnableCoreSerialization)
            {
                result = SerializeCacheData(result);
            }
            return result;

        }

        private byte[] SerializeCacheData(object result)
        {
            return BinaryConverter.Serialize(result);
        }
    }
}
