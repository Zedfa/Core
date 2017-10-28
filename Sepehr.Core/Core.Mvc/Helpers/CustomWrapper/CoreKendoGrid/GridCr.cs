
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.Features;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Core.Cmn.Extensions;
using Core.Mvc.ViewModel;
using Core.Mvc.Extensions;
using Core.Entity;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using System.Web.Mvc.Html;
using System.Web.Script.Serialization;
using Core.Service;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Helpers.ElementAuthentication;
using Core.Cmn;
using Core.Mvc.Helpers.CustomWrapper.SearchRelated;
using Core.Mvc.Helpers.Lookup;
using Core.Mvc.Attribute.Filter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    /// <summary>
    ///کلاسی که وظیفه تولید گرید کندو را به عهده دارد. 
    /// </summary>
    [Serializable()]
    public class GridCr<T> where T : new()
    {
        private Dictionary<string, object> _totalConfig;
        private List<Column> _columnsConfig;
        private DataSourceGridCr _dataSource;
        private Features _features;
        private ClientDependentFeature _clientDependency;

        private Toolbar _toolbar;
        public string ID { get; private set; }
        private Type _modelType;
        private string _dtoType;
        private string _addEditTemplate;
        //private HtmlHelper _helper;
        private ModelMetadata _modelMetaData;
        private string _removableStr = string.Empty;
        private string _width;
        private string _height;
        private List<GridCommandCr> commandsAdded;

        private bool _hasCommonAddOrEditScriptAppended;
        private AccessOperation _accessOperation;
        /// <summary>
        /// سازنده گرید 
        /// </summary>
        /// <param name="helper">متغیری که از نوع اچ تی ام ال هلپر است</param>
        /// <param name="gridId">شناسه گرید مربوطه</param>
        /// <param name="gridInfo">حاوی اطلاعات و ویژگیهای مربوط به گرید</param>
        /// <param name="clientDependency">  حاوی ویژگیهای مربوط به گرید که در سمت فایل تعریف گرید به کار می رود.</param>
        /// <param name="Width">طول گرید</param>
        /// <param name="Height">ارتفاع گرید</param>
        public GridCr(HtmlHelper helper, string gridId, GridInfo gridInfo, Type viewModelType = null, ClientDependentFeature clientDependency = null, string Width = null, string Height = null)
        {

            clientDependency = clientDependency ?? new ClientDependentFeature();
            viewModelType = gridInfo.DtoModelType ?? viewModelType;
            _dtoType = gridInfo.DtoModelType == null ? string.Empty : gridInfo.DtoModelType.FullName;
            _addEditTemplate = gridInfo.Features.EditableConfig.CustomConfig.Template.Url;
            _modelMetaData = helper == null ? null : helper.ViewData.ModelMetadata;
            _accessOperation = new AccessOperation();
            BuildUniqueIDForGrid(gridId, helper);
            gridInfo.GridID = ID;
            _width = Width;
            _height = Height;
            _features = gridInfo.Features;
            _totalConfig = new Dictionary<string, object>();

            if (typeof(IViewModel).IsAssignableFrom(typeof(T)) && gridInfo.DataSource.ModelCr.ModelType == null && gridInfo.ColumnsInfo == null && viewModelType == null)
            {
                var modelType = typeof(T);
                gridInfo.ColumnsInfo = modelType.BiuldColumnsFromViewModel();
                _modelType = modelType;
            }

            else if (!typeof(IViewModel).IsAssignableFrom(typeof(T)) && viewModelType != null)
            {
                if (typeof(IViewModel).IsAssignableFrom(viewModelType))
                {
                    var modelType = viewModelType;
                    //gridInfo.ColumnsInfo = modelType.BiuldColumnsFromViewModel();
                    _modelType = modelType;
                }

            }

            //if (typeof(IViewModelBase).IsAssignableFrom(typeof(T)) && gridInfo.DataSource.ModelRP.ModelType != null && gridInfo.ColumnsInfo == null)
            //{
            //}
            //if (!typeof(IViewModelBase).IsAssignableFrom(typeof(T)) && gridInfo.DataSource.ModelRP.ModelType != null && gridInfo.ColumnsInfo == null)
            //{
            //}
            else if ((!typeof(IViewModel).IsAssignableFrom(typeof(T)) || gridInfo.DataSource.ModelCr.ModelType != null) && gridInfo.ColumnsInfo != null)
            {
                var modelType = typeof(T);
                _modelType = modelType;
            }
            else if ((typeof(IViewModel).IsAssignableFrom(typeof(T)) || gridInfo.DataSource.ModelCr.ModelType == null) && gridInfo.ColumnsInfo != null)
            {
                var modelType = typeof(T);
                _modelType = modelType;
            }

            else if (!typeof(IViewModel).IsAssignableFrom(typeof(T)) && gridInfo.DataSource.ModelCr.ModelType != null)
            {
                _modelType = gridInfo.DataSource.ModelCr.ModelType;
            }

            _features.SetCulture(gridInfo.GetCultureInfo());
            _features.EditableConfig.CustomConfig.Template.PartialViewModel = _modelType;
            _features.EditableConfig.CustomConfig.Template.CorrespondingHtmlHelper = helper;
            _columnsConfig = gridInfo.ColumnsInfo;
            gridInfo.DataSource.ModelCr.ModelType = _modelType;

            if (clientDependency != null)
            {
                _clientDependency = clientDependency;
                gridInfo.DataSource.CrudCr.Read.QueryStringItems = clientDependency.ReadQueryParams;
                gridInfo.DataSource.CrudCr.Read.ParamsFunction = clientDependency.PreReadFunction;
                gridInfo.DataSource.CrudCr.Read.ReadFilterObject = clientDependency.ReadFilterObject;

            }

            _dataSource = BuildDataSourceObject(gridInfo.DataSource);
            DefineActionAuthority(gridInfo.DataSource.CrudCr);
            MakeCUDUrls(_dataSource.Transport, gridInfo.DataSource.CrudCr);

            if (_features.ReadOnly)
            {
                _accessOperation.InsertableOrUpdatable = false;
            }

            if (!_features.ReadOnly && !_accessOperation.Insertable && !_accessOperation.Updatable)
            {
                _accessOperation.InsertableOrUpdatable = false;
            }

            //if (_features.FileOutput != null)
            //{

            //    switch (_features.FileOutput.OutputType)
            //    {

            //        case CustomWrapper.CoreKendoGrid.Settings.Features.OutputType.Excel:
            //            gridInfo.GridToolbar.ExportToExcel = true;
            //            break;

            //        case CustomWrapper.CoreKendoGrid.Settings.Features.OutputType.Pdf:
            //            gridInfo.GridToolbar.ExportToPdf = true;
            //            break;

            //    };
            //}


            _toolbar = gridInfo.GridToolbar;
        }
        /// <summary>
        /// مجوز مربوط به عملیات گرید(ورود داده جدید ، ویرایش داده، و حذف) را تعیین می کند.
        /// </summary>
        private void DefineActionAuthority(CrudCr crudInfo)
        {
            //var url = this._dataSource.Transport.Read.Url.ToLower();
            _accessOperation.Insertable = _features.Insertable;
            _accessOperation.Updatable = _features.Updatable;
            _accessOperation.Removable = _features.Removable;
            UserAccessibleElement.DefineCrudActionAuthority(_accessOperation,crudInfo);
        }

        private void BuildUniqueIDForGrid(string gridId, HtmlHelper helper)
        {
            ID = gridId;
        }
        /// <summary>
        /// ساخت شیئ دیتا سورس 
        /// </summary>
        /// <param name="dsConfig"></param>
        /// <returns></returns>
        private DataSourceGridCr BuildDataSourceObject(DataSourceInfo dsConfig)
        {
            //dynamic modelMetaData = ModelMeat;
            DataSourceGridCr ds = null;
            if (typeof(T) == null)
            {
                ds = new DataSourceGridCr();
            }
            else
            {
                ds = new DataSourceGridCr(typeof(T));
            }

            ds.ID = this.ID;
            ds.Batch = dsConfig.ServerRelated.Batch ?? false;
            ds.ServerSorting = dsConfig.ServerRelated.ServerSorting == null ? true : dsConfig.ServerRelated.ServerSorting.Value;
            ds.ServerPaging = dsConfig.ServerRelated.ServerPaging == null ? true : dsConfig.ServerRelated.ServerSorting.Value;
            ds.ServerFiltering = dsConfig.ServerRelated.ServerFiltering == null ? true : dsConfig.ServerRelated.ServerSorting.Value;
            ds.Schema.Model = new ModelDescriptor(dsConfig.ModelCr.ModelType);
            ds.Events = ds.AssignDsEvents(dsConfig.DataSourceEvents, _clientDependency.Events);
            var serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            ds.Filter = dsConfig.InitialFilter;

            //ds.Filters.Add(new FilterDescriptor() { Member= dsConfig.InitialFilter.Field , Operator= FilterOperator.Contains ,// dsConfig.InitialFilter.Operator 
            //    , = dsConfig.InitialFilter.Value });

            ds.DefineDataSourceModelKey(dsConfig);

            if (!string.IsNullOrEmpty(dsConfig.CrudCr.Read.Url))
            {
                ds.Transport.Read.Url = dsConfig.CrudCr.Read.Url;
                if (dsConfig.CrudCr.Read.QueryStringItems != null)
                {
                    if (dsConfig.CrudCr.Read.QueryStringItems.Count > 0)
                    {
                        ds.Transport.Read.Params = dsConfig.CrudCr.Read.QueryStringItems;
                    }
                }
                if (!string.IsNullOrEmpty(dsConfig.CrudCr.Read.ReadFilterObject))
                {
                    ds.Transport.Read.ReadFilterObject = dsConfig.CrudCr.Read.ReadFilterObject;
                }
                if (!string.IsNullOrEmpty(dsConfig.CrudCr.Read.ParamsFunction))
                {
                    ds.Transport.Read.ReadFunction = dsConfig.CrudCr.Read.ParamsFunction;

                }
            }

            //dsConfig.CrudRP.Read.Data
            if (_features.Searchable)
            {
                BuildSearchInfos(ds, _columnsConfig);
            }


            return ds;
        }

        private void BuildSearchInfos(DataSourceGridCr ds, List<Column> columnsConfig)
        {
            //Lookups search config accomulation
            var dsSchema = (SchemaGrid)ds.Schema;
            columnsConfig.ForEach(column =>
            {
                if (column.LookupViewInfo != null)
                {
                    var grid = column.LookupViewInfo as Core.Mvc.Helpers.Lookup.Grid;
                    var lookupConfig = new LookupConfig();

                    if (grid != null)
                    {
                        lookupConfig.LookupType = LookupTypes.GRID;
                        lookupConfig.LookupTitle = grid.Title;
                        lookupConfig.ViewModelName = grid.ViewModel;
                        lookupConfig.ViewInfoName = grid.ViewInfoKey;
                        lookupConfig.LookupName = column.Field + "Lookup";
                        lookupConfig.NavigateViewModelDisplayName = string.IsNullOrEmpty(grid.PropertyNameForDisplay) ? column.Field : grid.PropertyNameForDisplay;
                        lookupConfig.NavigateViewModelValueName = string.IsNullOrEmpty(grid.PropertyNameForValue) ? "Id" : grid.PropertyNameForValue;

                        if (string.IsNullOrEmpty(grid.PropertyNameForValue))
                        {
                            throw new Exception("set PropertyNameForValue in your grid column ");
                        }
                        else
                        {
                            lookupConfig.ModelBindingName = grid.PropertyNameForBinding;
                        }
                        dsSchema.LookupViewInfos.Add(column.Field, lookupConfig);
                    }


                }
                else if (column.DropDownListInfo != null)
                {
                    var info = column.DropDownListInfo;

                    dsSchema.DropDownInfos.Add(column.Field, info);

                }
                else if (!string.IsNullOrEmpty(column.ConstantsCategoryName) && column.LookupViewInfo == null)
                {
                    var searchInfo = new SearchConstantField();
                    searchInfo.HasCustomTypeOf(SearchRelatedTypes.Enum);
                    searchInfo.ConstantsCategoryName = column.ConstantsCategoryName;
                    dsSchema.SearchInfos.Add(column.Field, searchInfo);
                }
                else if (string.IsNullOrEmpty(column.ConstantsCategoryName) && column.LookupViewInfo == null && DefineIfColumnIsOfTypeDateTime(column.Type, column.Field))
                {
                    var searchInfo = new SearchDateField();
                    searchInfo.HasCustomTypeOf(column.Type == typeof(Core.Cmn.FarsiUtils.PersianDate) ? SearchRelatedTypes.PersianDate : SearchRelatedTypes.Date);
                    //searchInfo.ConstantsCategoryName = column.ConstantsCategoryName;
                    dsSchema.SearchInfos.Add(column.Field.Split('.')[0], searchInfo);
                }
            });
            //columnsConfig.Where(cc => cc.SearchInfo.Key != null && cc.SearchInfo.Value != null).Select(cc => cc.SearchInfo).ToList<KeyValuePair<string, ISearch>>().ForEach(item =>
            //{
            //    dsSchema.SearchInfos.Add(item.Key, item.Value);
            //});
        }

        private bool DefineIfColumnIsOfTypeDateTime(Type colType, string fieldName)
        {

            if (colType == typeof(DateTime) || colType == typeof(Core.Cmn.FarsiUtils.PersianDate))
            {

                return true;
            }
            Type propertyType = null;

            var fldTitle = fieldName.Contains('.') ? fieldName.Split('.')[0] : fieldName;
            var prop = typeof(T).GetProperties().FirstOrDefault(col => col.Name == fldTitle);
            if (prop != null)
            {
                propertyType = prop.PropertyType;
            }


            return prop == null ? false : (propertyType.Name == "DateTime" || propertyType.Name == "PersianDate");

        }

        /// <summary>
        /// یو آر آل های مربوط عملیات درج، ویرایش و حذف ساخت
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="crudRP"></param>
        private void MakeCUDUrls(TransportBase ts, CrudCr crudRP)
        {
            if (!_features.ReadOnly)
            {
                if (_accessOperation.Insertable)
                {
                    ts.Create.Url = string.IsNullOrEmpty(crudRP.Insert.Url) ? crudRP.Read.Url : crudRP.Insert.Url;
                }

                if (_accessOperation.Updatable)
                {
                    ts.Update.Url = string.IsNullOrEmpty(crudRP.Update.Url) ? crudRP.Read.Url : crudRP.Update.Url;
                }

                if (_accessOperation.Removable)
                {
                    ts.Destroy.Url = string.IsNullOrEmpty(crudRP.Remove.Url) ? crudRP.Read.Url : crudRP.Remove.Url;
                }
            }
        }
        /// <summary>
        /// تولید و ارائه اسکریپت نهایی گرید .
        /// </summary>
        /// <returns></returns>
        internal MvcHtmlString Render()
        {
            var totalSerialized = SerializeConfig(_totalConfig);
            var gridScript = new StringBuilder((new JavaScriptGeneratorCr()).InitializeFor("#" + ID, "Grid", totalSerialized));
            var modalInitializationStr = string.Empty;
            TagBuilder initialScriptBuilder = new TagBuilder("script");
            initialScriptBuilder.SetInnerText(string.Format("var {0}_ns; {0}_isEventOnDataBoundAssigned=false , {0}_gridInitialized=true;", ID, "{", "}"));
            var refreshFunctionScript = string.Empty;
            var searchScript = string.Empty;

            if (_features.Refreshable)
            {
                refreshFunctionScript = "";
            }

            var dataSourceEventScript = _dataSource.GetOnDataSourceErrorScriptTemplate(ID);

            gridScript.Append(string.Format(" {0} {1} ", modalInitializationStr, refreshFunctionScript));

            var cssStyleAttr = string.Empty;
            if (_clientDependency != null)
            {
                if (_clientDependency.CssStyles.Any())
                {
                    if (!string.IsNullOrEmpty(_width))
                    {
                        if (string.IsNullOrEmpty(_clientDependency.CssStyles.Keys.FirstOrDefault(str => str == "width")))
                        {
                            _clientDependency.CssStyles.Add("width", _width + "px");
                        }
                    }

                    if (!string.IsNullOrEmpty(_height))
                    {
                        if (string.IsNullOrEmpty(_clientDependency.CssStyles.Keys.FirstOrDefault(str => str == "height")))
                        {
                            _clientDependency.CssStyles.Add("height", _height + "px");
                        }
                    }

                    if (_clientDependency.CssStyles.Count > 0)
                    {
                        cssStyleAttr = string.Format("style=\"{0}\"", ApplyInitialCssStyleOnGrid(_clientDependency.CssStyles));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_width))
                    {
                        _clientDependency.CssStyles.Add("width", _width + "px");
                    }

                    if (!string.IsNullOrEmpty(_height))
                    {
                        _clientDependency.CssStyles.Add("height", _height + "px");
                    }

                    if (_clientDependency.CssStyles.Count > 0)
                    {
                        cssStyleAttr = string.Format("style=\"{0}\"", ApplyInitialCssStyleOnGrid(_clientDependency.CssStyles));
                    }
                }

                if (!string.IsNullOrEmpty(_clientDependency.Events.OnLazyReadScript))//eval('{2}(e)');
                {
                    gridScript.Append(string.Format("$(document).ready(function() {0} eval('{2}(null);'); {1});", "{", "}", _clientDependency.Events.OnLazyReadScript));
                }
                //if (_dataSource.Filter != null && _features.AutoBind == false)
                //{
                //  gridScript.Append(string.Format("$(document).ready(function() {0} {2} {1});", "{", "}", string.Format("$('#{0}').data('kendoGrid').dataSource.fetch();" , ID)));
                // }
            }
            else
            {
                var styleDic = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(_width))
                {
                    styleDic.Add("width", _width + "px");
                }

                if (!string.IsNullOrEmpty(_height))
                {
                    styleDic.Add("height", _height + "px");
                }
                if (styleDic.Count > 0)
                {
                    cssStyleAttr = string.Format("style=\"{0}\"", ApplyInitialCssStyleOnGrid(styleDic));
                }
            }

            var cellModeDblClick = string.Empty;
            var rowModeDblClick = string.Empty;
            var initGridEvent = string.Empty;
            //if (!string.IsNullOrEmpty(_clientDependency.Events.OnDoubleClick) && _features.Selectability != Selectable.None)
            if (_clientDependency != null)
            {
                cellModeDblClick = _features.Selectability == Selectable.Cell || _features.Selectability == Selectable.MultipleCells ?
                   string.Format("ns_Grid.GridOperations.onCellDblClick('{0}');", _clientDependency.Events.OnDoubleClick)//string.Format("$(\"td[role='gridcell']\").on('dblclick' , function(e) {0}  if(typeof {3} == 'function') {0} eval(\"{3}\");  {1} else {0} var currentCell= $(this);  var eventArg = {0} doubleClickedCell {4} currentCell , wrappingRow {4} currentCell.closest(\"tr\") {1}; {3}(eventArg); {1}  {1}); ", "{", "}", ID, _clientDependency.Events.OnDoubleClick, ":")
                  : string.Empty;
                rowModeDblClick = _features.Selectability == Selectable.Row || _features.Selectability == Selectable.MultipleRows ?
                   string.Format("ns_Grid.GridOperations.onRowDblClick('{0}');", _clientDependency.Events.OnDoubleClick)//string.Format("$(\"tr[role='row']\").on('dblclick' , function(e) {0}  if(typeof {3} == 'function') {0} eval(\"{3}\");  {1} else {0} var currentRow= $(this); var eventArg = {0} doubleClickedRow {4} currentRow {1}; eval(\"{3}\"); {1} {1});  ", "{", "}", ID, _clientDependency.Events.OnDoubleClick, ":")
                  : string.Empty;

                initGridEvent = string.IsNullOrEmpty(_clientDependency.Events.OnInit) ? initGridEvent : string.Format(" ns_Grid.GridOperations.onInitCallback('{0}' , '{1}');", _clientDependency.Events.OnInit, ID);

            }
            var dblClickScript = string.Empty;//string.Format("<script> {0} </script>", cellModeDblClick + rowModeDblClick);
            var fialGridScript = gridScript.ToString();
            var finalString = string.Format("<{0} id=\"{2}\" {5} ><span>{9}{8}{7}{6}{4}</span><{1}>{3}</{1}></{0}>", "div", "script", ID, fialGridScript + initGridEvent, !string.IsNullOrEmpty(_removableStr) ? _removableStr : string.Empty,
                cssStyleAttr,
                dataSourceEventScript,
                initialScriptBuilder.ToString(TagRenderMode.Normal), searchScript, dblClickScript);
            return MvcHtmlString.Create(finalString);
        }

        public Dictionary<string, object> GetGridTotalConfig()
        {
            var totalSerialized = (Dictionary<string, object>)SerializeConfig(_totalConfig);
            if (!string.IsNullOrEmpty(_dtoType))
            {
                totalSerialized.Add("dtoType", _dtoType);
            }

            if (!string.IsNullOrEmpty(_addEditTemplate))
            {
                totalSerialized.Add("adTempAdd", _addEditTemplate);
            }

            ReformToolbar(totalSerialized);
            //totalSerialized.Remove("editable");
            //totalSerialized.Add("editable", new Dictionary<string, object> { { "mode", null },{"createAt", "bottom"} });
            totalSerialized.Remove("edit");
            totalSerialized.Remove("save");
            totalSerialized.Remove("error");
            totalSerialized.Remove("dataBound");
            totalSerialized.Remove("dataBinding");
            var ds = totalSerialized["dataSource"] as Dictionary<string, object>;
            ds.Remove("requestEnd");
            ds.Remove("requestStart");
            ds.Remove("error");


            return totalSerialized;
        }


        private void ReformToolbar(Dictionary<string, object> totalSerialized)
        {
            totalSerialized.Remove("toolbar");//"edit", "destroy"
            var tlbrActs = new List<Dictionary<string, object>>();

            foreach (var item in commandsAdded)
            {

                switch (item.type)
                {
                    case GCommandCr.Create:
                        item.info.Text = item.info.Text ?? "جدید";

                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "create" } });

                        break;
                    case GCommandCr.Edit:
                        item.info.Text = item.info.Text ?? "ویرایش";

                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "edit" } });
                        break;
                    case GCommandCr.Delete:
                        item.info.Text = item.info.Text ?? "حذف";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "destroy" } });
                        break;
                    case GCommandCr.Search:
                        item.info.Text = item.info.Text ?? "جستجو";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "search" } });
                        break;
                    case GCommandCr.RemoveFilter:
                        item.info.Text = item.info.Text ?? "حذف جستجو";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "removeFilters" } });
                        break;

                    //case GCommandCr.UserGuide:
                    //    //tlbrActs.Add(new Dictionary<string, object> { { "text", "راهنمای استفاده" }, { "name", "userGuid" } });
                    //    break;
                    case GCommandCr.Excel:
                        item.info.Text = item.info.Text ?? "خروجی اکسل";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "excel" } });
                        break;
                    case GCommandCr.Pdf:
                        item.info.Text = item.info.Text ?? "خروجی PDF";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "pdf" } });
                        break;
                    case GCommandCr.Refresh:
                        item.info.Text = item.info.Text ?? "refresh";
                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "refresh" } });
                        break;
                    case GCommandCr.Custom:

                        tlbrActs.Add(new Dictionary<string, object> { { "text", item.info.Text }, { "name", "customAction" },
                            { "clickHandler",item.info.ClickHandler} });
                        break;

                    default:
                        break;
                }


            }



            totalSerialized.Add("toolbar", tlbrActs);
            //totalSerialized.Add("toolbar", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "text", "ایجاد" }, { "name", "create" } } });
        }
        /// <summary>
        /// استایل های مربوط به گرید.
        /// </summary>
        /// <param name="cssStyles"></param>
        /// <returns></returns>
        private string ApplyInitialCssStyleOnGrid(Dictionary<string, string> cssStyles)
        {
            var cssString = new StringBuilder();
            if (cssStyles.Any())
            {
                foreach (var cssRule in cssStyles)
                {
                    cssString.Append(string.Format("{0}:{1}; ", cssRule.Key, cssRule.Value));
                }
            }

            if (cssString.Length > 0)
            {
                return cssString.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// سریال کردن داده های مربوط به گرید.
        /// </summary>
        /// <param name="totalConfig"></param>
        /// <returns></returns>
        private IDictionary<string, object> SerializeConfig(Dictionary<string, object> totalConfig)
        {
            if (_columnsConfig != null)
            {
                if (_columnsConfig.Count > 0)
                {

                    /*--------------------Auto Bind------------------- */
                    //Comment: When autoBind property is set to false,in order to get data from the 
                    //data source,the dataSource.read() method should be called inside any change 
                    //event handler.(this functionality is so useful in cases where multiple widgets are bound to the same dataSource.) 
                    totalConfig["autoBind"] = _features.AutoBind;

                    /*--------------------ColumnMenu------------------- */
                    if (_features.ColumnMenu.ColumnMenuEnabled)
                    {
                        totalConfig["columnMenu"] = _features.ColumnMenu.ToJson();
                        if (!string.IsNullOrEmpty(_features.ColumnMenu.ColumnMenuInit))
                        {
                            totalConfig["columnMenuInit"] = new ClientHandlerDescriptor { TemplateDelegate = obj => _features.ColumnMenu.ColumnMenuInit + "(e);" };
                        }
                    }

                    /*--------------------Columns Setup-------------- */

                    var colsList = new List<IDictionary<string, object>>();
                    List<AggregateDescriptorFunction> aggList;
                    _columnsConfig.ForEach(cc =>
                    {
                        cc.GridID = ID;
                        aggList = null;
                        if (cc.Aggregates != null)
                        {
                            if (cc.Aggregates.Count > 0)
                            {
                                aggList = new List<AggregateDescriptorFunction>();
                                // cc.Aggregates.ForEach(a =>
                                foreach (var a in cc.Aggregates)
                                {

                                    var aggregateDescriptor = new AggregateDescriptorFunction() { Field = cc.Field, Aggregate = a.Key.ToString().ToLower() };
                                    aggList.Add(aggregateDescriptor);
                                }
                                //);

                                cc.ClientFooterTemplate = BuildAggregateColumnFooterTemplate(cc.Aggregates);
                            }
                        }
                        if (aggList != null)
                        {
                            _dataSource.Aggregates.AddRange(aggList);
                        }

                        var dicItem = cc.ToJson();
                        colsList.Add(dicItem);
                    });

                    /*--------------------Toolbar-------------------- */

                    var modelTypeStr = _modelType.Name != "Object" ? _modelType.Name : null;
                    if (_toolbar != null)
                    {


                        var toolbarCommands = new List<IDictionary<string, object>>();
                        commandsAdded = new List<GridCommandCr>();
                        if (_toolbar.Commands != null/* && !_features.ReadOnly*/)
                        {
                            _toolbar.Commands.ForEach(tc =>
                            {

                                switch (tc.Name)
                                {
                                    case GCommandCr.Create:
                                        if (_accessOperation.Insertable)
                                        {
                                            tc.GridID = ID;
                                            tc.ModelType = _modelType.FullName; //modelTypeArr[modelTypeArr.Length - 1];
                                            tc.AddEditTemplateAddress = _features.EditableConfig.CustomConfig.Template.Url;
                                            tc.CustomCommandID = "add_custom_template";
                                            if (!_hasCommonAddOrEditScriptAppended)
                                            {
                                                _hasCommonAddOrEditScriptAppended = true;
                                                tc.CommonAddOrEditScript = tc.GetCustomAddEditScript();
                                            }
                                            else
                                            {
                                                tc.CommonAddOrEditScript = string.Empty;
                                            }
                                            tc.AddCallerScript = tc.GetAddEditScriptEventListener(GCommandCr.Create);
                                            var dicItem = tc.ToJson();
                                            toolbarCommands.Add(dicItem);
                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.Create, info = tc });
                                        }
                                        break;
                                    case GCommandCr.Delete:
                                        if (_accessOperation.Removable)
                                        {
                                            tc.GridID = ID;
                                            tc.PreCommitCommandCallback = _features.PreDeletionCallback;
                                            var dicItem = tc.ToJson();
                                            toolbarCommands.Add(dicItem);
                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.Delete, info = tc });
                                        }
                                        break;
                                    case GCommandCr.Edit:
                                        if (_accessOperation.Updatable)
                                        {
                                            tc.GridID = ID;

                                            tc.ModelType = _modelType.FullName; //modelTypeArr[modelTypeArr.Length - 1];
                                            tc.AddEditTemplateAddress = _features.EditableConfig.CustomConfig.Template.Url;
                                            tc.CustomCommandID = "edit_custom_template";
                                            if (!_hasCommonAddOrEditScriptAppended)
                                            {
                                                _hasCommonAddOrEditScriptAppended = true;
                                                tc.CommonAddOrEditScript = tc.GetCustomAddEditScript();
                                            }
                                            else
                                            {
                                                tc.CommonAddOrEditScript = string.Empty;
                                            }
                                            tc.EditCallerScript = tc.GetAddEditScriptEventListener(GCommandCr.Edit);
                                            var dicItem = tc.ToJson();
                                            toolbarCommands.Add(dicItem);
                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.Edit, info = tc });
                                        }
                                        break;
                                    case GCommandCr.Search:
                                        if (_features.Searchable)
                                        {
                                            var searchCommand = new ColumnCommand { Name = GCommandCr.Search };
                                            searchCommand.GridID = ID;
                                            var dicItem = searchCommand.ToJson();
                                            toolbarCommands.Add(dicItem);
                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.Search, info = searchCommand });

                                        }

                                        break;
                                    case GCommandCr.RemoveFilter:
                                        if (_features.Searchable)
                                        {
                                            var removeSearchCommand = new ColumnCommand { Name = GCommandCr.RemoveFilter };
                                            removeSearchCommand.GridID = ID;
                                            //var jsonItem = removeSearchCommand.ToJson();
                                            //toolbarCommands.Add(jsonItem);
                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.RemoveFilter, info = removeSearchCommand });
                                        }
                                        break;

                                    case GCommandCr.Excel:
                                        if (_features.FileOutput != null)
                                        {
                                            if (_features.FileOutput.OutputType == CustomWrapper.CoreKendoGrid.Settings.Features.OutputType.Excel)
                                            {

                                                var excelCommand = new ColumnCommand { Name = GCommandCr.Excel, Text = _features.FileOutput.ButtonText ?? "" };
                                                excelCommand.GridID = ID;
                                                var dicItem = excelCommand.ToJson();
                                                toolbarCommands.Add(dicItem);

                                                var fName = _features.FileOutput.FileName ?? ID;

                                                totalConfig["excel"] = new Dictionary<string, object>() { { "allPages", _features.FileOutput.AllPages }, { "fileName", fName + ".xlsx" } };
                                                //if (_features.FileOutput.Rtl)
                                                //    totalConfig["excelExport"] = new ClientHandlerDescriptor { TemplateDelegate = obj => GetExcelExportForRTLSupport() };
                                                commandsAdded.Add(new GridCommandCr { type = GCommandCr.Excel, info = excelCommand });
                                            }
                                        }
                                        break;
                                    case GCommandCr.Pdf:

                                        if (_features.FileOutput != null)
                                        {
                                            if (_features.FileOutput.OutputType == CustomWrapper.CoreKendoGrid.Settings.Features.OutputType.Pdf)
                                            {

                                                var pdfCommand = new ColumnCommand { Name = GCommandCr.Pdf, Text = _features.FileOutput.ButtonText ?? "" };
                                                pdfCommand.GridID = ID;
                                                var dicItem = pdfCommand.ToJson();
                                                toolbarCommands.Add(dicItem);
                                                _features.FileOutput.FileName = _features.FileOutput.FileName ?? ID;

                                                totalConfig["pdf"] = _features.FileOutput.ToJson();

                                                commandsAdded.Add(new GridCommandCr { type = GCommandCr.Pdf, info = pdfCommand });
                                            }

                                        }

                                        break;

                                    case GCommandCr.Refresh:
                                        tc.HasInitialFilter = _dataSource.Filter != null ? true : false;
                                        if (_features.Refreshable)
                                        {
                                            var refreshableCommand = _toolbar.Commands.FirstOrDefault(com => com.Name == GCommandCr.Refresh);
                                            if (refreshableCommand != null)
                                            {
                                                refreshableCommand.GridID = ID;
                                                toolbarCommands.Add(refreshableCommand.ToJson());
                                                commandsAdded.Add(new GridCommandCr { type = GCommandCr.Refresh, info = refreshableCommand });
                                            }
                                        }
                                        break;
                                    case GCommandCr.Custom:

                                        if (VerifyCustomActionAuthorization(tc.UniqueName))
                                        {
                                            var customCommand = new ColumnCommand
                                            {
                                                GridID = ID,
                                                Name = GCommandCr.Custom,
                                                Text = tc.Text,
                                                UniqueName = tc.UniqueName
                                            };

                                            var dicItem = customCommand.ToJson();
                                            toolbarCommands.Add(dicItem);


                                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.Custom, info = tc });
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            });
                        }
                        if (_clientDependency != null)
                        {
                            var customActions = _clientDependency.CustomActions;
                            if (customActions.Count > 0)
                            {
                                customActions.ForEach(ca =>
                                {
                                    if (string.IsNullOrEmpty(ca.Template))
                                        if (!string.IsNullOrEmpty(ca.CommandText) && !string.IsNullOrEmpty(ca.ClickEventHandler) && !string.IsNullOrEmpty(ca.CustomActionUniqueName))
                                        {
                                            if (VerifyCustomActionAuthorization(ca.CustomActionUniqueName.Split('#')[0]))
                                            {
                                                var customCommand = new ColumnCommand() { Name = GCommandCr.Custom };
                                                customCommand.CustomCommandID = ca.ID;
                                                customCommand.GridID = ID;
                                                customCommand.ClickHandler = ca.ClickEventHandler;
                                                customCommand.Text = ca.CommandText;
                                                customCommand.CommandIconRelativePath = ca.IconRelativePath;
                                                customCommand.CssClass = ca.CssClass;
                                                toolbarCommands.Add(customCommand.ToJson());
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(ca.CustomActionUniqueName))
                                            {
                                                var customCommand = new ColumnCommand() { Name = GCommandCr.Custom };
                                                customCommand.Template = ca.Template;
                                                toolbarCommands.Add(customCommand.ToJson());
                                            }
                                        }
                                });
                            }
                        }


                        if (_accessOperation.UserGuideIncluded && modelTypeStr != null && _features.UserGuideIncluded)
                        {
                            var _constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
                            var helpUrl = string.Empty;
                            _constantService.TryGetValue<string>("HelpUrl", out helpUrl);
                            var uGuideCommand = new ColumnCommand { Name = GCommandCr.UserGuide };
                            uGuideCommand.GridID = ID;
                            uGuideCommand.ClickHandler = helpUrl /*Core.Resources.Constants.HelpUrl*/;
                            //TODO: not nead to use COC(Convention over configuration) for ViewModel
                            var viewModelModelIndex = modelTypeStr.ToLower().IndexOf("viewmodel");
                            if (viewModelModelIndex == -1)
                            {
                                var DTOlIndex = modelTypeStr.ToLower().IndexOf("dto");

                                if (DTOlIndex == -1)
                                    throw new Exception("ViewModel class Name must end by DTO or ViewModel.");
                                else
                                {
                                    uGuideCommand.ModelType = modelTypeStr.Substring(0, DTOlIndex);
                                }
                            }
                            else
                            {
                                uGuideCommand.ModelType = modelTypeStr.Substring(0, viewModelModelIndex);
                            }
                            var helpImageUrl = string.Empty;
                            _constantService.TryGetValue<string>("HelpImageUrl", out helpImageUrl);
                            uGuideCommand.CommandIconRelativePath = helpImageUrl/* Core.Resources.Constants.HelpImageUrl*/;
                            uGuideCommand.Text = "راهنما";
                            toolbarCommands.Add(uGuideCommand.ToJson());
                            commandsAdded.Add(new GridCommandCr { type = GCommandCr.UserGuide, info = uGuideCommand });

                        }



                        if (toolbarCommands.Count > 0)
                        {
                            totalConfig["toolbar"] = toolbarCommands;
                        }
                    }

                    totalConfig["columns"] = colsList;

                    /*--------------------Paging-------------- */

                    if (_features.Paging)
                    {
                        var pageable = _features.PagingInfo;
                        //if (_features.Scrollability.IsVirtual)
                        //{
                        //    pageable.PreviousNext = false;
                        //    pageable.Numeric = false;
                        //}
                        totalConfig["pageable"] = pageable.ToJson();

                        /*------------------Scrollable --------------- */

                        totalConfig["pageSize"] = _features.PageSize ?? 100;
                    }

                    /*--------------------Filterable-------------- */

                    var filterConfig = _features.Filter;
                    if (filterConfig.FilterAffection)
                    {
                        totalConfig["filterable"] = filterConfig.ToJson();
                    }

                    /*--------------------Navigatable-------------- */

                    var nav = _features.Navigatable;

                    totalConfig["navigatable"] = nav;

                    /*--------------------Sortable-------------- */
                    if (_features.Sortability.IsSortable)
                    {
                        totalConfig["sortable"] = _features.Sortability.IsSortable;// _features.Sortability.ToJson();
                    }

                    /*--------------------Groupable-------------- */
                    if (_features.Grouping)
                    {
                        totalConfig["groupable"] = _features.GroupingInfo.ToJson();
                    }
                    else
                    {
                        totalConfig["groupable"] = false;
                    }

                    /*-------------------Reorderable-------------- */

                    totalConfig["reorderable"] = _features.Reorderable;

                    /*------------------Scrollable --------------- */

                    totalConfig["scrollable"] = _features.Scrollability.ToJson();


                    /*-----------------Resizable--------------- */

                    totalConfig["resizable"] = _features.Resizable;

                    /*--------------------Aggregates-------------- */


                    /*--------------------Editable-------------- */
                    //This property is very important in Editing(delete , edit)
                    var edConfig = _features.EditableConfig;

                    if (edConfig.CustomConfig != null)
                    {
                        totalConfig["editable"] = edConfig.CustomConfig.ToJson();
                    }
                    else
                    {
                        totalConfig["editable"] = edConfig.Editable;
                    }

                    /*--------------------Selectable-------------- */
                    totalConfig["selectable"] = DefineSelectableString(_features.Selectability);
                    if (_clientDependency != null)
                    {
                        DefineEventHandlers(totalConfig);
                    }

                    /*--------------------Data Source-------------- */

                    totalConfig["dataSource"] = _dataSource.ToJson();
                    var model = _dataSource.Schema.Model;
                }
            }
            return totalConfig;
        }

        private string GetExcelExportForRTLSupport()
        {
            return "function(e) { var sheet = e.workbook.sheets[0]; " +
                   " for (var i = 0; i < sheet.rows.length; i++) { sheet.rows[i].cells.reverse(); for (var ci = 0; ci < sheet.rows[i].cells.length; ci++) { sheet.rows[i].cells[ci].hAlign = 'right'; }  } } ";

        }

        private string BuildAggregateColumnFooterTemplate(Dictionary<AggregateType, string> list)
        {
            var footerTotalTemplate = new List<string>();

            //list(l =>
            foreach (var l in list)
            {
                var aggFragment = string.Empty;
                switch (l.Key)
                {
                    case AggregateType.Count:
                        aggFragment = GetAggFragment("تعداد", AggregateType.Count, l.Value);
                        break;
                    case AggregateType.Min:
                        aggFragment = GetAggFragment("کمترین", AggregateType.Min, l.Value);
                        break;
                    case AggregateType.Max:
                        aggFragment = GetAggFragment("بیشترین", AggregateType.Max, l.Value);
                        break;
                    case AggregateType.Sum:
                        aggFragment = GetAggFragment("مجموع", AggregateType.Sum, l.Value);
                        break;
                    case AggregateType.Average:
                        aggFragment = GetAggFragment("میانگین", AggregateType.Average, l.Value);
                        break;
                    default:
                        break;
                }
                footerTotalTemplate.Add(aggFragment);
            };

            return string.Join(" ", footerTotalTemplate);



        }

        private string GetAggFragment(string localizedTitle, AggregateType aggregateType, string footerTemplateFormat)
        {
            if (string.IsNullOrEmpty(footerTemplateFormat))
            {
                return string.Format("{0}: # if(eval('{1}') < 0) {{ # <span style='color:red' > (#={1} * -1 #) </span> #}} else {{# # ={1} # #}}#", localizedTitle, aggregateType.ToString().ToLower());
            }
            return string.Format("{0}: # if(eval('{1}') < 0) {{ # <span style='color:red' > (#=kendo.toString({1} * -1,'{2}')#) </span> #}} else {{# #=kendo.toString({1},'{2}')# #}}#", localizedTitle, aggregateType.ToString().ToLower(), footerTemplateFormat);
        }

        /// <summary>
        /// مجوز مربوط به عملکرد اضافی
        /// </summary>
        /// <param name="customActionUniqueName"></param>
        /// <returns></returns>
        private bool VerifyCustomActionAuthorization(string customActionUniqueName)
        {
            return UserAccessibleElement.HasCustomActionAuthorized(customActionUniqueName);
        }
        /// <summary>
        /// رویداد های مربوط به گرید .
        /// </summary>
        /// <param name="totalConfig"></param>
        private void DefineEventHandlers(Dictionary<string, object> totalConfig)
        {
            BuildOnDataBindingEventHandler(totalConfig);
            BuildOnEditEventHandler(totalConfig);
            BuildOnChangeEventHandler(totalConfig);
            BuildOnSaveEventHandler(totalConfig);
            BuildOnCancelEventHandler(totalConfig);
            BuildOnDataBoundEventHandler(totalConfig);
        }


        #region Grid Event Handlers

        private void BuildOnChangeEventHandler(Dictionary<string, object> totalConfig)
        {
            if (!string.IsNullOrEmpty(_clientDependency.Events.OnChange))
            {
                var onChangeStr = string.Format("function {3}_onChange(e) {0} {2} {1}", "{", "}", _clientDependency.Events.OnChange + "(e);", ID);
                totalConfig["change"] = new ClientHandlerDescriptor { TemplateDelegate = obj => onChangeStr };
            }
        }

        private void BuildOnEditEventHandler(Dictionary<string, object> totalConfig)
        {
            if (!_features.ReadOnly)
            {
                if (_accessOperation.InsertableOrUpdatable)
                {
                    var initialOnEditCode = string.Format("ns_Grid.GridOperations.onEditEventHandler(e);");
                    var afterOnDataBindingCode = string.Empty;
                    var onEditStr = string.Format("function {2}_onEdit(e) {{  e.preventDefault(); {0} {1}  }}", initialOnEditCode, !string.IsNullOrEmpty(_clientDependency.Events.OnEdit) ? _clientDependency.Events.OnEdit + "(e);" : string.Empty, ID);
                    totalConfig["edit"] = new ClientHandlerDescriptor { TemplateDelegate = obj => onEditStr };
                }
            }
        }

        private void BuildOnDataBindingEventHandler(Dictionary<string, object> totalConfig)
        {
            if (!_features.ReadOnly)
            {
                var afterOnDataBindingCode = string.Empty;
                if (!string.IsNullOrEmpty(_clientDependency.Events.OnDataBinding))
                {
                    afterOnDataBindingCode = string.Format("if(typeof {0} == 'function') eval(\"{0}\");  else {0}(event);", _clientDependency.Events.OnDataBinding);
                }
                var onDataBoundingInitialStrEdit = string.Format("var grd =$('#{0}');", ID);
                var onDataBoundingStrDelete = string.Empty;
                var completeBoundStr = string.Format("function {0}_onDataBinding(event) {{ {1} {2} }}", ID, onDataBoundingInitialStrEdit, afterOnDataBindingCode);
                totalConfig["dataBinding"] = new ClientHandlerDescriptor { TemplateDelegate = obj => completeBoundStr };
            }
        }

        private void BuildOnDataBoundEventHandler(Dictionary<string, object> totalConfig)
        {
            var cellModeDblClick = string.Empty;
            var rowModeDblClick = string.Empty;

            if (!string.IsNullOrEmpty(_clientDependency.Events.OnDoubleClick) && _features.Selectability != Selectable.None)
            {
                cellModeDblClick = _features.Selectability == Selectable.Cell || _features.Selectability == Selectable.MultipleCells ? //(" ns_Grid.GridOperations.onCellDblClick('' , {0});", _clientDependency.Events.OnDoubleClick)//
                   GetCellDoubleClickScript(_clientDependency.Events.OnDoubleClick)
                  : string.Empty;
                rowModeDblClick = _features.Selectability == Selectable.Row || _features.Selectability == Selectable.MultipleRows ?
                   GetRowDoubleClickScript(_clientDependency.Events.OnDoubleClick) // string.Format(" ns_Grid.GridOperations.onRowDblClick('' ,  {0});", _clientDependency.Events.OnDoubleClick)//
                  : string.Empty;
                //$("td[role='gridcell']").on('dblclick' , function(e) {  if(typeof onDoubleClick == 'function')  eval(onDoubleClick);  else { var currentCell= $(this);  var eventArg = { doubleClickedCell : currentCell , wrappingRow : currentCell.closest("tr") }; eval(onDoubleClick + '(eventArg);'); }  });
            }
            var onDataBoundScript = string.Empty;
            if (!string.IsNullOrEmpty(_clientDependency.Events.OnDataBound))
            {
                onDataBoundScript = GetOnDataBoundScript(_clientDependency.Events.OnDataBound, "event");
            }
            totalConfig["dataBound"] = new ClientHandlerDescriptor
            {
                TemplateDelegate = obj =>
                    string.Format("function {0}_onDataBound(event) {{  {1}  {2} {3} }}", ID, cellModeDblClick + rowModeDblClick, onDataBoundScript, getKeyboardNavigation())
            };
        }

        public string GetOnDataBoundScript(string dataBoundCustomCode, string args)
        {
            return string.Format("ns_Grid.GridOperations.onDataBoundCustomCode({0} , {1});", dataBoundCustomCode, args);
        }

        public string getKeyboardNavigation()
        {
            return string.Format("ns_Grid.GridOperations.supressAnyKeyEvent('{0}');", ID);
        }

        private string GetRowDoubleClickScript(string rowDblClick)
        {
            return string.Format("ns_Grid.GridOperations.onRowDblClick(\"{0}\" , '{1}');", rowDblClick, ID);
        }

        private string GetCellDoubleClickScript(string cellDblClick)
        {
            return string.Format("ns_Grid.GridOperations.onCellDblClick(\"{0}\" , '{1}');", cellDblClick, ID);
        }

        private string GetOnInitScript(string onInit)
        {
            return string.Format("ns_Grid.GridOperations.onGridInit(\"{0}\");", onInit);
        }

        private void BuildOnCancelEventHandler(Dictionary<string, object> totalConfig)
        {

        }

        private void BuildOnSaveEventHandler(Dictionary<string, object> totalConfig)
        {
            var onSaveFinalEventStr = string.Empty;

            if (!string.IsNullOrEmpty(_clientDependency.Events.OnSave))
            {
                onSaveFinalEventStr = string.Format("function {2}_onSave(e) {0}  {3} {4}(e);  {1}", "{", "}", ID, string.Empty, _clientDependency.Events.OnSave);
            }
            else
            {
                onSaveFinalEventStr = string.Format("function {2}_onSave(e) {0}  {3} {1}", "{", "}", ID, string.Empty);
            }
            totalConfig["save"] = new ClientHandlerDescriptor { TemplateDelegate = obj => onSaveFinalEventStr };

        }

        #endregion
        /// <summary>
        /// مود انتخاب گرید
        /// </summary>
        /// <param name="selectable"></param>
        /// <returns></returns>
        private string DefineSelectableString(Selectable selectable)
        {
            var selectableStr = string.Empty;
            switch (selectable)
            {
                case Selectable.Row:
                    selectableStr = "row";
                    break;
                case Selectable.Cell:
                    selectableStr = "cell";
                    break;
                case Selectable.MultipleRows:
                    selectableStr = "multiple row";
                    break;
                case Selectable.MultipleCells:
                    selectableStr = "multiple cell";
                    break;
                case Selectable.None:
                    selectableStr = "none";
                    break;
                default:
                    selectableStr = "row";
                    break;
            }
            return selectableStr;
        }
    }
}
