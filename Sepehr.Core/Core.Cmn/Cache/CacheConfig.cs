﻿using Core.Cmn.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Core.Cmn.Extensions;
using System.Collections.Concurrent;
using Core.Cmn.Cache;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using Core.Cmn.Cache.Server;

namespace Core.Cmn.Cache
{

    public class CacheConfig
    {
        static CacheConfig()
        {
            CacheInfoDic = new Dictionary<string, CacheInfo>();
        }
        public static Dictionary<string, CacheInfo> CacheInfoDic { get; private set; }
        static ICacheManagementRepository _cacheManagementRepository;
        public static ICacheManagementRepository CacheManagementRepository
        {
            get
            {
                if (_cacheManagementRepository == null)
                    _cacheManagementRepository = AppBase.DependencyInjectionManager.Resolve<ICacheManagementRepository>();
                return _cacheManagementRepository;
            }
        }
        public static void OnRepositoryCacheConfig(Type dBContext)
        {

            var repositoryAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.ToLower().Contains(".rep")).ToList();
            bool isNeedCache;
            if (Core.Cmn.ConfigHelper.TryGetConfigValue<bool>("IsNeedCache", out isNeedCache) && !isNeedCache)
            {
                repositoryAssemblies = repositoryAssemblies.Where(ass => ass.FullName.ToLower().Contains("core.rep")).ToList();
            }
            repositoryAssemblies.ForEach(ass => BuildCachesInAssembly(dBContext, ass, true));
            PeriodicTaskFactory p = new PeriodicTaskFactory((pt) =>
            {
                CacheConfig.CacheManagementRepository.Delete();
            }, new TimeSpan(0, 0, 1200), new TimeSpan(0, 0, 1200));

            p.Start();

            if (Core.Cmn.ConfigHelper.GetConfigValue<bool>("EnableCacheServerListener") && !IsCacheServerServiceStart)
                StartCacheServerService();
        }

        public static void OnServiceCacheConfig(Type dBContext)
        {
            var serviceAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.ToLower().Contains(".service")).ToList();
            bool isNeedCache;
            if (Core.Cmn.ConfigHelper.TryGetConfigValue<bool>("IsNeedCache", out isNeedCache) && !isNeedCache)
            {
                serviceAssemblies = serviceAssemblies.Where(ass => ass.FullName.ToLower().Contains("core.service")).ToList();
            }

            serviceAssemblies.ForEach(ass => BuildCachesInAssembly(dBContext, ass, false));
            if (Core.Cmn.ConfigHelper.GetConfigValue<bool>("EnableCacheServerListener") && !IsCacheServerServiceStart)
                StartCacheServerService();
        }

        public static ServiceHost serviceHost;

        public static bool IsCacheServerServiceStart { get; private set; }
        public static void StartCacheServerService()
        {
            Uri baseAddress = new Uri(ConfigHelper.GetConfigValue<string>("CacheHostWebServiceUrl"));
            Uri mexAddress = new Uri("mex", UriKind.Relative);
            serviceHost = new ServiceHost(typeof(ServerSideCacheServerService), baseAddress);

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
            binding.CloseTimeout = new TimeSpan(0, 2, 0);
            binding.OpenTimeout = new TimeSpan(0, 2, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 2, 0);
            binding.SendTimeout = new TimeSpan(0, 2, 0);
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            //binding.Security.Message.ClientCredentialType =  MessageCredentialType.Windows;

            serviceHost.AddServiceEndpoint(typeof(Core.Cmn.Cache.Server.IServerSideCacheServerService), binding, baseAddress);

            // Add metadata exchange behavior to the service
            ServiceDebugBehavior debug = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on 
            if (debug == null)
            {
                serviceHost.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }

            // Add a service Endpoint for the metadata exchange
            serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), mexAddress);

            // Run the service
            serviceHost.Open();
            Console.WriteLine("Service started. Press enter to terminate service.");
            IsCacheServerServiceStart = true;

            // serviceHost.Close();

        }


        static List<System.Windows.Forms.Timer> timers = new List<System.Windows.Forms.Timer>();
        private static void BuildCachesInAssembly(Type dBContext, Assembly repAssembly, bool isRepositoryAssembly)
        {
            IEnumerable<MethodInfo> types = null;
            try
            {
                types = repAssembly.GetTypes()
                 .SelectMany(type => type.GetMethods(BindingFlags.Static  | BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic));
            }
            catch (Exception ex)
            {
                return;
            }

            foreach (var type in types)
            {
                var cacheInfoAtt = type.GetAttributeValue<CacheableAttribute, CacheableAttribute>(cAtt => cAtt);
                if (cacheInfoAtt != null)
                {
                    //if (string.IsNullOrEmpty(cacheInfo.Key))
                    //{
                    //    throw new ArgumentNullException("Cache Key can't be empty.");
                    //}             

                    if ((type as MethodInfo).IsStatic)
                    {
                        var methodInfo = type as MethodInfo;
                        var parames = methodInfo.GetParameters();
                        Type funcBaseType = null;
                        if (parames.Count() == 0)
                            funcBaseType = typeof(Func<>);
                        if (parames.Count() == 1)
                            funcBaseType = typeof(Func<,>);
                        if (parames.Count() == 2)
                            funcBaseType = typeof(Func<,,>);
                        if (parames.Count() == 3)
                            funcBaseType = typeof(Func<,,,>);
                        if (parames.Count() == 4)
                            funcBaseType = typeof(Func<,,,,>);
                        if (parames.Count() == 5)
                            funcBaseType = typeof(Func<,,,,,>);
                        List<Type> tArgs = methodInfo.GetParameters().Select(item => item.ParameterType).ToList();
                        tArgs.Add(methodInfo.ReturnType);
                        var funcType = funcBaseType.MakeGenericType(tArgs.ToArray());
                        var funcInstanc = methodInfo.CreateDelegate(funcType);
                        var key = funcInstanc.Method.GetHashCode().ToString();
                        CacheInfo currentCacheInfo = null;
                        if (!CacheInfoDic.TryGetValue(key, out currentCacheInfo))
                            // throw new ArgumentException("Duplicate cache key can't be used , please use an unique key for cache.");
                            currentCacheInfo = CreateCacheInfo(cacheInfoAtt, methodInfo, funcInstanc, key);


                        if (typeof(IQueryable).IsAssignableFrom(methodInfo.ReturnType) && parames.Length > 0 && typeof(IQueryable).IsAssignableFrom(parames[0].ParameterType))
                        {
                            if (!isRepositoryAssembly)
                                throw new NotSupportedException("Queryable Cache just can use in Repository Layer.");
                            var repository = (IRepositoryCache)Activator.CreateInstance(methodInfo.DeclaringType, Activator.CreateInstance(dBContext));
                            currentCacheInfo.Repository = repository;
                            if (parames.Length == 1)
                            {
                                var cacheDataProviderType = typeof(QueryableCacheDataProvider<>);
                                Type[] typeArgs = methodInfo.ReturnType.GenericTypeArguments;
                                var cacheDataProviderGenericType = cacheDataProviderType.MakeGenericType(typeArgs);
                                var returnCacheType = typeof(List<>).MakeGenericType(methodInfo.ReturnType.GenericTypeArguments[0]);
                                CacheWCFTypeHelper.typeList.Add(cacheDataProviderGenericType);
                                CacheWCFTypeHelper.typeList.Add(methodInfo.ReturnType.GenericTypeArguments[0]);
                                CacheWCFTypeHelper.typeList.Add(returnCacheType);
                                // if (currentCacheInfo.EnableUseCacheServer)
                                //      CacheWCFTypeHelper.typeList.Add(methodInfo.ReturnType);
                                object queryableCacheExecution = Activator.CreateInstance(cacheDataProviderGenericType, new object[] { currentCacheInfo });

                                if (currentCacheInfo.FrequencyOfBuilding == 0)
                                {
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Start();
                                    if (!cacheInfoAtt.DisableCache)
                                        (typeof(CacheBase)).GetMethod("RefreshCache").MakeGenericMethod(returnCacheType).Invoke(null, new object[] { queryableCacheExecution, currentCacheInfo });
                                    stopwatch.Stop();
                                    currentCacheInfo.FrequencyOfBuilding += 1;
                                    currentCacheInfo.BuildingTime += new TimeSpan(stopwatch.ElapsedTicks);
                                }

                                if (cacheInfoAtt.EnableAutomaticallyAndPeriodicallyRefreshCache)
                                {

                                    //var timer = new System.Windows.Forms.Timer();
                                    //timers.Add(timer);
                                    //timer.Tick += (pt, a) =>
                                    //{
                                    //    try
                                    //    {
                                    //        Stopwatch stopwatch = new Stopwatch();
                                    //        stopwatch.Start();
                                    //        (typeof(CacheBase)).GetMethod("RefreshCache").MakeGenericMethod(returnCacheType).Invoke(null, new object[] { queryableCacheExecution, currentCacheInfo });
                                    //        stopwatch.Stop();
                                    //        currentCacheInfo.FrequencyOfBuilding += 1;
                                    //        currentCacheInfo.BuildingTime += new TimeSpan(stopwatch.ElapsedTicks);
                                    //        System.Diagnostics.Debug.WriteLine("qqqqqqqqqqqqqqqqqqqqqqqqqqqq");
                                    //    }
                                    //    catch
                                    //    { }
                                    //};

                                    //timer.Interval = cacheInfoAtt.ExpireCacheSecondTime * 1000;
                                    //timer.Start();


                                    PeriodicTaskFactory p = new PeriodicTaskFactory((pt) =>
                                    {
                                        if (currentCacheInfo.CountOfWaitingThreads < 3)
                                        {
                                            Stopwatch stopwatch = new Stopwatch();
                                            stopwatch.Start();
                                            (typeof(CacheBase)).GetMethod("RefreshCache").MakeGenericMethod(returnCacheType).Invoke(null, new object[] { queryableCacheExecution, currentCacheInfo });
                                            stopwatch.Stop();
                                            currentCacheInfo.FrequencyOfBuilding += 1;
                                            currentCacheInfo.BuildingTime += new TimeSpan(stopwatch.ElapsedTicks);
                                        }
                                    }, new TimeSpan(0, 0, cacheInfoAtt.ExpireCacheSecondTime), new TimeSpan(0, 0, cacheInfoAtt.ExpireCacheSecondTime));

                                    p.Start();
                                }

                            }

                            if (currentCacheInfo.EnableToFetchOnlyChangedDataFromDB && !currentCacheInfo.DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB)
                            {
                                CacheManagementRepository.CreateSqlTriggerForDetectingDeletedRecords(string.Format("{0}.{1}", repository.Schema, repository.TableName), repository.KeyName);
                            }
                        }
                        else
                        {
                            if (isRepositoryAssembly)
                                throw new NotSupportedException("Functional Cache just can use in Service Layer.");
                            var Service = (IServiceCache)Activator.CreateInstance(methodInfo.DeclaringType, Activator.CreateInstance(dBContext));
                            currentCacheInfo.Service = Service;
                            Type cacheDataProviderType = null;
                            if (parames.Length == 0)
                            {
                                cacheDataProviderType = typeof(FunctionalCacheDataProvider<>);
                                var cacheDataProviderGenericType = AddAndGetCacheDataProviderTypeForSerialization(methodInfo, cacheDataProviderType);
                                object functionalCacheExecution = Activator.CreateInstance(cacheDataProviderGenericType, new object[] { currentCacheInfo });
                                if (currentCacheInfo.FrequencyOfBuilding == 0)
                                {
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Start();
                                    if (!cacheInfoAtt.DisableCache)
                                        (typeof(CacheBase)).GetMethod("RefreshCache").MakeGenericMethod(methodInfo.ReturnType).Invoke(null, new object[] { functionalCacheExecution, currentCacheInfo });
                                    stopwatch.Stop();
                                    currentCacheInfo.FrequencyOfBuilding += 1;
                                    currentCacheInfo.BuildingTime += new TimeSpan(stopwatch.ElapsedTicks);
                                }

                                if (cacheInfoAtt.EnableAutomaticallyAndPeriodicallyRefreshCache)
                                {
                                    PeriodicTaskFactory p = new PeriodicTaskFactory((pt) =>
                                    {
                                        if (currentCacheInfo.CountOfWaitingThreads < 3)
                                        {
                                            Stopwatch stopwatch = new Stopwatch();
                                            stopwatch.Start();
                                            (typeof(CacheBase)).GetMethod("RefreshCache").MakeGenericMethod(methodInfo.ReturnType).Invoke(null, new object[] { functionalCacheExecution, currentCacheInfo });
                                            stopwatch.Stop();
                                            currentCacheInfo.FrequencyOfBuilding += 1;
                                            currentCacheInfo.BuildingTime += new TimeSpan(stopwatch.ElapsedTicks);
                                        }
                                    }, new TimeSpan(0, 0, cacheInfoAtt.ExpireCacheSecondTime), new TimeSpan(0, 0, cacheInfoAtt.ExpireCacheSecondTime));

                                    p.Start();
                                }
                            }
                            else
                            {

                                if (parames.Length == 1)
                                {
                                    cacheDataProviderType = typeof(FunctionalCacheDataProvider<,>);
                                }

                                if (parames.Length == 2)
                                {
                                    cacheDataProviderType = typeof(FunctionalCacheDataProvider<,,>);
                                }

                                if (parames.Length == 3)
                                {
                                    cacheDataProviderType = typeof(FunctionalCacheDataProvider<,,,>);
                                }

                                if (parames.Length == 4)
                                {
                                    cacheDataProviderType = typeof(FunctionalCacheDataProvider<,,,,>);
                                }

                                if (parames.Length == 5)
                                {
                                    cacheDataProviderType = typeof(FunctionalCacheDataProvider<,,,,,>);
                                }

                                AddAndGetCacheDataProviderTypeForSerialization(methodInfo, cacheDataProviderType);
                            }

                        }
                        currentCacheInfo.LastBuildDateTime = DateTime.Now;
                    }
                    else
                    {
                        throw new NotSupportedException("Cacheable Attribute can't use on Non static methods, because we can't work on nostatic method for caching.");
                    }

                }
            }

        }

        public static CacheInfo CreateCacheInfo(CacheableAttribute cacheInfoAtt, MethodInfo methodInfo, Delegate funcInstanc, string key)
        {
            var name = string.Format("{0}.{1}",
                                     methodInfo.ReflectedType.Name,
                                     methodInfo.Name);
            string parameterPartUniqueKey = string.Empty;
            foreach (var item in methodInfo.GetParameters())
            {
                parameterPartUniqueKey += "_" + item.ParameterType.Name;
            }
            var uniqueKeyInServerLevel = name + parameterPartUniqueKey;
            var currentCacheInfo = CacheInfoDic[key] = new CacheInfo()
            {
                Name = name,
                ExpireCacheSecondTime = cacheInfoAtt.ExpireCacheSecondTime,
                MethodInfo = methodInfo,
                BasicKey = int.Parse(key),
                CreateDateTime = DateTime.Now,
                Func = funcInstanc,
                EnableUseCacheServer = cacheInfoAtt.EnableUseCacheServer,
                UniqueKeyInServerLevel = uniqueKeyInServerLevel,
                EnableToFetchOnlyChangedDataFromDB = cacheInfoAtt.EnableToFetchOnlyChangedDataFromDB,
                NameOfNavigationPropsForFetchingOnlyChangedDataFromDB = cacheInfoAtt.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB,
                IsAutomaticallyAndPeriodicallyRefreshCache = cacheInfoAtt.EnableAutomaticallyAndPeriodicallyRefreshCache,
                DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB = cacheInfoAtt.DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB,
                DisableCache = cacheInfoAtt.DisableCache,
                EnableCoreSerialization = cacheInfoAtt.EnableSaveCacheOnHDD

            };

            var parames = methodInfo.GetParameters();
            if (typeof(IQueryable).IsAssignableFrom(methodInfo.ReturnType) && parames.Length > 0 && typeof(IQueryable).IsAssignableFrom(parames[0].ParameterType))
                currentCacheInfo.IsFunctionalCache = false;
            return currentCacheInfo;
        }

        private static Type AddAndGetCacheDataProviderTypeForSerialization(MethodInfo methodInfo, Type cacheDataProviderType)
        {
            List<Type> tArgs = methodInfo.GetParameters().Select(item => item.ParameterType).ToList();
            tArgs.Add(methodInfo.ReturnType);
            var cacheDataProviderGenericType = cacheDataProviderType.MakeGenericType(tArgs.ToArray());
            CacheWCFTypeHelper.typeList.Add(cacheDataProviderGenericType);
            CacheWCFTypeHelper.typeList.Add(methodInfo.ReturnType);
            return cacheDataProviderGenericType;
        }
    }
}