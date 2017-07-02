using Core.Entity;
using Core.Mvc.ViewModel;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using System.Linq.Expressions;
using Kendo.Mvc;
using Core.Mvc.Extensions.FilterRelated;
using Core.Cmn;

namespace Core.Mvc.Helpers
{
    public static class IQueryableFilterApplier
    {

        private static List<Kendo.Mvc.IFilterDescriptor> _viewModelFilterItems;

        public static DataSourceResult GetDataSourceResult<TModel, TResult>(this IQueryable<TModel> modelQueryableInstances, DataSourceRequest request, Func<TModel, TResult> mapper)
        {
            DataSourceResult dataSourceResult = null;
            if (request.Filters.Any())
            {
                var dataSourceBasedOnViewModelFields = request.RefineDataSource(typeof(TResult));
                if (dataSourceBasedOnViewModelFields.Any())
                {
                    //modelQueryableInstances.WhereV<TResult>(dataSourceBasedOnViewModelFields);
                    dataSourceResult = modelQueryableInstances.ToDataSourceResult<TModel,TResult>(request, mapper);
                    dataSourceResult.Data = (IQueryable)dataSourceResult.Data.AsQueryable().Where(dataSourceBasedOnViewModelFields);
                    dataSourceResult.Total = dataSourceResult.Data.AsQueryable().Count();
                }
                else // if filter contains not any items.
                {
                    dataSourceResult = modelQueryableInstances.ToDataSourceResult<TModel, TResult>(request, mapper);
                    //return dataSourceResult;
                }
                return dataSourceResult;
            }
            else // if filter contains not any items.
            {
                dataSourceResult = modelQueryableInstances.ToDataSourceResult<TModel, TResult>(request, mapper);
                return dataSourceResult;
            }
        }

        /// <summary>
        /// Investigates for the fields which are not in Model but in its respective ViewModel , and clears the incoming DataSourceRequest from the fields which are not going to be used in 
        /// the Query parsing based on Model fields and , eventually , returns DataSourceRequest object which solely include ViewModel fields.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="ViewModel.IViewModel<TModel>"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private static IEnumerable<Kendo.Mvc.IFilterDescriptor> RefineDataSource(this DataSourceRequest request, Type entityViewModelType)
        {
            _viewModelFilterItems = new List<Kendo.Mvc.IFilterDescriptor>();
            var viewModelFields = entityViewModelType;
            if (typeof(IViewModel).IsAssignableFrom(entityViewModelType))
            {
                var filters = (request.Filters as List<Kendo.Mvc.IFilterDescriptor>);

                foreach (var filterItem in filters)
                {
                    if (filterItem is Kendo.Mvc.CompositeFilterDescriptor)
                    {
                        var filterDescriptors = (filterItem as Kendo.Mvc.CompositeFilterDescriptor).FilterDescriptors;
                        DoDataSourceRequestRefining(request, filterDescriptors , entityViewModelType);
                        var compositeFilterDescriptor = filterItem as Kendo.Mvc.CompositeFilterDescriptor;
                    }
                    else if (filterItem is Kendo.Mvc.FilterDescriptor)
                    {
                        var filterCollection = new Kendo.Mvc.Infrastructure.Implementation.FilterDescriptorCollection();
                        filterCollection.Add(filterItem as Kendo.Mvc.FilterDescriptor);
                        DoDataSourceRequestRefining(request, filterCollection, entityViewModelType);
                        break;
                    }
                }

            }
            return _viewModelFilterItems;
        }

        private static void DoDataSourceRequestRefining(DataSourceRequest request, Kendo.Mvc.Infrastructure.Implementation.FilterDescriptorCollection filterDescriptorCollection , Type viewModelType)
        {
            IFilterDescriptor filterItem = null;
            foreach (var item in filterDescriptorCollection)
            {
                filterItem  = item as Kendo.Mvc.FilterDescriptor;
                if (filterItem == null)
                {
                    var compFilterItem = item as Kendo.Mvc.CompositeFilterDescriptor;
                        

                    if (CheckIfFilteritemIsFromViewModel((compFilterItem.FilterDescriptors[0] as Kendo.Mvc.FilterDescriptor).Member , viewModelType))
                    {
                        _viewModelFilterItems.Add(compFilterItem);
                    }
                   

                }
                else if (CheckIfFilteritemIsFromViewModel((filterItem as Kendo.Mvc.FilterDescriptor).Member , viewModelType))
                {
                    //Adding ViewModel filter to its respective list.
                    _viewModelFilterItems.Add(filterItem);
                }
            }
            if (_viewModelFilterItems.Count != 0)
            {
                foreach (var fiItem in _viewModelFilterItems)
                {
                    //TODO: Omit the filter descriptor entry from DataSourceRequest
                    RemoveFilterDescriptorFromDataSourceResult(request, fiItem);
                }
            }
            
        }
       
        private static void RemoveFilterDescriptorFromDataSourceResult(DataSourceRequest request, Kendo.Mvc.IFilterDescriptor filterItemDescriptor)
        {
            var compFilter = request.Filters[0] as Kendo.Mvc.CompositeFilterDescriptor;
            if(compFilter != null){
                compFilter.FilterDescriptors.Remove(filterItemDescriptor);
            }
            else // null
            {
                request.Filters.Remove(filterItemDescriptor);
            }
            
        }

        private static void RemoveFilterDescriptorFromDataSourceResult(DataSourceRequest request)
        {
            
        }

        private static void ConvertFilterItemToCondition(Kendo.Mvc.FilterDescriptor filterItemDescriptor)
        {
            
        }

        private static bool CheckIfFilteritemIsFromViewModel(string filterItemName , Type viewModelType)
        {
            var viewModelTypeProperties = viewModelType.GetProperties();
            return !(viewModelTypeProperties.Any(prop => prop.Name == filterItemName ) && viewModelType.GetProperty("Model").PropertyType.GetProperty(filterItemName) != null);
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
        public static IQueryable WhereV<TViewModel>(this IQueryable source, IEnumerable<IFilterDescriptor> filterDescriptors)
        {
            if (filterDescriptors.Any())
            {
                var viewType = typeof(TViewModel);
                ConvertViewModelToModelFilterDescriptor(viewType, filterDescriptors);
               

                var parameterExpression = Expression.Parameter(source.ElementType, "item");
                //ViewModelToModelFilter
                var expressionBuilder = new FilterDescriptorCollectionExpressionBuilder(parameterExpression, filterDescriptors);
                expressionBuilder.Options.LiftMemberAccessToNull = source.Provider.IsLinqToObjectsProvider();
                var predicate = expressionBuilder.CreateFilterExpression();
                return source.Where(predicate);
            }

            return source;
        }

        private static void ConvertViewModelToModelFilterDescriptor(Type viewType , IEnumerable<IFilterDescriptor> filterDescriptors)
        {
           var concreteFilterDscriptors = filterDescriptors as IEnumerable<FilterDescriptor>;
          
           //foreach (var filterItem in filterDescriptors)
           //{
           //    if (filterItem is Kendo.Mvc.CompositeFilterDescriptor)
           //    {
           //        var filterDescriptor = filterItem as Kendo.Mvc.CompositeFilterDescriptor;

           //        foreach (var fItem in filterDescriptor.FilterDescriptors)
           //        {
           //           fItem = DoManipulationOnFilterDescriptorItem(fItem as Kendo.Mvc.FilterDescriptor, viewType);
           //        }

           //    }
           //    else if(filterItem is Kendo.Mvc.FilterDescriptor)
           //    {
           //        fItem = DoManipulationOnFilterDescriptorItem(filterItem as Kendo.Mvc.FilterDescriptor, viewType);
           //    }
  
           //}
            
        }

        //private static CompositeFilterDescriptor DoManipulationOnFilterDescriptorItem(IFilterDescriptor filterDescriptor, Type viewType)
        //{
        //    var filterDescriptorItem = filterDescriptor as Kendo.Mvc.FilterDescriptor;
        //    var customAttributes = (viewType.GetProperty(filterDescriptorItem.Member) as System.Reflection.MemberInfo).CustomAttributes.SingleOrDefault(attr => attr.AttributeType.FullName == "Core.Mvc.Attribute.Filter.ComputedPropertyInfoAttribute");
        //    var filterItemCopy = filterDescriptorItem;
        //    if (customAttributes != null)
        //    {
        //        ReplaceViewModelFiltersToModelEquivalent(customAttributes, filterItemCopy);
        //    }
        //}

        
        private static void ReplaceViewModelFiltersToModelEquivalent(System.Reflection.CustomAttributeData customAttributes, FilterDescriptor filterItem)
        {
            //var ctrosArguments = customAttributes.ConstructorArguments;
            //var compositeFilterDescriptorItem = new CompositeFilterDescriptor();
            //if (ctrosArguments.Count != 0)
            //{
            //    compositeFilterDescriptorItem.FilterDescriptors = new Kendo.Mvc.Infrastructure.Implementation.FilterDescriptorCollection();
            //}
            //foreach (var argumentItem in ctrosArguments)
            //{
            //    if(argumentItem.ArgumentType.FullName == "System.String")
            //    {

            //    }
            //    else if(argumentItem.ArgumentType.FullName == "System.Array")
            //    {
            //        foreach(var arrItem in argumentItem.Value as System.Array){
            //            var filterDescriptor = new FilterDescriptor(arrItem.ToString(), filterItem.Operator, filterItem.Value);
            //            compositeFilterDescriptorItem.FilterDescriptors.Add(filterDescriptor);
            //       }

            //    }
            //}
        }
    }
}
