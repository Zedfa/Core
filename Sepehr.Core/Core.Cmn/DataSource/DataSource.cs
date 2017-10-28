using Core.Cmn.Exceptions;
using Core.Cmn.Extensions;
using Core.Cmn.Monitoring;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Cmn.DataSource
{
    public enum AndOrOperator
    {
        And,
        Or
    }
    public enum RelationalOperator
    {
        Equal,
        Unequal,
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo
    }
    public class Comparer<T> : IComparer<T> where T : _EntityBase
    {
        private Func<T, T, int> _func;
        /// <summary>
        /// PropertPathes for every Indexed grouped
        /// </summary>
        public string[][] PropertyPathesForGrouped { get; set; }
        public string[] PropertyPathes { get; set; }
        public Comparer(Func<T, T, int> func)
        {
            _func = func;
        }

        public int Compare(T x, T y)
        {
            return _func(x, y);
        }
    }
    public class DataSource : IQueryable
    {
        public Type ElementType
        {
            get
            {
                return Source.ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return Source.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return Source.Provider;
            }
        }

        public IQueryable Source { get; set; }

        public IEnumerator GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }

    public class DataSource<T> : DataSourceQueryable<T> where T : _EntityBase
    {
        private static Dictionary<Type, int> LastDataSourceIdPerType = new Dictionary<Type, int>();
        protected FirstFilterClause FilterClause;
        public DataSource(List<T> dataSourceList)
        {
            FilterValue = new Dictionary<Guid, FilterClause>();
            DataSourceList = dataSourceList.ToList();
            BuildAllIndexes(false);
        }
        public DataSource(List<T> dataSourceList, bool writeRecordIndexInEntity)
        {
            if (writeRecordIndexInEntity)
            {
                GenerateIdPerType();
            }

            FilterValue = new Dictionary<Guid, FilterClause>();
            DataSourceList = dataSourceList.ToList();
            lock (EntityInfo)
            {
                BuildAllIndexes(writeRecordIndexInEntity);
            }
        }

        private void GenerateIdPerType()
        {
            lock (LastDataSourceIdPerType)
            {
                var type = typeof(T);
                var lastId = 0;
                if (LastDataSourceIdPerType.TryGetValue(type, out lastId))
                    lastId++;
                LastDataSourceIdPerType[type] = lastId;
                IdPerType = lastId;
            }
        }

        protected DataSource(DataSource<T> dataSourceToCopy, FilterClause filter)
        {
            // var w = new WatchElapsed();
            //  using (new Watch(out w))
            //    {
            int i = 0;
            Core.Serialization.ObjectMetaData.GetEntityMetaData(typeof(DataSource<T>)).ReflectionEmitPropertyAccessor.EmittedWritablePropertyGetters.ToList().ForEach(item =>
            {
                Core.Serialization.ObjectMetaData.GetEntityMetaData(typeof(DataSource<T>)).ReflectionEmitPropertyAccessor.EmittedWritablePropertySetters[i](this, item(dataSourceToCopy));
                i++;
            });

            if (dataSourceToCopy.FilterClause == null)
            {
                this.FilterClause = filter;
            }
            else
            {
                this.FilterClause = ((FilterClause)dataSourceToCopy.FilterClause).Copy();
                if (this.FilterClause.Clauses == null)
                    this.FilterClause.Clauses = new List<Cmn.DataSource.FilterClause>();
                this.FilterClause.Clauses.Add(filter);
            }

            //    }
            //   System.Diagnostics.Debug.WriteLine("Reinit DataSource:" + w.TotalElapsedMilliseconds);
        }

        public EntityInfo _entityInfo { get; set; }
        public Comparer<T>[] AllComparer { get; set; }

        public List<T>[] AllSingleIndexes { get; set; }
        public List<T>[] AllGroupedIndexes { get; set; }
        public EntityInfo EntityInfo
        {
            get
            {
                if (_entityInfo == null)
                {
                    _entityInfo = Core.Cmn.EntityInfo.GetEntityInfo(typeof(T));
                }
                return _entityInfo;
            }
        }

        public object FilterValue { get; set; }
        public List<T> CurrentIndex { get; private set; }
        public List<FirstFilterClause> CurrentOrderedFiltering { get; private set; }
        public int IdPerType { get; private set; }

        public static IEnumerable<T> BinarySearchForDuplicates(List<T> objectsToSearch, List<FirstFilterClause> filters)
        {
            if (objectsToSearch == null)
                yield break;

            int low = 0, high = objectsToSearch.Count - 1;
            // get the start index of target number
            int startIndex = -1;
            while (low <= high)
            {
                int mid = (high - low) / 2 + low;
                var source = objectsToSearch[mid];
                var compareResult = CompareAll(source, filters);
                if (compareResult == 1)
                {
                    if (high == mid)
                        break;
                    high = mid;
                }
                else if (compareResult == 0)
                {
                    if (high == mid)
                        break;
                    startIndex = mid;
                    high = mid;

                }
                else
                {
                    if (low == mid)
                        break;
                    low = mid;
                }
            }

            // get the end index of target number
            int endIndex = -1;
            low = 0;
            high = objectsToSearch.Count - 1;
            while (low <= high)
            {
                int mid = (high - low) / 2 + low;
                var source = objectsToSearch[mid];
                int compareResult = CompareAll(source, filters);
                if (compareResult == 1)
                {
                    if (high == mid)
                        break;
                    high = mid;
                }
                else if (compareResult == 0)
                {
                    if (low == mid)
                        break;
                    endIndex = mid;
                    low = mid;
                }
                else
                {
                    if (low == mid)
                        break;
                    low = mid;
                }
            }

            if (startIndex != -1 && endIndex != -1)
            {
                for (int i = 0; i + startIndex <= endIndex; i++)
                {
                    yield return objectsToSearch[i + startIndex];
                }
            }
        }

        private static int CompareAll(T source, List<FirstFilterClause> filters)
        {
            int compareResult = 0;
            foreach (var filter in filters)
            {
                compareResult = Compare(source, filter.Value, filter.PropertyPathes);
                if (compareResult != 0)
                    break;
            }

            return compareResult;
        }

        public IEnumerable<T> All()
        {
            if (CurrentIndex == null)
            {
                var valuesForFiltering = new List<FirstFilterClause>();
                CurrentIndex = FindBestIndexToBinarySearch(out valuesForFiltering);
                CurrentOrderedFiltering = valuesForFiltering;
            }

            return BinarySearchForDuplicates(CurrentIndex, CurrentOrderedFiltering);
        }

        public DataSource<T> ApplyFilter<R>(Expression<Func<T, R>> propertyName, R valueToFilter)
        {
            FilterClause filter = CreateFilterClause(propertyName, valueToFilter);
            var ds = new DataSource<T>(this, filter);

            return ds;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            if (CurrentIndex == null)
            {
                var valuesForFiltering = new List<FirstFilterClause>();
                CurrentIndex = FindBestIndexToBinarySearch(out valuesForFiltering);
                CurrentOrderedFiltering = valuesForFiltering;
            }
            var result = BinarySearchForDuplicates(CurrentIndex, CurrentOrderedFiltering).First();
            return result;
        }

        public T FirstOrDefault()
        {
            if (CurrentIndex == null)
            {
                var valuesForFiltering = new List<FirstFilterClause>();
                CurrentIndex = FindBestIndexToBinarySearch(out valuesForFiltering);
                CurrentOrderedFiltering = valuesForFiltering;
            }
            var result = BinarySearchForDuplicates(CurrentIndex, CurrentOrderedFiltering).FirstOrDefault();
            return result;
        }

        private List<T> FindBestIndexToBinarySearch(out List<FirstFilterClause> orderedFilters)
        {
            List<T> resultIndex = null;
            var allFilters = new List<FirstFilterClause>();
            orderedFilters = new List<FirstFilterClause>();
            allFilters.Add(FilterClause);
            if (FilterClause.Clauses != null)
                foreach (var f in FilterClause.Clauses)
                {
                    allFilters.Add(f);
                }

            if (allFilters.Count == 0)
            {
                resultIndex = this.DataSourceList;
            }
            else
            {
                if (allFilters.Count == 1)
                {
                    int i;
                    if (!EntityInfo.IndexableProps.TryGetValue(FilterClause.PropertyPath, out i))
                    {
                        var groupedIndex = EntityInfo.IndexableGroupedProps.FirstOrDefault(index => index.FirstPropertyPath == FilterClause.PropertyPath);
                        if (groupedIndex == null)
                        {
                            resultIndex = FindGroupedIndex(ref orderedFilters, allFilters);
                        }
                        else
                        {
                            resultIndex = this.AllGroupedIndexes[groupedIndex.IndexOrder];
                        }
                    }
                    else
                    {
                        resultIndex = this.AllSingleIndexes[i];
                    }

                    orderedFilters.Add(allFilters[0]);
                }
                else
                {
                    resultIndex = FindGroupedIndex(ref orderedFilters, allFilters);
                }
            }
            return resultIndex;
        }

        private List<T> FindGroupedIndex(ref List<FirstFilterClause> orderedFilters, List<FirstFilterClause> allFilters)
        {
            List<T> resultIndex;
            GroupedIndexablePropertyData groupedIndex = null;
            foreach (var g in EntityInfo.IndexableGroupedProps)
            {
                var firstIndexCriteria = allFilters.FirstOrDefault(filter => filter.PropertyPath == g.FirstPropertyPath);
                orderedFilters = new List<FirstFilterClause>();
                if (firstIndexCriteria == null)
                    continue;
                var filters = allFilters.ToList();
                filters.Remove(firstIndexCriteria);
                orderedFilters.Add(firstIndexCriteria);
                foreach (var idx in g.Indexes.Skip(1).ToList())
                {
                    if (filters.Count == 0) break;
                    var filter = allFilters.FirstOrDefault(f => f.PropertyPath == idx.NavigationPath);
                    if (filter != null)
                    {
                        filters.Remove(filter);
                        orderedFilters.Add(filter);
                    }
                };

                if (filters.Count == 0)
                {
                    groupedIndex = g;
                    break;
                }
            }

            if (groupedIndex == null)
            {
                //remark: maloom nist kodoum filter baese khata boude => FilterClause.PropertyPath
                string strProperties = string.Empty;
                orderedFilters.ForEach(filter => strProperties += string.Concat("PropertyPath : ", filter.PropertyPath, " ,"));
                throw new InvalidFilterOnIndexablePropertyException(EntityInfo.EntityType.Name, FilterClause.PropertyPath);
            }

            resultIndex = AllGroupedIndexes[groupedIndex.IndexOrder];
            return resultIndex;
        }

        private static int Compare(T source, object target, string[] propertyPath)
        {
            IComparable src = GetPropValue(source, propertyPath);
            if (src == null && target == null) return 0;
            if (src == null) return -1;
            if (target == null) return 1;
            return src.CompareTo(target);
        }
        private static FilterClause CreateFilterClause<R>(Expression<Func<T, R>> propertyName, R valueToFilter)
        {
            var propertyPath = (propertyName.Body as MemberExpression).ToString();
            var propertyPathes = propertyPath.Split('.');
            FilterClause filter = new Cmn.DataSource.FilterClause();
            filter.PropertyPathes = propertyPathes.Skip(1).ToArray();
            filter.PropertyPath = propertyPath.Replace(propertyPathes[0] + ".", "");
            filter.RelationalOperator = RelationalOperator.Equal;
            filter.Value = valueToFilter;
            return filter;
        }
        private static IComparable GetPropValue(T t1, string[] propertyPath)
        {
            object obj = t1;
            foreach (var prop in propertyPath)
            {
                if (obj == null) return null;
                var metadata = Core.Serialization.ObjectMetaData.GetEntityMetaData(obj.GetType());
                obj = metadata.ReflectionEmitPropertyAccessor.EmittedAllPropertyGetters[prop](obj);
            }
            return (IComparable)obj;
        }

        private void BuildAllIndexes(bool writeRecordIndexInEntity)
        {
            if (EntityInfo.IndexableProps.Count > 0 || EntityInfo.IndexableGroupedProps.Count > 0)
            {
                AllSingleIndexes = new List<T>[EntityInfo.IndexableProps.Count];
                AllComparer = new Comparer<T>[EntityInfo.IndexableProps.Count];
                BuildSingleIndexes(writeRecordIndexInEntity);
                BuildGroupedIndexes(writeRecordIndexInEntity);
            }
        }

        private void BuildGroupedIndexes(bool writeRecordIndexInEntity)
        {
            AllGroupedIndexes = new List<T>[EntityInfo.IndexableGroupedProps.Count];
            foreach (var gIndex in EntityInfo.IndexableGroupedProps)
            {
                //   var propertyPathes = item.FirstPropertyPath.Split('.');
                string[][] propertyPathes = new string[gIndex.Indexes.Count][];
                IndexablePropertyData indexablePropertyData = null;
                try
                {
                    foreach (var index in gIndex.Indexes)
                    {
                        propertyPathes[index.IndexOrder] = index.NavigationPath.Split('.');
                        indexablePropertyData = index;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException($"IndexOrder for 'IndexablePropertyAttribute' in your '{typeof(T).Name}' Entity and '{indexablePropertyData.NavigationPath}' Property by GroupName '{indexablePropertyData.GroupName}' is out of range.");
                }
                var comparer1 = new Comparer<T>((Func<T, T, int>)((t1, t2) =>
                {
                    return CompareGroupedProperties(t1, t2, propertyPathes);
                }))
                {
                    PropertyPathesForGrouped = propertyPathes

                };
                var newIndex = DataSourceList.ToList();
                newIndex.Sort(comparer1);
                if (writeRecordIndexInEntity)
                    WriteRecordIndexForGroupedIndex(gIndex.IndexOrder, newIndex);
                AllGroupedIndexes[gIndex.IndexOrder] = newIndex;
            };
        }

        private static int CompareGroupedProperties(T t1, T t2, string[][] propertyPathes)
        {
            var comparisonResult = 0;
            foreach (var propertyPath in propertyPathes)
            {
                if (comparisonResult == 0)
                {
                    comparisonResult = Compare(t1, GetPropValue(t2, propertyPath), propertyPath);
                }
                else
                    return comparisonResult;
            }
            return comparisonResult;
        }

        private void BuildSingleIndexes(bool writeRecordIndexInEntity)
        {
            foreach (var index in EntityInfo.IndexableProps)
            {
                var propertyPathes = index.Key.Split('.');

                var comparer1 = new Comparer<T>((Func<T, T, int>)((t1, t2) =>
                {
                    return Compare(t1, GetPropValue(t2, propertyPathes), propertyPathes);
                }));
                //AllComparer[item.Value] = comparer;
                var newIndex = DataSourceList.ToList();
                newIndex.Sort(comparer1);
                if (writeRecordIndexInEntity)
                    WriteRecordIndexForSingleIndex(index.Value, newIndex);

                AllSingleIndexes[index.Value] = newIndex;
            }
        }

        private void WriteRecordIndexForSingleIndex(int index, List<T> newIndex)
        {
            if (typeof(_EntityBase).IsAssignableFrom(typeof(T)))
            {
                var i = 0;
                newIndex.ForEach(obj =>
                {
                    if (((_EntityBase)obj).IndexOfIndexableProperties == null)
                    {
                        ((_EntityBase)obj).IndexOfIndexableProperties = new System.Collections.Concurrent.ConcurrentDictionary<int, int[]>();
                    }

                    int[] indexRecordsId;
                    if (!((_EntityBase)obj).IndexOfIndexableProperties.TryGetValue(this.IdPerType, out indexRecordsId))
                    {
                        indexRecordsId = new int[EntityInfo.IndexableProps.Count];
                        ((_EntityBase)obj).IndexOfIndexableProperties.TryAdd(this.IdPerType, indexRecordsId);
                    }

                    indexRecordsId[index] = i;
                    i++;
                });
            }
        }
        private void WriteRecordIndexForGroupedIndex(int index, List<T> newIndex)
        {
            if (typeof(_EntityBase).IsAssignableFrom(typeof(T)))
            {
                var i = 0;
                newIndex.ForEach(obj =>
                {
                    if (((_EntityBase)obj).IndexOfGroupedIndexableProperties == null)
                    {
                        ((_EntityBase)obj).IndexOfGroupedIndexableProperties = new System.Collections.Concurrent.ConcurrentDictionary<int, int[]>();
                    }

                    int[] indexRecordsId;
                    if (!((_EntityBase)obj).IndexOfGroupedIndexableProperties.TryGetValue(this.IdPerType, out indexRecordsId))
                    {
                        indexRecordsId = new int[EntityInfo.IndexableGroupedProps.Count];
                        ((_EntityBase)obj).IndexOfGroupedIndexableProperties.TryAdd(this.IdPerType, indexRecordsId);
                    }

                    indexRecordsId[index] = i;
                    i++;
                });
            }
        }
    }

    public class DataSourceQueryable<T> : DataSource, IQueryable<T>, IList<T>
    {
        public int Count
        {
            get
            {
                return DataSourceList.Count();
            }
        }

        public List<T> DataSourceList { get; set; }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)DataSourceList).IsReadOnly;
            }
        }

        public T this[int index]
        {
            get
            {
                return DataSourceList[index];
            }

            set
            {
                DataSourceList[index] = value;
            }
        }

        public void Add(T item)
        {
            DataSourceList.Add(item);
        }
        public void Clear()
        {
            DataSourceList.Clear();
        }

        public bool Contains(T item)
        {
            return DataSourceList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            DataSourceList.CopyTo(array, arrayIndex);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return DataSourceList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return DataSourceList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            DataSourceList.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return DataSourceList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            DataSourceList.RemoveAt(index);
        }
    }
    public class FilterClause : FirstFilterClause
    {
        public AndOrOperator AndOrOperator { get; set; }
        public override FirstFilterClause Copy()
        {
            List<FilterClause> clauses = null;
            if (this.Clauses != null)
            {
                clauses = new List<Cmn.DataSource.FilterClause>();
                this.Clauses.ForEach(item => clauses.Add((FilterClause)item.Copy()));
            }
            return new FilterClause
            {
                Id = this.Id,
                Clauses = clauses,
                PropertyPath = this.PropertyPath,
                PropertyPathes = this.PropertyPathes,
                RelationalOperator = this.RelationalOperator,
                Value = this.Value,
                AndOrOperator = this.AndOrOperator
            };
        }
    }

    public class FirstFilterClause
    {
        public Guid Id = Guid.NewGuid();
        public List<FilterClause> Clauses { get; set; }
        public string PropertyPath { get; set; }
        public string[] PropertyPathes { get; set; }
        public RelationalOperator RelationalOperator { get; set; }
        public object Value { get; set; }
        public virtual FirstFilterClause Copy()
        {
            List<FilterClause> clauses = null;
            if (this.Clauses != null)
            {
                clauses = new List<Cmn.DataSource.FilterClause>();
                this.Clauses.ForEach(item => clauses.Add((FilterClause)item.Copy()));
            }
            return new FirstFilterClause
            {
                Id = this.Id,
                Clauses = clauses,
                PropertyPath = this.PropertyPath,
                PropertyPathes = this.PropertyPathes,
                RelationalOperator = this.RelationalOperator,
                Value = this.Value,
            };
        }
    }
}