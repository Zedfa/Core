using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;

namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    /// <summary>
    /// DataSource کلاس مبنا برای همه کلاس های 
    /// </summary>
    public abstract class DataSourceBase : JsonObjectBase
    {
        public DataSourceBase()
        {
            Transport = new TransportBase();

            Filters = new List<IFilterDescriptor>();

            ServerFiltering = true;

            OrderBy = new List<SortDescriptor>();

            Groups = new List<GroupDescriptor>();

            Aggregates = new List<AggregateDescriptorFunction>();

            Schema = new DataSourceSchema();

            Events = new Dictionary<DataSourceEvent, object>();

            Type = Core.Mvc.Helpers.CustomWrapper.DataSource.DataSourceType.Ajax;

        }
        public int TotalPages { get; set; }

        public IFilterItem Filter { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public DataSourceSchema Schema { get; protected set; }
        internal Dictionary<DataSourceEvent, object> Events { get; set; }

        private object SerializeDataSource(IEnumerable data)
        {
            // var dataTableEnumerable = RawData as DataTableWrapperCr;

            //if (dataTableEnumerable != null && dataTableEnumerable.Table != null)
            //{
            //    return data.SerializeToDictionary(dataTableEnumerable.Table);
            //}
            //else if (data is IEnumerable<AggregateFunctionsGroup>)
            //{
            //    if (!ServerGrouping)
            //    {
            //        return data.Cast<IGroup>().Leaves();
            //    }
            //}
            return data;
        }

        internal void DefineDataSourceModelKey(DataSourceInfo dataSourceInfo)
        {
            if (!string.IsNullOrEmpty(dataSourceInfo.ModelCr.ModelIdName))
            {   //At this point,we know that the Schema has been filled with our desired values,otherwise we should have transfered an instance of DataSourceBase as a parameter.
                this.Schema.Model.Id = new DataKeyCr(dataSourceInfo.ModelCr.ModelIdName);
            }
            else if (dataSourceInfo.ModelCr.ModelType.GetMember("Id").Count() > 0)
            {
                this.Schema.Model.Id = new DataKeyCr("Id");
            }
            else if (dataSourceInfo.ModelCr.ModelType.GetMember("ID").Count() > 0)
            {
                this.Schema.Model.Id = new DataKeyCr("ID");

            }
            else
            {
                throw new Exception("you must set Model Id Name in DataSourceInfo");
            }
            this.Transport.EntityKeyName = this.Schema.Model.Id.Name;
        }

        public bool IsClientOperationMode
        {
            get
            {
                return Type == DataSourceType.Ajax && !(ServerPaging && ServerSorting && ServerGrouping && ServerFiltering && ServerAggregates);
            }
        }

        public void ModelType(Type modelType)
        {
            Schema.Model = new ModelDescriptor(modelType);
        }

        public bool Batch
        {
            get;
            set;
        }

        public DataSourceType? Type
        {
            get;
            set;
        }

        public IList<IFilterDescriptor> Filters
        {
            get;
            protected set;
        }

        public IList<SortDescriptor> OrderBy
        {
            get;
            protected set;
        }

        public IList<GroupDescriptor> Groups
        {
            get;
            protected set;
        }

        //public IList<AggregateDescriptor> Aggregates
        //{
        //    get;
        //    protected set;
        //}
        public List<AggregateDescriptorFunction> Aggregates { get; set; }
        //{
        //    get;
        //    protected set;
        //}
        public int PageSize
        {
            get;
            set;
        }

        public bool ServerPaging
        {
            get;
            set;
        }

        public bool ServerSorting
        {
            get;
            set;
        }

        public bool ServerFiltering
        {
            get;
            set;
        }

        public bool ServerGrouping
        {
            get;
            set;
        }

        public bool ServerAggregates
        {
            get;
            set;
        }

        public TransportBase Transport
        {
            get;
            protected set;
        }

        public IEnumerable Data
        {
            get;
            set;
        }

        public string ID { get; set; }

        public IEnumerable RawData
        {
            get;
            set;
        }

        public IEnumerable<AggregateResult> AggregateResults
        {
            get;
            set;
        }

        public void Process(DataSourceRequest request, bool processData)
        {
            RawData = Data;

            if (request.Sorts == null)
            {
                request.Sorts = OrderBy;
            }
            else if (request.Sorts.Any())
            {
                OrderBy.Clear();
                OrderBy.AddRange(request.Sorts);
            }
            else
            {
                OrderBy.Clear();
            }

            if (request.PageSize == 0)
            {
                request.PageSize = PageSize;
            }

            PageSize = request.PageSize;

            if (request.Groups == null)
            {
                request.Groups = Groups;
            }
            else if (request.Groups.Any())
            {
                Groups.Clear();
                Groups.AddRange(request.Groups);
            }
            else
            {
                Groups.Clear();
            }

            if (request.Filters == null)
            {
                request.Filters = Filters;
            }
            else if (request.Filters.Any())
            {
                Filters.Clear();
                Filters.AddRange(request.Filters);
            }
            else
            {
                Filters.Clear();
            }

            if (!request.Aggregates.Any())
            {
                //request.Aggregates = Aggregates;
            }
            else if (request.Aggregates.Any())
            {
                MergeAggregateTypes(request);

                Aggregates.Clear();
                // Aggregates.AddRange(request.Aggregates);
            }
            else
            {
                Aggregates.Clear();
            }

            if (Groups.Any() && Aggregates.Any() && Data == null)
            {
                // Groups.Each(g => g.AggregateFunctions.AddRange(Aggregates.SelectMany(a => a.Aggregates)));
            }

            if (Data != null)
            {
                if (processData)
                {
                    var result = Data.AsQueryable().ToDataSourceResult(request);

                    Data = result.Data;

                    Total = result.Total;

                    AggregateResults = result.AggregateResults;
                }
                else
                {
                    var wrapper = Data as IGridCustomGroupingWrapperCr;
                    if (wrapper != null)
                    {
                        //RawData = Data = wrapper.GroupedEnumerable.AsGenericEnumerable();
                    }
                }
            }

            Page = request.Page;

            if (Total == 0 || PageSize == 0)
            {
                TotalPages = 1;
            }
            else
            {
                TotalPages = (Total + PageSize - 1) / PageSize;
            }
        }

        private void MergeAggregateTypes(DataSourceRequest request)
        {
            //if (Aggregates.Any())
            //{
            //    foreach (var requestAggregate in request.Aggregates)
            //    {
            //        var match = Aggregates.SingleOrDefault(agg => agg.Member.Equals(requestAggregate.Member, StringComparison.InvariantCultureIgnoreCase));
            //        if (match != null)
            //        {
            //            requestAggregate.Aggregates.Each(function =>
            //            {
            //                var innerFunction = match.Aggregates.SingleOrDefault(matchFunction => matchFunction.AggregateMethodName == function.AggregateMethodName);
            //                if (innerFunction != null && innerFunction.MemberType != null)
            //                {
            //                    function.MemberType = innerFunction.MemberType;
            //                }
            //            });
            //        }
            //    }
            //}
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            if (Transport.Read.Url == null)
            {
                // If Url is not set assume the current url (used in server binding)
                Transport.Read.Url = "";
            }

            var transport = Transport.ToJson();

            if (transport.Keys.Any())
            {
                json["transport"] = transport;
            }

            if (PageSize > 0)
            {
                json["pageSize"] = PageSize;
                json["page"] = Page;
                json["total"] = Total;
            }

            if (ServerPaging)
            {
                json["serverPaging"] = ServerPaging;
            }

            if (ServerSorting)
            {
                json["serverSorting"] = ServerSorting;
            }

            if (ServerFiltering)
            {
                json["serverFiltering"] = ServerFiltering;
            }

            if (ServerGrouping)
            {
                json["serverGrouping"] = ServerGrouping;
            }

            if (ServerAggregates)
            {
                json["serverAggregates"] = ServerAggregates;
            }
            if (Events.Keys.Any())
            {
                SerializeDataSourceEvents(Events, json);
            }
            if (Type != null)
            {
                json["type"] = "aspnetmvc-" + Type.ToString().ToLower();
            }

            if (OrderBy.Any())
            {
                json["sort"] = OrderBy.ToJson();
            }

            if (Groups.Any())
            {
                json["group"] = Groups.ToJson();
            }

            if (Aggregates.Any())
            {
                var aggObjectList = new List<IDictionary<string, object>>();
                foreach (var item in Aggregates)
                {
                    var jsonDic = item.ToJson();
                    aggObjectList.Add(jsonDic);
                }
                json["aggregate"] = aggObjectList;// Aggregates.SelectMany(agg => agg.Aggregates.ToJson());
            }

            if (Filters.Any() || ServerFiltering)
            {
                json["filter"] = Filters.OfType<FilterDescriptorBase>().ToJson();
            }

            if (Filter != null && ServerFiltering)
            {
                //var f = Filter.Trim(new char[] { '\\', '"' });
                var filter = (Filter as FilterItem);
                if (filter != null)
                {
                    json["filter"] = filter.ToJson();
                }
                else
                {
                    var compFilter = (Filter as CompositeFilterItem);
                    json["filter"] = compFilter.ToJson();
                }

            }


            if (Schema.Model != null)
            {
                json["schema"] = Schema.ToJson();
            }


            json["batch"] = Batch;


            if (IsClientOperationMode && RawData != null)
            {
                json["data"] = new Dictionary<string, object>()
                {
                    { Schema.Data,  SerializeDataSource(RawData) },
                    { Schema.Total, Total }
                };
            }
            else if (Type == DataSourceType.Ajax && !IsClientOperationMode && Data != null)
            {
                json["data"] = new Dictionary<string, object>()
                {
                    //{ Schema.Data,  SerializeDataSource(Data) },
                    { Schema.Total, Total }
                };
                //json["type"] = "aspnetmvc-ajax";
            }
        }

        private void SerializeDataSourceEvents(Dictionary<DataSourceEvent, object> Events, IDictionary<string, object> json)
        {
            foreach (var item in Events.Keys)
            {
                switch (item)
                {
                    case DataSourceEvent.OnError:
                        json["error"] = new ClientHandlerDescriptor { TemplateDelegate = obj => Events.SingleOrDefault(itm => itm.Key == DataSourceEvent.OnError).Value };
                        break;
                    case DataSourceEvent.OnRequestStart:
                        json["requestStart"] = new ClientHandlerDescriptor { TemplateDelegate = obj => Events.SingleOrDefault(itm => itm.Key == DataSourceEvent.OnRequestStart).Value };
                        break;
                    case DataSourceEvent.OnRequestEnd:
                        json["requestEnd"] = new ClientHandlerDescriptor { TemplateDelegate = obj => Events.SingleOrDefault(itm => itm.Key == DataSourceEvent.OnRequestEnd).Value };
                        break;
                    case DataSourceEvent.Sync:
                        //json["sync"] = new ClientHandlerDescriptor { TemplateDelegate = obj => Events.SingleOrDefault(itm => itm.Key == DataSourceEvent.Sync).Value };
                        break;
                    case DataSourceEvent.OnChange:
                        //json["change"] = new ClientHandlerDescriptor { TemplateDelegate = obj => Events.SingleOrDefault(itm => itm.Key == DataSourceEvent.OnChange).Value };
                        break;
                    default:
                        break;
                }
            }
        }

        #region Events
        public Dictionary<DataSourceEvent, object> AssignDsEvents(Dictionary<DataSourceEvent, object> dictionary, Helpers.CoreKendoGrid.Settings.Features.ClientEvent clientEvent)
        {
            if (dictionary.Any(k => k.Value.ToString() != string.Empty))
            {
                var tobeAppendToCustomHandlers = dictionary.Where(item => item.Value.ToString() != string.Empty).ToList();
                var tobeAppendToDefaultHandlers = this.GetListOfPredefinedEvents().Where(item => tobeAppendToCustomHandlers.Any(it => it.Key != item.Key)).ToList();
                var refinedDic = new Dictionary<DataSourceEvent, object>();
                if (tobeAppendToCustomHandlers.Count > 0)
                {
                    tobeAppendToCustomHandlers.ForEach(itm => { refinedDic.Add(itm.Key, itm.Value); });
                    dictionary = MakeDataSourceEvents(refinedDic, clientEvent);
                }
                tobeAppendToDefaultHandlers.ForEach(itm =>
                {

                    switch (itm.Key)
                    {
                        case DataSourceEvent.OnError:
                            dictionary.Add(DataSourceEvent.OnError, BuildDefaultOnErrorEvent(string.Empty));
                            break;
                        case DataSourceEvent.OnRequestStart:
                            dictionary.Add(DataSourceEvent.OnRequestStart, BuildDefaultOnRequestStartEvent(string.Empty));
                            break;
                        case DataSourceEvent.OnRequestEnd:
                            dictionary.Add(DataSourceEvent.OnRequestEnd, BuildDefaultOnRequestEndEvent(string.Empty));
                            break;
                        case DataSourceEvent.Sync:
                            dictionary.Add(DataSourceEvent.Sync, BuildDefaultOnSyncEvent(string.Empty));
                            break;
                        case DataSourceEvent.OnChange:
                            dictionary.Add(DataSourceEvent.OnChange, BuildDefaultOnChangeEvent(string.Empty));
                            break;
                        default:
                            break;
                    }
                });
            }
            else
            {
                dictionary = MakeDataSourceEvents(dictionary, clientEvent);
            }
            return dictionary;
        }

        private Dictionary<DataSourceEvent, object> MakeDataSourceEvents(Dictionary<DataSourceEvent, object> recivedEvents, Helpers.CoreKendoGrid.Settings.Features.ClientEvent clEvent)
        {
            var finalEventDic = new Dictionary<DataSourceEvent, object>();

            //if (clEvent != null)
            //{
            //    if (!string.IsNullOrEmpty(clEvent.OnInit))
            //    {
            //        recivedEvents.Add(DataSourceEvent.OnRequestStart, clEvent.OnInit);
            //    }
            //}
            if (recivedEvents.Any(itm => itm.Value.ToString() != string.Empty))
            {
                //list of Events which their implementations have to be defined as Default.
                //var tobeDefineAsDefault = defEvents.Keys.Where(k => list.Any(lk => lk != k)).ToList();

                foreach (var item in recivedEvents)
                {
                    switch (item.Key)
                    {
                        case DataSourceEvent.OnError:
                            BuildCustomOnErrorEvent(finalEventDic, item.Value.ToString());
                            break;
                        case DataSourceEvent.OnRequestStart:
                            BuildCustomOnRequestStartEvent(finalEventDic, item.Value.ToString());
                            break;
                        case DataSourceEvent.OnRequestEnd:
                            BuildCustomOnRequestEndEvent(finalEventDic, item.Value.ToString());
                            break;
                        case DataSourceEvent.Sync:
                            BuildCustomOnSyncEvent(finalEventDic, item.Value.ToString());
                            break;
                        case DataSourceEvent.OnChange:
                            BuildCustomOnChangeEvent(finalEventDic, item.Value.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {

                MakePredefinedEventHandlers(finalEventDic);
            }
            return finalEventDic;
        }

        private void MakePredefinedEventHandlers(Dictionary<DataSourceEvent, object> finalEventDic)
        {
            finalEventDic.Add(DataSourceEvent.OnChange, BuildDefaultOnChangeEvent(string.Empty));
            finalEventDic.Add(DataSourceEvent.Sync, BuildDefaultOnSyncEvent(string.Empty));
            finalEventDic.Add(DataSourceEvent.OnRequestEnd, BuildDefaultOnRequestEndEvent(string.Empty));
            finalEventDic.Add(DataSourceEvent.OnRequestStart, BuildDefaultOnRequestStartEvent(string.Empty));
            finalEventDic.Add(DataSourceEvent.OnError, BuildDefaultOnErrorEvent(string.Empty));
        }

        #region Custom Event Handler Definitions

        private void BuildCustomOnChangeEvent(Dictionary<DataSourceEvent, object> finalEventDic, string customOnChangeFunction)
        {
            finalEventDic.Add(DataSourceEvent.OnChange, BuildDefaultOnChangeEvent(customOnChangeFunction));
        }

        private void BuildCustomOnSyncEvent(Dictionary<DataSourceEvent, object> finalEventDic, string customOnSyncEventFunction)
        {
            finalEventDic.Add(DataSourceEvent.Sync, BuildDefaultOnSyncEvent(customOnSyncEventFunction));
        }

        private void BuildCustomOnRequestEndEvent(Dictionary<DataSourceEvent, object> finalEventDic, string customOnRequestEndFunction)
        {
            finalEventDic.Add(DataSourceEvent.OnRequestEnd, BuildDefaultOnRequestEndEvent(customOnRequestEndFunction));
        }

        private void BuildCustomOnRequestStartEvent(Dictionary<DataSourceEvent, object> finalEventDic, string customOnRequestStartFunction)
        {
            finalEventDic.Add(DataSourceEvent.OnRequestStart, BuildDefaultOnRequestStartEvent(customOnRequestStartFunction));
        }

        private void BuildCustomOnErrorEvent(Dictionary<DataSourceEvent, object> finalEventDic, string customOnErrorFunction)
        {
            finalEventDic.Add(DataSourceEvent.OnError, BuildDefaultOnErrorEvent(customOnErrorFunction));
        }

        #endregion

        #region Default Datasoucre Event Definition

        private object BuildDefaultOnChangeEvent(string customOnChangeFunction)
        {
            return string.Empty;
        }

        private object BuildDefaultOnSyncEvent(string customOnSyncEventFunction)
        {
            return string.Empty;
        }

        //private object BuildDefaultOnRequestEndEvent(string customOnRequestEndFunction)
        //{
        //    var OnRequestEndDef = string.Empty;

        //    OnRequestEndDef = string.Format("function {2}_requestEnd(e) {0} " +
        //        //" var grid = $('#{2}').data('kendoGrid');  $('#{2}_loadingSpinner').hide(30000);" +
        //                                            " {3} " +
        //                                            "{1}"
        //                                            , "{", "}", ID, customOnRequestEndFunction != string.Empty ? "eval('" + customOnRequestEndFunction + "(e);');" : string.Empty);

        //    return OnRequestEndDef;
        //}
        public abstract object BuildDefaultOnRequestEndEvent(string customOnRequestEndFunction);

        public abstract object BuildDefaultOnRequestStartEvent(string customOnRequestStartFunction);

        public abstract object BuildDefaultOnErrorEvent(string customOnErrorFunction);

        #endregion

        private Dictionary<DataSourceEvent, object> GetListOfPredefinedEvents()
        {
            var defEvents = new Dictionary<DataSourceEvent, object>();
            defEvents.Add(DataSourceEvent.OnChange, "");
            defEvents.Add(DataSourceEvent.OnError, "");
            defEvents.Add(DataSourceEvent.OnRequestEnd, "");
            defEvents.Add(DataSourceEvent.OnRequestStart, "");
            defEvents.Add(DataSourceEvent.Sync, "");
            return defEvents;
        }

        public string GetOnDataSourceErrorScriptTemplate(string gridID)
        {
            return string.Format("<script type=\"text/kendo-template\" id=\"{0}_val_message\">" +
                                 "<div class=\"k-widget k-tooltip k-tooltip-validation k-invalid-msg field-validation-error\" style=\"margin: 0.5em; display: block; \" " +
                                 "data-for=\"#=field#\" data-valmsg-for=\"#=field#\" id=\"#=field#_validationMessage\" >" +
                                 "<span class=\"k-icon k-warning\"> </span>#=message#<div class=\"k-callout k-callout-n\"></div></div>" +
                                 "</script>" +
                                 "<script> var {0}_validationMessageTmpl = kendo.template($('#{0}_val_message').html()); " +
                                 "function showMessage_{0}(container, name, errors){1} " +
                                 "container.find(\"[data-valmsg-for='\" + name + \"'],[data-val-msg-for= '\"  + name +  \"']\")" +
                                 ".replaceWith({0}_validationMessageTmpl({1} field: name, message: errors[0] {2}));{2}" +
                                 "</script>", gridID, "{", "}");
        }

        #endregion
    }
}
