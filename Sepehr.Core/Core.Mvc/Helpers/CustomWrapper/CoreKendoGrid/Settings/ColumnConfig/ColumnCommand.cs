using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using Kendo.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Mvc.Helpers.CoreKendoGrid
{   
    [Serializable()]
    [DataContract(IsReference = true)]  
    public class ColumnCommand : JsonObjectBase
    {
        public GCommandCr? Name { get; set; }
        public string Text { get; set; }
        public string ClickHandler { get; set; }

        public string PreCommitCommandCallback { get; set; }
        public string CommandIconRelativePath { get; set; }
        public string Template { get; set; }
        public string CustomCommandID { get; set; }
        public string GridID { get; set; }
        public string AddEditTemplateAddress { get; set; }
        public string ModelType { get; set; }
        public string CommonAddOrEditScript { get; set; }
        public string EditCallerScript { get; set; }
        public string AddCallerScript { get; set; }
        public Dictionary<string,object> HtmlAttributes { get; set; }

        public string CssClass { get; set; }

        public bool HasInitialFilter { get; set; }
        public string UniqueName { get; set; }
        //public Settings.Features.CustomActionInfo CustomAction { get;set;}
        protected override void Serialize(IDictionary<string, object> json)
        {

            if (Name != null)
            {
                switch (Name)
                {
                    case GCommandCr.Create:
                        if (CustomCommandID == "add_custom_template")//add
                        {
                            var customAddHtml = string.Format("<a id=\"{0}_ns_btn_{1}\"  class=\"{3}\"  href=\"\"  ><span class=\"{4}\" ></span>{2}</a>",
                                GridID,
                                "ad",
                                Text,
                                StyleKind.CommandButton,
                                StyleKind.Icons.Plus );
                            json["template"] = customAddHtml + AddCallerScript ; 
                        }
                        break;
                    case GCommandCr.Edit:
                        if (CustomCommandID == "edit_custom_template")//edit
                        {
                            var customEditHtml = string.Format("<a id=\"{0}_ns_btn_{1}\"  class=\"{3}\"   href=\"\"  ><span class=\"{4}\" ></span>{2}</a>",
                                GridID,
                                "ed",
                                Text,
                                StyleKind.CommandButton,
                                StyleKind.Icons.Edit);
                            json["template"] = customEditHtml + EditCallerScript;
                        }
                        break;
                    case GCommandCr.Delete:
                        //json["name"] = "destroy";  $(document).ready(function() {0} $('#k_del_btn_{2}').on('click' , function() {0} alert('ok'); {1}); {1});
                        json["template"] = string.Format("<a id=\"k_del_btn_{2}\"  class=\"{6}\"  href=\"\" ><span class=\"{7}\" ></span>{3}</a><script>  $(\"[id='k_del_btn_{2}']\").on('click' , function() {0} var gridD = $(\"[id='{2}']\").data('kendoGrid'); var currentRow = gridD.select().closest('tr');  if(gridD.dataItem(currentRow)) {0} var delTemp={4}; $(\"[id='k_del_btn_{2}']\").append(delTemp); {5}  {1}  {1}); <\\/script>",
                            "{" ,
                            "}" ,
                            GridID ,
                            Text ,
                            GetDeleteTemplate(GridID) ,
                            GetRemovalScript() ,
                            StyleKind.CommandButton,
                            StyleKind.Icons.Delete);
                        break;
                    case GCommandCr.Refresh:
                        json["template"] = string.Format("<a id=\"k_ref_btn_{0}\"  class=\"{1}\"  href=\"\" ><span  class=\"{2}\" ></span></a> <script> {3} </script>",
                            GridID , StyleKind.CommandButton , StyleKind.Icons.Refresh , getRefreshButtonAction(GridID));
                        break;
                    case GCommandCr.Custom:
                        var customScript = string.Format("<script> $(\"[id='k_custom_btn_{2}_{3}']\").on('click' , function(e) {0} eval(\"{4}\"); {1}); <\\/script>", "{", "}", GridID, CustomCommandID, ClickHandler);//string.Format("background-image : url('{0}') !important;" , CommandIconRelativePath)  <style> .k-i-custom-rp {0} background-image : url('{2}') !important; z-index:11111; width:16px; hieght:16px; {1} </style>
                        if (string.IsNullOrEmpty(Template))
                        {
                            json["template"] = string.Format("<a id=\"k_custom_btn_{0}_{1}\" style=\"text-align:center !important;  \" class=\"{5}\" href=\"\" >{2}{3}</a>{4}",
                                GridID,
                                CustomCommandID,
                                Text,
                                string.IsNullOrEmpty(CommandIconRelativePath) ? string.Empty
                                : string.Format("<img src=\"{2}\" style=\"vertical-align:middle; margin-right:3.5px !important;\" alt=\"\" />", "{", "}",
                                CommandIconRelativePath),
                                customScript,
                                StyleKind.CommandButton,
                                string.IsNullOrEmpty(CssClass) ? StyleKind.Icons.Setting : CssClass);
                        }
                        else
                        {
                            json["template"] = Template;
                        }

                        break;
                    case GCommandCr.UserGuide :
                        var userGuideScript = string.Format("<script> $(\"[id='k_help_btn_{0}']\").on('click' , function(e) {{ {1} }}); <\\/script>", GridID, GetUserGuideScript(ModelType, ClickHandler, "راهنما"));
                        json["template"] = string.Format("<a id=\"k_help_btn_{0}\"  class=\"{3}\" href=\"\" ><img src='{4}' class='k-icon' alt='' \\></span>{1}</a>{2}",
                                GridID,
                                Text,
                                userGuideScript,
                                StyleKind.CommandButton,
                                CommandIconRelativePath);
                        break;
                    case GCommandCr.Search:
                        var searchScript = GetSearchModalLoaderScript();
                        json["template"] = string.Format("<a id=\"{0}_ns_btn_{1}\"  class=\"{4}\" href=\"\" ><span class=\"{5}\" ></span>{2}</a>{3}{6}",
                                GridID,
                                "srch",
                                "جستجو",
                                searchScript,
                                StyleKind.CommandButton,
                                StyleKind.Icons.Search,
                                string.Empty);
                        break;
                    case GCommandCr.Excel :
                        json["name"]=  this.Name.ToString().ToLower();
                        json["text"]=  this.Text;
                        break;
                      case GCommandCr.Pdf:
                        json["name"] = this.Name.ToString().ToLower();
                        json["text"] = this.Text;
                        break;
                    default:
                        break;
                }
            }
            //json["text"] = Text;
            if (!string.IsNullOrEmpty(ClickHandler))
            {
                //json["click"] = new ClientHandlerDescriptor { TemplateDelegate = obj => ClickHandler }; 
            }
        }

        private string getRefreshButtonAction(string gId)
        {
            return string.Format(" $('\\\\#k_ref_btn_{2}').on('click' , function(e) {0} e.preventDefault(); if({3}) {{ ns_Search.setGridInitialFilterRule('{2}',null); ns_Grid.GridOperations.doWithInitialOrClearFilter('{2}' , true); }} else  {{ if(ns_Grid.GridOperations.ifRefreshCanApplyAccordingToFilter('{2}')) {{ ns_Grid.GridOperations.doWithInitialOrClearFilter('{2}' , false);  }} else {{ ns_Grid.GridOperations.doWithInitialOrClearFilter('{2}' , true); }}  }} {1});", "{", "}", gId, HasInitialFilter ? "true" : "false");
        }
        public string GetUserGuideScript(string viewModel , string helpControllerAddress , string helpModalCaption)
        {
        //    $.ajax({
        //    type: "GET",
        //    url: "@Url.Action("CreateHelpView", "Organization")",
        //    cache: false,           
        //    success: function (response) {             
        //        DialogBox.ShowNotify("راهنما", response , 400, 400);
        //    },

        //    error: function () {
        //        alert('@Titles.ErrorOccurd');
        //    }
        //});  
            return string.Format("$.ajax({{ type: 'GET' , url: '{0}' , data: {1} , contentType: 'application/json; charset=utf-8' , cache: false,  success: function (response) {{  DialogBox.ShowNotify('{2}', response , 400, 400); }} , error: function(err){{  }}  }});", helpControllerAddress, "{ viewModelName : '" + viewModel + "'}", helpModalCaption );
        }
        public string GetCustomAddEditScript()
        {
            return string.Format("ns_get_ad_Template('{0}')", GridID);
        }

        #region Search
        public string GetSearchModalScript()
        {
            return string.Format("<script> var se_mod = <\\/script>",
                                 "{",
                                 "}",
                                 GridID);
        }

        public string GetSearchModalLoaderScript()
        {
            return string.Format("<script> $(\"[id='{2}_ns_btn_{3}'] \").on('click' , function() {0} ns_Search.loadGridSearch('{2}'); {1}); <\\/script>",
                                "{",
                                "}",
                                 GridID,
                                 "srch");

        }
        #endregion

        public string GetAddEditScriptEventListener(GCommandCr buttonType)
        {
            var addListener = string.Format("ns_Grid.GridOperations.gridAdTemplate('{0}' , '{1}' , '{2}' , '{3}');" , AddEditTemplateAddress, ModelType, "a", GridID);
            var editListener = string.Format("ns_Grid.GridOperations.gridAdTemplate('{0}' , '{1}' , '{2}' , '{3}');", AddEditTemplateAddress, ModelType, "e", GridID);
            return string.Format("<script> $('\\\\#{0}_ns_btn_{1}').on('click' , function(e) {{  if($(this).attr('disabled')!== 'disabled') {2} }}); <\\/script>",
                GridID,
                buttonType == GCommandCr.Create ? "ad" : "ed",
                buttonType == GCommandCr.Create ? addListener : editListener);
        }

        private string GetDeleteTemplate(string Id, string OkButoonText = null, string CancelButtonText = null)
        {
            return string.Format("ns_Grid.GridOperations.gridRemovalTemplate('{0}');" , GridID);
        }

        public  string GetRemovalScript()
        {

            var removeModalShow = string.Format(" var {2}_wnd; {2}_wnd= $(\"[id='wm_{2}']\").kendoWindow({0} " +
                                               "title: '{3}' ," +
                                               "modal: true," +
                                               "visible: false," +
                                               "resizable: false," +
                                               "width: 400" +
                                               "{1}).data('kendoWindow'); " +
                                               "{2}_wnd.center();" +
                                                " {2}_wnd.open();" +
                                                "$(\"[id='wm_{2}_btn_yes']\").click(function() {0} " +
                                                " {4} " +
                                                " {2}_wnd.close(); " +
                                                "{1}); " +
                                                " $(\"[id='wm_{2}_btn_no']\").click(function() {0} " +
                                                " {2}_wnd.close();" +
                                                "{1});", "{", "}", GridID, "حذف" , GetRemovalCommitmentExpression());
            return removeModalShow;
            //return string.Format("ns_Grid.GridOperations.setRemovalWindowTemplate('{0}' , '{1}');", GridID, "حذف");
 
        }

        private string GetRemovalCommitmentExpression()
        {
            if(string.IsNullOrEmpty(this.PreCommitCommandCallback))
            {
                return "gridD.removeRow(currentRow);";
            }

            return string.Format("if(eval('{0}')) {{ debugger; var model= gridD.dataItem(currentRow); gridD.dataSource.remove(model);  }}", this.PreCommitCommandCallback + "(" + this.GridID + ")" );
        }
    }

    public enum CommandType
    {
        Ordinary,
        Custom
    }
}
