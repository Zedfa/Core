namespace Kendo.Mvc.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Kendo.Mvc;
    using Kendo.Mvc.Infrastructure;
    using Kendo.Mvc.Infrastructure.Implementation;
    using Infrastructure.Implementation.Expressions;
    using Kendo.Mvc.UI;
    using Core.Cmn.FarsiUtils;
    using Core.Cmn.Extensions;


    /// <summary>
    /// Provides extension methods to process DataSourceRequest.
    /// </summary>
    public static class QueryableExtensions
    {
        private static string _filterSeperator = "|";

        private static DataSourceResult ToDataSourceResult(this DataTableWrapper enumerable, DataSourceRequest request)
        {
            var filters = new List<IFilterDescriptor>();

            if (request.Filters != null)
            {
                filters.AddRange(request.Filters);
            }

            if (filters.Any())
            {
                var dataTable = enumerable.Table;
                filters.SelectMemberDescriptors()
                    .Each(f => f.MemberType = GetFieldByTypeFromDataColumn(dataTable, f.Member));
            }

            var group = new List<GroupDescriptor>();

            if (request.Groups != null)
            {
                group.AddRange(request.Groups);
            }

            if (group.Any())
            {
                var dataTable = enumerable.Table;
                group.Each(g => g.MemberType = GetFieldByTypeFromDataColumn(dataTable, g.Member));
            }

            var result = enumerable.AsEnumerable().ToDataSourceResult(request);
            result.Data = result.Data.SerializeToDictionary(enumerable.Table);

            return result;
        }

        private static Type GetFieldByTypeFromDataColumn(DataTable dataTable, string memberName)
        {
            return dataTable.Columns.Contains(memberName) ? dataTable.Columns[memberName].DataType : null;
        }

        public static DataSourceResult ToDataSourceResult(this DataTable dataTable, DataSourceRequest request)
        {
            return dataTable.WrapAsEnumerable().ToDataSourceResult(request);
        }

        public static DataSourceResult ToDataSourceResult(this IEnumerable enumerable, DataSourceRequest request)
        {
            return enumerable.AsQueryable().ToDataSourceResult(request);
        }

        public static DataSourceResult ToDataSourceResult(this IEnumerable enumerable, DataSourceRequest request, ModelStateDictionary modelState)
        {
            return enumerable.AsQueryable().ToDataSourceResult(request, modelState);
        }

        public static DataSourceResult ToDataSourceResult(this IQueryable enumerable, DataSourceRequest request)
        {
            return enumerable.ToDataSourceResult(request, null);
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IEnumerable<TModel> enumerable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return enumerable.AsQueryable().CreateDataSourceResult(request, null, selector);
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IEnumerable<TModel> enumerable, DataSourceRequest request, ModelStateDictionary modelState, Func<TModel, TResult> selector)
        {
            return enumerable.AsQueryable().CreateDataSourceResult(request, modelState, selector);
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IQueryable<TModel> enumerable, DataSourceRequest request, Func<TModel, TResult> selector)
        {
            return enumerable.CreateDataSourceResult(request, null, selector);
        }

        public static DataSourceResult ToDataSourceResult<TModel, TResult>(this IQueryable<TModel> enumerable, DataSourceRequest request, ModelStateDictionary modelState, Func<TModel, TResult> selector)
        {
            return enumerable.CreateDataSourceResult(request, modelState, selector);
        }

        public static DataSourceResult ToDataSourceResult(this IQueryable queryable, DataSourceRequest request, ModelStateDictionary modelState)
        {
            
            return queryable.CreateDataSourceResult<object, object>(request, modelState, null);
        }

        private static DataSourceResult CreateDataSourceResult<TModel, TResult>(this IQueryable queryable, DataSourceRequest request, ModelStateDictionary modelState, Func<TModel, TResult> selector)
        {
            var result = new DataSourceResult();

            var data = queryable;    

            var filters = new List<IFilterDescriptor>();

            if (request.Filters != null)
            {
                RefineFilterValues(request.Filters);
                filters.AddRange(request.Filters);
            }

            if (filters.Any())
            {
                data = data.Where(filters);
            }

            var sort = new List<SortDescriptor>();

            if (request.Sorts != null)
            {
                sort.AddRange(request.Sorts);
            }

            var temporarySortDescriptors = new List<SortDescriptor>();

            IList<GroupDescriptor> groups = new List<GroupDescriptor>();

            if (request.Groups != null)
            {
                groups.AddRange(request.Groups);
            }

            var aggregates = new List<AggregateDescriptor>();

            if (request.Aggregates != null)
            {
                aggregates.AddRange(request.Aggregates);
            }

            if (aggregates.Any())
            {
                var dataSource = data.AsQueryable();

                var source = dataSource;
                if (filters.Any())
                {
                    source = dataSource.Where(filters);
                }

                result.AggregateResults = source.Aggregate(aggregates.SelectMany(a => a.Aggregates));

                if (groups.Any() && aggregates.Any())
                {
                    groups.Each(g => g.AggregateFunctions.AddRange(aggregates.SelectMany(a => a.Aggregates)));
                }
            }

            result.Total = data.Count();

            if (!sort.Any() && queryable.Provider.IsEntityFrameworkProvider())
            {
                // The Entity Framework provider demands OrderBy before calling Skip.
                SortDescriptor sortDescriptor = new SortDescriptor
                {
                    Member = queryable.ElementType.FirstSortableProperty()
                };
                sort.Add(sortDescriptor);
                temporarySortDescriptors.Add(sortDescriptor);
            }

            if (groups.Any())
            {                
                groups.Reverse().Each(groupDescriptor =>
                {
                    var sortDescriptor = new SortDescriptor
                    {
                        Member = groupDescriptor.Member,
                        SortDirection = groupDescriptor.SortDirection
                    };

                    sort.Insert(0, sortDescriptor);
                    temporarySortDescriptors.Add(sortDescriptor);
                });
            }

            if (sort.Any())
            {
                data = data.Sort(sort);
            }

            var notPagedData = data;

            data = data.Page(request.Page - 1, request.PageSize);

            if (groups.Any())
            {
                data = data.GroupBy(notPagedData, groups);
            }           

            result.Data = data.Execute(selector);

            if (modelState != null && !modelState.IsValid)
            {
                result.Errors = modelState.SerializeErrors();
            }

            temporarySortDescriptors.Each(sortDescriptor => sort.Remove(sortDescriptor));            

            return result;
        }


        //----------custom filtering-----------------

        private static void RefineFilterValues(this IList<IFilterDescriptor> filters)
        {
            if (filters.Count > 0)
            {
                var mainFilterObj = filters.First();
                //mainFilterObj is FilterDescriptor
                if (mainFilterObj is FilterDescriptor)
                {
                    var result = AmendFilterValuesForSingleFilterDescriptor(mainFilterObj as FilterDescriptor);
                    if (result != null)
                    {
                        filters.RemoveAt(0);
                        filters.Add(result);
                    }
                }
                //mainFilterObj is CompositeFilterDescriptor
                else
                {
                    AmendFilterValuesForCompositeFilterDescriptor(mainFilterObj as CompositeFilterDescriptor);
                }
            }
        }

        private static void AmendFilterValuesForCompositeFilterDescriptor(CompositeFilterDescriptor compositeFilterDescriptor)
        {
            var filterConditions = compositeFilterDescriptor.FilterDescriptors;
            Dictionary<FilterDescriptor, CompositeFilterDescriptor> convertedSingleFilters = null;

            foreach (var filterItem in filterConditions)
            {
                
                if (filterItem is CompositeFilterDescriptor)
                {
                    
                    AmendFilterValuesForCompositeFilterDescriptor(filterItem as CompositeFilterDescriptor);
                }
                else
                {
                    var fItem = filterItem as FilterDescriptor;
                    var result = AmendFilterValuesForSingleFilterDescriptor(fItem);

                    if (result != null)
                    {
                        if (result is CompositeFilterDescriptor)
                        {
                            convertedSingleFilters = convertedSingleFilters ?? new Dictionary<FilterDescriptor, CompositeFilterDescriptor>();
                            convertedSingleFilters.Add(fItem, result as CompositeFilterDescriptor);
                        }

                        else
                        {
                            result = fItem;
                        }
                    }
                }
            }

            if (convertedSingleFilters != null)
            {
                foreach (var singleFilterItem in convertedSingleFilters.Keys)
                {
                    filterConditions[filterConditions.IndexOf(singleFilterItem)] = convertedSingleFilters[singleFilterItem];
                }
            }
        }


        private static IFilterDescriptor AmendFilterValuesForSingleFilterDescriptor(IFilterDescriptor filterDescriptor)
        {
            IFilterDescriptor retVal = null;
            var fItem = (filterDescriptor as FilterDescriptor);
            var filterValObj = fItem.Value;
            if (filterValObj!=null&& filterValObj.GetType() == typeof(string))
            {
                string filterVal = filterValObj.ToString().CorrectPersianChars();
                (filterDescriptor as FilterDescriptor).Value = filterVal;

                #region Geregorian Date
                if (filterVal.EndsWith($"{_filterSeperator}dt"))
                {
                    switch (fItem.Operator)
                    {
                        case FilterOperator.IsEqualTo:
                            retVal = getDateRangeCompositeFilterConditionForEquality(fItem, filterVal);
                            break;
                        case FilterOperator.IsGreaterThan:
                            getDateRangeCompositeFilterConditionForGreaterThan(fItem, filterVal);
                            break;
                        case FilterOperator.IsLessThan:
                            getDateRangeCompositeFilterConditionForLessThan(fItem, filterVal);
                            break;
                        case FilterOperator.IsNotEqualTo:
                            retVal = getDateRangeCompositeFilterConditionForInEquality(fItem, filterVal);
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region persian Date
                else if (filterVal.EndsWith($"{_filterSeperator}pdt"))
                {
                    switch (fItem.Operator)
                    {
                        case FilterOperator.IsEqualTo:
                            GetPersianDateRangeFilterForEquality(fItem, filterVal);
                            break;
                        case FilterOperator.IsGreaterThan:
                            GetPersianDateRangeFilterForGreaterThan(fItem, filterVal);
                            break;
                        case FilterOperator.IsLessThan:
                            GetPersianDateRangeFilterForLessThan(fItem, filterVal);
                            break;
                        case FilterOperator.IsNotEqualTo:
                            GePersiantDateRangeFilterForNotEquality(fItem, filterVal);
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region navigation property
                else if (filterVal.Contains($"{_filterSeperator}nv"))
                {
                    //TODO:
                    //1.Update both of "ConvertedValue" and "Value" Properties to the desired value from currently multi secion data of these fields.
                    //2.Update "Member" Property value.
                    retVal = new FilterDescriptor();
                    var newValue = fItem.ConvertedValue.ToString().Split(Convert.ToChar(_filterSeperator));
                    fItem.Member = newValue[1].Split(':')[1];
                    //fItem.ConvertedValue = newValue[0];
                    fItem.Value = newValue[0];
                    retVal = fItem;
                }
                #endregion 

                #region lookup and dropdownList
                else if (filterVal.Contains($"{_filterSeperator}lkp") || filterVal.Contains($"{_filterSeperator}ddl"))
                {
                    retVal = new FilterDescriptor();
                    var newValue = fItem.ConvertedValue.ToString().Split(Convert.ToChar(_filterSeperator));
                    fItem.Member = newValue[1].Split(':')[1];
                    //fItem.ConvertedValue = newValue[0];
                    fItem.Value = newValue[0];
                    retVal = fItem;
                }
                #endregion
                else
                {
                    retVal = filterDescriptor;
                }
            }

            else
            {
                retVal = filterDescriptor;

            }

            return retVal;
        }

        private static CompositeFilterDescriptor getDateRangeCompositeFilterConditionForInEquality(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar( _filterSeperator))[0] + " 00:00 Þ.Ù");
            //dateTimeValue.Date
            fItem.Operator = FilterOperator.IsLessThan;
            fItem.Value = dateTimeValue;
            var compositeFilter = new CompositeFilterDescriptor();
            var secondFilterRule = new FilterDescriptor();
            secondFilterRule.Operator = FilterOperator.IsGreaterThan;
            secondFilterRule.Value = ((DateTime)PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar(_filterSeperator))[0] + " 00:00 Þ.Ù")).Add(new TimeSpan(23, 59, 59));
            secondFilterRule.Member = fItem.Member;
            compositeFilter.LogicalOperator = FilterCompositionLogicalOperator.Or;
            compositeFilter.FilterDescriptors.Add(fItem);
            compositeFilter.FilterDescriptors.Add(secondFilterRule);
            return compositeFilter;
        }
        private static void GePersiantDateRangeFilterForNotEquality(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = filterVal.Split(Convert.ToChar(_filterSeperator))[0];//.AddDays(-1);
            fItem.Value = dateTimeValue;
        }

        private static void getDateRangeCompositeFilterConditionForLessThan(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar(_filterSeperator))[0] + " 00:00 Þ.Ù");//.AddDays(-1);
            fItem.Value = dateTimeValue;
        }
        private static void GetPersianDateRangeFilterForLessThan(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = filterVal.Split(Convert.ToChar(_filterSeperator))[0];//.AddDays(-1);
            fItem.Value = dateTimeValue;
        }

        private static void getDateRangeCompositeFilterConditionForGreaterThan(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar(_filterSeperator))[0] + " 00:00 Þ.Ù").Add(new TimeSpan(23, 59, 59));
            fItem.Value = dateTimeValue;

        }
        private static void GetPersianDateRangeFilterForGreaterThan(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = filterVal.Split(Convert.ToChar(_filterSeperator))[0];
            fItem.Value = dateTimeValue;

        }

        private static CompositeFilterDescriptor getDateRangeCompositeFilterConditionForEquality(FilterDescriptor fItem, string filterVal)
        {
            var dateTimeValue = PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar(_filterSeperator))[0] + " 00:00 Þ.Ù");
            fItem.Operator = FilterOperator.IsGreaterThanOrEqualTo;
            fItem.Value = dateTimeValue;
            var compositeFilter = new CompositeFilterDescriptor();
            var secondFilterRule = new FilterDescriptor();
            secondFilterRule.Operator = FilterOperator.IsLessThan;
            secondFilterRule.Value = ((DateTime)PersianDateConverter.ToGregorianDateTime(filterVal.Split(Convert.ToChar(_filterSeperator))[0] + " 00:00 Þ.Ù")).Add(new TimeSpan(23, 59, 59));
            //secondFilterRule.Value
            secondFilterRule.Member = fItem.Member;
            compositeFilter.LogicalOperator = FilterCompositionLogicalOperator.And;
            compositeFilter.FilterDescriptors.Add(fItem);
            compositeFilter.FilterDescriptors.Add(secondFilterRule);
            return compositeFilter;
        }

        private static void GetPersianDateRangeFilterForEquality(FilterDescriptor fItem, string filterVal)
        {
           
            var dateTimeValue = filterVal.Split(Convert.ToChar(_filterSeperator))[0];
            fItem.Value = dateTimeValue;

        }



        //-------------------------------



        private static IQueryable CallQueryableMethod(this IQueryable source, string methodName, LambdaExpression selector)
        {
            IQueryable query = source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { source.ElementType, selector.Body.Type },
                    source.Expression,
                    Expression.Quote(selector)));

            return query;
        }

        /// <summary>
        /// Sorts the elements of a sequence using the specified sort descriptors.
        /// </summary>
        /// <param name="source">A sequence of values to sort.</param>
        /// <param name="sortDescriptors">The sort descriptors used for sorting.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a <paramref name="sortDescriptors"/>.
        /// </returns>
        public static IQueryable Sort(this IQueryable source, IEnumerable<SortDescriptor> sortDescriptors)
        {
            var builder = new SortDescriptorCollectionExpressionBuilder(source, sortDescriptors);
            return builder.Sort();
        }

        /// <summary>
        /// Pages through the elements of a sequence until the specified 
        /// <paramref name="pageIndex"/> using <paramref name="pageSize"/>.
        /// </summary>
        /// <param name="source">A sequence of values to page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are at the specified <paramref name="pageIndex"/>.
        /// </returns>
        public static IQueryable Page(this IQueryable source, int pageIndex, int pageSize)
        {
            IQueryable query = source;

            if (pageIndex > 0)
            {
                query = query.Skip(pageIndex * pageSize);
            }

            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }

            return query;
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are the result of invoking a 
        /// projection selector on each element of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> A sequence of values to project. </param>
        /// <param name="selector"> A projection function to apply to each element. </param>
        public static IQueryable Select(this IQueryable source, LambdaExpression selector)
        {
            return source.CallQueryableMethod("Select", selector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <param name="source"> An <see cref="IQueryable" /> whose elements to group.</param>
        /// <param name="keySelector"> A function to extract the key for each element.</param>
        /// <returns>
        /// An <see cref="IQueryable"/> with <see cref="IGrouping{TKey,TElement}"/> items, 
        /// whose elements contains a sequence of objects and a key.
        /// </returns>
        public static IQueryable GroupBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("GroupBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderBy(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderBy", keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted in descending order according to a key.
        /// </returns>
        /// <param name="source">
        /// A sequence of values to order.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        public static IQueryable OrderByDescending(this IQueryable source, LambdaExpression keySelector)
        {
            return source.CallQueryableMethod("OrderByDescending", keySelector);
        }

        /// <summary>
        /// Calls <see cref="OrderBy(System.Linq.IQueryable,System.Linq.Expressions.LambdaExpression)"/> 
        /// or <see cref="OrderByDescending"/> depending on the <paramref name="sortDirection"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> whose elements are sorted according to a key.
        /// </returns>
        public static IQueryable OrderBy(this IQueryable source, LambdaExpression keySelector, ListSortDirection? sortDirection)
        {
            if (sortDirection.HasValue)
            {
                if (sortDirection.Value == ListSortDirection.Ascending)
                {
                    return source.OrderBy(keySelector);
                }

                return source.OrderByDescending(keySelector);
            }

            return source;
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified <paramref name="groupDescriptors"/>.
        /// </summary>
        /// <param name="source"> An <see cref="IQueryable" /> whose elements to group. </param>
        /// <param name="groupDescriptors">The group descriptors used for grouping.</param>
        /// <returns>
        /// An <see cref="IQueryable"/> with <see cref="IGroup"/> items, 
        /// whose elements contains a sequence of objects and a key.
        /// </returns>
        public static IQueryable GroupBy(this IQueryable source, IEnumerable<GroupDescriptor> groupDescriptors)
        {
            return source.GroupBy(source, groupDescriptors);
        }

        public static IQueryable GroupBy(this IQueryable source, IQueryable notPagedData, IEnumerable<GroupDescriptor> groupDescriptors)
        {
            var builder = new GroupDescriptorCollectionExpressionBuilder(source, groupDescriptors, notPagedData);
            builder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
            return builder.CreateQuery();
        }

        /// <summary>
        /// Calculates the results of given aggregates functions on a sequence of elements.
        /// </summary>
        /// <param name="source"> An <see cref="IQueryable" /> whose elements will 
        /// be used for aggregate calculation.</param>
        /// <param name="aggregateFunctions">The aggregate functions.</param>
        /// <returns>Collection of <see cref="AggregateResult"/>s calculated for each function.</returns>
        public static AggregateResultCollection Aggregate(this IQueryable source, IEnumerable<AggregateFunction> aggregateFunctions)
        {
            var functions = aggregateFunctions.ToList();

            if (functions.Count > 0)
            {
                var builder = new QueryableAggregatesExpressionBuilder(source, functions);
                builder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
                var groups = builder.CreateQuery();

                foreach (AggregateFunctionsGroup group in groups)
                {
                    return group.GetAggregateResults(functions);
                }
            }

            return new AggregateResultCollection();
        }

        /// <summary> 
        /// Filters a sequence of values based on a predicate. 
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements from the input sequence 
        /// that satisfy the condition specified by <paramref name="predicate" />.
        /// </returns>
        /// <param name="source"> An <see cref="IQueryable" /> to filter.</param>
        /// <param name="predicate"> A function to test each element for a condition.</param>
        public static IQueryable Where(this IQueryable source, Expression predicate)
        {
            return source.Provider.CreateQuery(
               Expression.Call(
                   typeof(Queryable),
                   "Where",
                   new[] { source.ElementType },
                   source.Expression,
                   Expression.Quote(predicate)));
        }

        /// <summary> 
        /// Filters a sequence of values based on a collection of <see cref="IFilterDescriptor"/>. 
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="filterDescriptors">The filter descriptors.</param>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements from the input sequence 
        /// that satisfy the conditions specified by each filter descriptor in <paramref name="filterDescriptors" />.
        /// </returns>
        public static IQueryable Where(this IQueryable source, IEnumerable<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                var parameterExpression = Expression.Parameter(source.ElementType, "item");

                var expressionBuilder = new FilterDescriptorCollectionExpressionBuilder(parameterExpression, filterDescriptors);
                expressionBuilder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
                var predicate = expressionBuilder.CreateFilterExpression();
                return source.Where(predicate);
            }

            return source;
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains the specified number 
        /// of elements from the start of <paramref name="source" />.
        /// </returns>
        /// <param name="source"> The sequence to return elements from.</param>
        /// <param name="count"> The number of elements to return. </param>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is null. </exception>
        public static IQueryable Take(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence 
        /// and then returns the remaining elements.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable" /> that contains elements that occur 
        /// after the specified index in the input sequence.
        /// </returns>
        /// <param name="source">
        /// An <see cref="IQueryable" /> to return elements from.
        /// </param>
        /// <param name="count">
        /// The number of elements to skip before returning the remaining elements.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static IQueryable Skip(this IQueryable source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant(count)));
        }

        /// <summary> Returns the number of elements in a sequence.</summary>
        /// <returns> The number of elements in the input sequence.</returns>
        /// <param name="source">
        /// The <see cref="IQueryable" /> that contains the elements to be counted.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        public static int Count(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute<int>(
                Expression.Call(
                    typeof(Queryable), "Count",
                    new Type[] { source.ElementType }, source.Expression));
        }

        /// <summary> Returns the element at a specified index in a sequence.</summary>
        /// <returns> The element at the specified position in <paramref name="source" />.</returns>
        /// <param name="source"> An <see cref="IQueryable" /> to return an element from.</param>
        /// <param name="index"> The zero-based index of the element to retrieve.</param>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero.</exception>
        public static object ElementAt(this IQueryable source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) throw new ArgumentOutOfRangeException("index");

            return source.Provider.Execute(
                Expression.Call(
                    typeof(Queryable),
                    "ElementAt",
                    new Type[] { source.ElementType },
                    source.Expression,
                    Expression.Constant(index)));
        }

        private static IEnumerable Execute<TModel, TResult>(this IQueryable source, Func<TModel, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (source is DataTableWrapper)
            {
                return source;
            }

            var type = source.ElementType;


            if (selector != null)
            {
                var groups = new List<AggregateFunctionsGroup>();

                if (type == typeof(AggregateFunctionsGroup))
                {
                    foreach (AggregateFunctionsGroup group in source)
                    {
                        group.Items = group.Items.AsQueryable().Execute(selector);
                        groups.Add(group);
                    }

                    return groups;
                }
                else
                {
                    var list = new List<TResult>();

                    foreach (TModel item in source)
                    {
                        list.Add(selector(item));
                    }

                    return list;
                }
            }
            else
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

                foreach (var item in source)
                {
                    list.Add(item);
                }

                return list;
            }
        }
    }
}
