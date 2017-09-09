using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    public static class CacheBase
    {
        static ILogService _logService = AppBase.LogService;
        public static T Cache<T>(this ICacheDataProvider<T> cacheExecution, CacheInfo cacheInfo, int expireCacheSecondTime, string cacheKey, bool canUseCache)
        {
            T result = default(T);
            var fakeResult = string.Empty;
            var cacheKeyFake = cacheKey + "_Fake";


            if (CacheService.TryGetCache<T>(cacheKey, out result))
            {
                if (!CacheService.TryGetCache<string>(cacheKeyFake, out fakeResult))
                {
                    if (cacheInfo.CacheRefreshingKind == CacheRefreshingKind.Slide)
                    {
                        CacheService.SetCache<string>(cacheKeyFake, "Fake value", expireCacheSecondTime * 10);
                    }
                    else
                    {
                        CacheService.SetCache<string>(cacheKeyFake, "Fake value", expireCacheSecondTime);
                    }

                    if (cacheInfo.CountOfWaitingThreads < 3)
                    {
                        var task = new Task(() =>
                        {
                            var resultFunc = default(T);
                            TryRefreshCache<T>(cacheExecution, cacheInfo, out resultFunc);
                        });

                        task.Start();
                    }
                }
                else
                {
                    cacheInfo.FrequencyOfUsing += 1;
                    cacheInfo.LastUseDateTime = DateTime.Now;
                }
            }
            else
            {
                result = cacheExecution.GetFreshData();
                if (cacheInfo.EnableToFetchOnlyChangedDataFromDB)
                {
                    List<_EntityBase> nlst = (result as IList).Cast<_EntityBase>().ToList();
                    QueryableCacheDataProvider<object>.CalcAllTimeStampAndSet(result as IList, cacheInfo, true);
                    if (nlst.Count > 0)
                    {
                        foreach (var en in nlst.Cast<_EntityBase>())
                        {
                            (en as _EntityBase).EnableFillNavigationProperyByCache();
                        }
                    }
                }
                CacheService.SetCache<T>(cacheKey, result, expireCacheSecondTime * (double)100000);
                CacheService.SetCache<string>(cacheKeyFake, "Fake value", expireCacheSecondTime);
            }
            return result;
        }

        public static T MergeFreshDataByOldCache<T>(T oldData, T newData, CacheInfo cacheInfo, bool isQueryableCache)
        {
            List<_EntityBase> entitiesForDeletion = null;
            List<_EntityBase> entitiesForAddition = null;
            var nlst = (newData as IList);
            T resultList = oldData;
            if (oldData == null)
                resultList = newData;
            else
            {
                if (!cacheInfo.DisableToSyncDeletedRecord_JustIfEnableToFetchOnlyChangedDataFromDB && !cacheInfo.IsFunctionalCache)
                {
                    var deletedRecords = CacheConfig.CacheManagementRepository.
                          GetDeletedRecordsByTable(cacheInfo.Repository.Schema + "." + cacheInfo.Repository.TableName,
                          cacheInfo.MaxTimeStampForDeletedRecord, cacheInfo.CacheRefreshingKind != CacheRefreshingKind.SqlDependency);

                    if (deletedRecords.Count > 0)
                    {
                        var maxDeletedRecordItem = (_EntityBase)deletedRecords.OrderByDescending(item => ((_EntityBase)item).TimeStampUnit).First();
                        var maxTimeStampDeletedRecords = ((_EntityBase)maxDeletedRecordItem).TimeStampUnit;
                        var oldEntityBaseLst = (oldData as IList).Cast<_EntityBase>().ToList();
                        var oldLst = oldData as IList;
                        entitiesForDeletion = new List<_EntityBase>();
                        foreach (var record in deletedRecords)
                        {
                            if (nlst != null && nlst.Count > 0)
                            {
                                var newItemToRemove = nlst.Cast<_EntityBase>().FirstOrDefault(item => item.CacheId == record.DeletedEntityId);
                                if (newItemToRemove != null)
                                {
                                    nlst.Remove(newItemToRemove);
                                    newItemToRemove.IsDeletedForCache = true;
                                }
                            }

                            var itemToRemove = oldEntityBaseLst.FirstOrDefault(item => item.CacheId == record.DeletedEntityId);
                            if (itemToRemove != null)
                            {
                                entitiesForDeletion.Add(itemToRemove);
                                oldLst.Remove(itemToRemove);
                                itemToRemove.IsDeletedForCache = true;

                                foreach (var inf in cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic.ToList())
                                {
                                    List<_EntityBase> entities;
                                    /// attention: Added Tolist Here
                                    //lock (inf.Value)
                                    //{
                                    entities = inf.Value.Where(en =>
                                    {
                                        var thisPropertyValue = en[inf.Key.ThisEntityRefrencePropertyName];
                                        return thisPropertyValue != null && thisPropertyValue.Equals(itemToRemove[inf.Key.OtherEntityRefrencePropertyName]);
                                    }).ToList();
                                    //}
                                    //Where(en => en[inf.Key.ThisEntityRefrencePropertyName].Equals(itemToRemove[inf.Key.OtherEntityRefrencePropertyName])).ToList()
                                    entities.ForEach(parentEntity =>
                                    {
                                        object navContainer;
                                        if (parentEntity.NavigationPropertyDataDic.TryGetValue(inf.Key.PropertyInfo.Name, out navContainer))
                                        {
                                            if (inf.Key.IsEnumerable)
                                            {
                                                // var navContainer = parentEntity.NavigationPropertyDataDic[inf.Key.PropertyInfo.Name];
                                                var navPropList = navContainer as IList;
                                                if (navPropList.Count > 0)
                                                {
                                                    IList newNavProp = Activator.CreateInstance(navContainer.GetType()) as IList;
                                                    navPropList.Cast<_EntityBase>().ToList().ForEach(entity =>
                                                    {
                                                        if (!entity.Equals(itemToRemove))
                                                        {
                                                            newNavProp.Add(entity);
                                                        }
                                                    });

                                                    parentEntity.NavigationPropertyDataDic.TryUpdate(inf.Key.PropertyInfo.Name, newNavProp, navContainer);
                                                    parentEntity.CallNavigationPropertyChangedByCache(parentEntity, inf.Key.PropertyInfo.Name);
                                                }
                                                // (en.NavigationPropertyDataDic[inf.Key.PropertyInfo.Name] as IList).Remove(itemToRemove);
                                            }
                                            else
                                            {
                                                //en[inf.Key.PropertyInfo.Name] = null;
                                                object tmp;
                                                parentEntity.NavigationPropertyDataDic.TryRemove(inf.Key.PropertyInfo.Name, out tmp);
                                                parentEntity.CallNavigationPropertyChangedByCache(parentEntity, inf.Key.PropertyInfo.Name);
                                            }
                                        }
                                        else
                                        {
                                            parentEntity.CallNavigationPropertyChangedByCache(parentEntity, inf.Key.PropertyInfo.Name);
                                        }
                                    });
                                }
                            }
                        }



                        cacheInfo.MaxTimeStampUintForDeletedRecord = cacheInfo.MaxTimeStampUintForDeletedRecord2;
                        cacheInfo.MaxTimeStampUintForDeletedRecord2 = maxTimeStampDeletedRecords;
                        cacheInfo.MaxTimeStampForDeletedRecord = cacheInfo.MaxTimeStampForDeletedRecord2;
                        cacheInfo.MaxTimeStampForDeletedRecord2 = maxDeletedRecordItem.TimeStamp;
                    }

                }


                if (nlst.Count > 0)
                {
                    entitiesForAddition = new List<_EntityBase>();
                    var newLst = nlst.Cast<_EntityBase>().ToList();
                    var oldLst = (oldData as IList);
                    var oldEntityBaseLst = (oldData as IList).Cast<_EntityBase>().ToList();
                    var result = Activator.CreateInstance<T>() as IList;
                    foreach (var item in oldEntityBaseLst)
                    {
                        result.Add(item);
                    }

                    newLst.ForEach(newItem =>
                    {
                        var oldItem = oldEntityBaseLst.FirstOrDefault(item => item.Equals(newItem));
                        if (oldItem != null)
                        {
                            UpdateNavigationProperties(cacheInfo, newItem, oldItem);
                            oldItem.UpdateAllProps(newItem);
                        }
                        else
                        {
                            entitiesForAddition.Add(newItem);
                            result.Add(newItem);
                            UpdateNavigationProperties(cacheInfo, newItem, null);

                            // newItem.EnableFillNavigationProperyByCache = true;
                        }
                    });

                    resultList = (T)result;
                }
            }

            if (entitiesForAddition != null && entitiesForAddition.Count > 0)
            {
                cacheInfo.CallOnAddEntities(entitiesForAddition);
            }

            if (entitiesForDeletion != null && entitiesForDeletion.Count > 0)
            {
                cacheInfo.CallOnDeleteEntities(entitiesForDeletion);
            }

            if ((cacheInfo.EnableUseCacheServer && ConfigHelper.GetConfigValue<bool>("IsCacheServer")) || !cacheInfo.EnableUseCacheServer)
            {

                if (!string.IsNullOrEmpty(cacheInfo.NameOfNavigationPropsForFetchingOnlyChangedDataFromDB) || (cacheInfo.EnableSaveCacheOnHDD && cacheInfo.NotYetGetCacheData))
                {
                    QueryableCacheDataProvider<object>.CalcAllTimeStampAndSet(newData as IList, cacheInfo, isQueryableCache);
                }
                else
                {
                    cacheInfo.MaxTimeStamp = cacheInfo.MaxTimeStampCopy;
                    cacheInfo.MaxTimeStampUint = cacheInfo.MaxTimeStampUintCopy;
                }
            }

            if (nlst.Count > 0)
            {
                foreach (var en in nlst.Cast<_EntityBase>())
                {
                    (en as _EntityBase).EnableFillNavigationProperyByCache();
                }
            }

            // System.Diagnostics.Debug.WriteLine("after merg:" + (oldData as IList).Count);
            return resultList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheInfo"></param>
        /// <param name="newCacheItem"></param>
        /// <param name="mustDeleteCacheItemFromNavigationProperty">Action in this method could be add or remove from navigationProperty.</param>
        /// <param name="isCacheItemUpdated">cacheItem could be in add or update mode.</param>
        private static void UpdateNavigationProperties(CacheInfo cacheInfo, _EntityBase newCacheItem, _EntityBase oldCacheItem)
        {
            List<KeyValuePair<InfoForFillingNavigationProperty, ConcurrentBag<_EntityBase>>> infoAndEntityListForFillingNavigationPropertyDic;
            infoAndEntityListForFillingNavigationPropertyDic = cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic.ToList();
            foreach (var info in infoAndEntityListForFillingNavigationPropertyDic)
            {

                /// attention: Added Tolist Here
                /// 
                if (oldCacheItem != null)
                {
                    if (info.Key.IsEnumerable &&
                      ((newCacheItem[info.Key.OtherEntityRefrencePropertyName] == null && oldCacheItem[info.Key.OtherEntityRefrencePropertyName] != null) ||
                        (newCacheItem[info.Key.OtherEntityRefrencePropertyName] != null &&
                        !newCacheItem[info.Key.OtherEntityRefrencePropertyName].Equals(oldCacheItem[info.Key.OtherEntityRefrencePropertyName]))))
                    {
                        UpdateNavigationPropertiesByInfo(newCacheItem, false, info);
                        UpdateNavigationPropertiesByInfo(oldCacheItem, true, info);
                    }
                }
                else
                {
                    UpdateNavigationPropertiesByInfo(newCacheItem, false, info);
                }

                ///TODO: Event for changing navigation property
                //  if (parentEntities.Count > 0)
                //     parentEntities.First().CallPropertyChangedByCache(parentEntities, inf.Key.PropertyInfo.Name);
            }
        }

        private static void UpdateNavigationPropertiesByInfo(_EntityBase cacheItem, bool mustDeleteCacheItemFromNavigationProperty, KeyValuePair<InfoForFillingNavigationProperty, ConcurrentBag<_EntityBase>> info)
        {
            List<_EntityBase> parentEntities = null;
            parentEntities = info.Value.
                            Where(en =>
                            {
                                var thisPropertyValue = en[info.Key.ThisEntityRefrencePropertyName];
                                return thisPropertyValue != null && thisPropertyValue.Equals(cacheItem[info.Key.OtherEntityRefrencePropertyName]) && !en.IsDeletedForCache;
                            }).ToList();

            parentEntities.ForEach(parentEntity =>
            {
                object navContainer;
                if (parentEntity.NavigationPropertyDataDic.TryGetValue(info.Key.PropertyInfo.Name, out navContainer))
                {
                    if (info.Key.IsEnumerable)
                    {
                        var navPropList = navContainer as IList;
                        IList newNavProp = Activator.CreateInstance(navContainer.GetType()) as IList;
                        navPropList.Cast<_EntityBase>().ToList().ForEach(entity =>
                        {
                            if (!entity.Equals(cacheItem))
                            {
                                newNavProp.Add(entity);
                            }
                        });

                        if (!mustDeleteCacheItemFromNavigationProperty)
                            newNavProp.Add(cacheItem);
                        parentEntity.NavigationPropertyDataDic.TryUpdate(info.Key.PropertyInfo.Name, newNavProp, navContainer);
                        parentEntity.CallNavigationPropertyChangedByCache(parentEntity, info.Key.PropertyInfo.Name);
                    }
                    else
                    {
                        parentEntity.NavigationPropertyDataDic.TryUpdate(info.Key.PropertyInfo.Name, cacheItem, navContainer);
                        parentEntity.CallNavigationPropertyChangedByCache(parentEntity, info.Key.PropertyInfo.Name);
                    }
                }
                else
                {
                    parentEntity.CallNavigationPropertyChangedByCache(parentEntity, info.Key.PropertyInfo.Name);
                }
                ///TODO: Event for changing navigation property
                // object changedEntity = parentEntity;
                //   parentEntity.OnColumnChanging(inf.Key.PropertyInfo.Name, ref changedEntity);                                        

            });
        }

        public static T RefreshCache<T>(ICacheDataProvider<T> cacheExecution, CacheInfo cacheInfo)
        {
            try
            {
                cacheInfo.CountOfWaitingThreads++;
                if (cacheInfo.CountOfWaitingThreads < 3)
                {
                    lock (cacheInfo)
                    {
                        var result = default(T);
                        var refreshCacheValue = default(T);
                        CacheService.TryGetCache<T>(cacheExecution.GenerateCacheKey(), out result);
                        var oldCacheValue = result;
                        RefreshCache_Force(cacheExecution, cacheInfo, oldCacheValue, out refreshCacheValue);
                        return refreshCacheValue;
                    }
                }
                else
                {
                    var result = default(T);
                    CacheService.TryGetCache<T>(cacheExecution.GenerateCacheKey(), out result);
                    return result;
                }
            }
            finally
            {
                cacheInfo.CountOfWaitingThreads--;
            }

        }


        private static bool TryRefreshCache<T>(ICacheDataProvider<T> cacheExecution, CacheInfo cacheInfo, out T refreshCacheValue)
        {
            try
            {
                refreshCacheValue = RefreshCache<T>(cacheExecution, cacheInfo);
                return true;
            }
            catch
            {
                refreshCacheValue = default(T);
                return false;
            }
        }


        private static void RefreshCache_Force<T>(ICacheDataProvider<T> cacheExecution, CacheInfo cacheInfo, T oldCacheValue, out T refreshCacheValue)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var cacheKey = cacheExecution.GenerateCacheKey();
            var cacheKeyFake = cacheKey + "_Fake";
            try
            {

                refreshCacheValue = cacheExecution.GetFreshData();
                lock (cacheInfo.InfoAndEntityListForFillingNavigationPropertyDic)
                {
                    if (cacheInfo.EnableToFetchOnlyChangedDataFromDB)
                    {
                        refreshCacheValue = MergeFreshDataByOldCache<T>(oldCacheValue, refreshCacheValue, cacheInfo, true);

                    }

                    CacheService.SetCache<T>(cacheExecution.GenerateCacheKey(), refreshCacheValue, cacheInfo.AutoRefreshInterval * (double)100000);
                    cacheInfo.NotYetGetCacheData = false;
                }

                cacheInfo.FrequencyOfBuilding += 1;
                cacheInfo.LastBuildDateTime = DateTime.Now;
                stopwatch.Stop();
                var time = new TimeSpan(stopwatch.ElapsedTicks);
                cacheInfo.BuildingTime += time;

            }
            catch (Exception ex)
            {
                cacheInfo.ErrorCount = cacheInfo.ErrorCount + 1;
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                //System.Diagnostics.StackFrame[] frames = st.GetFrames();
                //string x = "";
                //// Iterate over the frames extracting the information you need
                //foreach (System.Diagnostics.StackFrame frame in frames)
                //{
                //    //   x = ""+ frame.GetFileName()+"";
                //    x += "filename:" + frame.GetFileName() + "--methodname:" + frame.GetMethod().Name + "--linenumber:" + frame.GetFileLineNumber() + "--columnnumber:" + frame.GetFileColumnNumber();
                //}
                CacheService.RemoveCache(cacheKeyFake);
                //var eLog = _logService.GetEventLogObj();
                //eLog.OccuredException = ex;
                //eLog.UserId = "dbContext Cache in Thread !";
                //eLog.CustomMessage = x;
                //_logService.Handle(eLog);

                refreshCacheValue = default(T);
                throw _logService.Handle(ex, "dbContext Cache in Thread !");
            }
        }
    }
}

