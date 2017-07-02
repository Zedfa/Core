
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
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
using System.Web.Mvc;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;



namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class DataSourceGridCr : DataSourceBase   
    {
        public DataSourceGridCr(Type modelMetaData = null)
        {
            ServerAggregates = true;
            ServerGrouping = true;
            ServerFiltering = true;
            ServerPaging = true;
            ServerSorting = true;
            Schema = new SchemaGrid(modelMetaData);
        }

        

        public PreDefinedEvents PreDefinedEvents { get; set; }
        

        //protected override void Serialize(IDictionary<string, object> json)
        //{
        //    base.Serialize(json);
            
        //}

        
        
        public override object BuildDefaultOnRequestEndEvent(string customOnRequestEndFunction)
        {
            var OnRequestEndDef = string.Empty;

            OnRequestEndDef = string.Format("function {2}_requestEnd(e) {0} " +
                //" var grid = $('#{2}').data('kendoGrid');  $('#{2}_loadingSpinner').hide(30000);" +
                                                    " {3} " +
                                                    "{1}"
                                                    , "{", "}", ID, customOnRequestEndFunction != string.Empty ? "eval('" + customOnRequestEndFunction + "(e);');" : string.Empty);

            return OnRequestEndDef;
        }


        public override object BuildDefaultOnRequestStartEvent(string customOnRequestStartFunction)
        {
            var OnRequestStartDef = string.Empty;

            OnRequestStartDef = string.Format("function {2}_requestStart(e) {0} " + //kendo.ui.progress($('.k-loading-mask'), true);$('#{2}').css('position' ,'absolute');
                //" var grid = $('#{2}').data('kendoGrid'); $('#{2}_loadingSpinner').show(); $('.k-loading-mask').css('z-index' ,'10000'); $('.k-loading-mask').css('position' ,'relative');" +
                                                    "if({2}_gridInitialized) {0}  {1} " +
                                                    " {3} " +
                                                    " {1} "
                                                    , "{", "}", ID, customOnRequestStartFunction != string.Empty ? customOnRequestStartFunction + "(e);" : string.Empty);

            return OnRequestStartDef;
        }

        public override object BuildDefaultOnErrorEvent(string customOnErrorFunction)
        {
            var onErrorFunctionDef = string.Empty;

            onErrorFunctionDef = string.Format("function {2}_Error(args) {0} " +
                                                    " var grid = $('#{2}').data('kendoGrid'); " +
                                                    " var msg = args.xhr.responseText; " +
                                                    " var object = $.parseJSON(args.xhr.responseText); " +
                                                    "if (args.xhr.status == 500)   " +
                                                       " grid.cancelChanges();" +
                                                    " for(var error in object.ModelState) {0}  " +
                                                    " showMessage_{2}(grid.editable.element, error, object.ModelState[error]); " +
                                                    "{1} " +
                                                    "{3}" +
                                                    "{1}"
                                                    , "{", "}", ID, customOnErrorFunction != string.Empty ? customOnErrorFunction + "(e);" : string.Empty);

            return onErrorFunctionDef;
        }
        //protected override void DoCustomSerialization(IDictionary<string, object> json)
        //{
        //   if (Events.Keys.Any())
        //   {
        //       SerializeDataSourceEvents(Events, json);
        //   }
        //}

        
    }
}
