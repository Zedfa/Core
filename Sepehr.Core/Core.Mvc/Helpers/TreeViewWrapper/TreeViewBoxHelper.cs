using System.Web.Mvc;
using System;
using Core.Cmn;


namespace Core.Mvc.Helpers
{

    public static class TreeViewBoxHelper
    {
        private readonly static Service.IConstantService _constantService;
        static TreeViewBoxHelper()
        {
            _constantService = AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();

        }
        public static MvcHtmlString TreeViewBoxCr<TModel>(this HtmlHelper helper, TreeInfo info, string name, TreeViewEventBuilder events
            , bool hasCheckbox) where TModel : IViewModel, new()
        
        {

            var treeViewHtmlString = helper.TreeViewCr<TModel>(info, name, events, hasCheckbox).ToHtmlString();

            return CreateToolBox<TModel>(helper, info, name, treeViewHtmlString);
        }

        public static MvcHtmlString TreeViewBoxCr<TModel>(this HtmlHelper helper, TreeInfo info, string name, TreeViewEventBuilder events) where TModel : IViewModel, new()
        {

            var treeViewHtmlString = helper.TreeViewCr<TModel>(info, name, events).ToHtmlString();

            return CreateToolBox<TModel>(helper, info, name, treeViewHtmlString);
        }

        private static MvcHtmlString CreateToolBox<TModel>(HtmlHelper helper, TreeInfo info, string treeName, string ComponentHtmlString, string templateName, int? width, int? height)
            where TModel : IViewModel
        {


            // var jsonModel = new ModelDescriptor(typeof(TModel)).ToJson();

            //  string serializedModel = new Kendo.Mvc.Infrastructure.JavaScriptInitializer().Serialize(jsonModel);

            TagBuilder boxBuilder = new TagBuilder("div");

            boxBuilder.AddCssClass(StyleKind.BoxBoarder);


            TagBuilder toolBarBuilder = new TagBuilder("div");

            toolBarBuilder.AddCssClass(StyleKind.TreeToolbar);


            if (!info.Operation.ReadOnly)
            {
               
                ElementAuthentication.UserAccessibleElement.DefineCrudActionAuthority( info.Operation, info.DataSource.CrudCr);
                if (info.Operation.Insertable)
                {
                     
                    string insertScript = CreateTemplateScriptForCRUD(treeName, typeof(TModel).Name, "Insert"/*Constants.Insert*/, info.TemplateInfo, Enum.GetName(typeof(HttpVerbs), HttpVerbs.Post));

                    MvcHtmlString insertButton = helper.IconButtonCr("btnInsert_tree_" + treeName + "_ns", "Insert"/*Constants.Insert*/, string.Format("{0} {1}", StyleKind.CommandButton, "rp-tree-add")
                        , StyleKind.Icons.Add, insertScript);

                    toolBarBuilder.InnerHtml += insertButton.ToHtmlString();
                }

                if (info.Operation.Updatable)
                {
                   
                   

                    string updateScript = CreateTemplateScriptForCRUD(treeName, typeof(TModel).Name, "Edit"/*Constants.Edit*/, info.TemplateInfo, Enum.GetName(typeof(HttpVerbs), HttpVerbs.Put));

                    MvcHtmlString editButton = helper.IconButtonCr("btnUpdate_tree_" + treeName + "_ns", "Edit"/*Constants.Edit*/, string.Format("{0} {1}", StyleKind.CommandButton, "rp-tree-edit")
                        , StyleKind.Icons.Edit, updateScript);

                    toolBarBuilder.InnerHtml += editButton.ToHtmlString();
                }

                if (info.Operation.Removable)
                {
                   
                    string deleteScript = CreateTemplateScriptForCRUD(treeName, typeof(TModel).Name, "Delete"/*Constants.Delete*/, info.TemplateInfo, Enum.GetName(typeof(HttpVerbs), HttpVerbs.Delete));

                    MvcHtmlString deletetButton = helper.IconButtonCr("btnDelete_tree_" + treeName + "_ns", "Delete"/*Constants.Delete*/, string.Format("{0} {1}", StyleKind.CommandButton, "rp-tree-delete")
                        , StyleKind.Icons.Delete, deleteScript);

                    toolBarBuilder.InnerHtml += deletetButton.ToHtmlString();
                }


            }

            //------Refresh--------
            string refreshScript = "<script> $(document).ready( function(){$('#btnRefresh_tree_" + treeName + "_ns').click( function(){" +
                                                                               "TreeView.refresh(" + treeName + ");});" +
                                                                  "}); </script>";

            MvcHtmlString refreshButton = helper.IconButtonCr("btnRefresh_tree_" + treeName + "_ns", string.Format("{0} {1}", StyleKind.CommandButton, "rp-tree-refresh")
                , StyleKind.Icons.Refresh, refreshScript);

            toolBarBuilder.InnerHtml += refreshButton.ToHtmlString();
            //-----------------

            //---------Help
          
            string helpScript = CreateTemplateScriptForCRUD(treeName, typeof(TModel).Name, "Help" /*Constants.Help*/, info.TemplateInfo, "Help");

            MvcHtmlString helpButton = helper.ImageLinkButtonCr("btnHelp_tree_" + treeName + "_ns", "Help"/* Constants.Help*/,"#", "HelpImageUrl"/*Constants.HelpImageUrl*/
                , string.Format("{0} {1}", StyleKind.CommandButton, "rp-tree-Help"), helpScript);

            toolBarBuilder.InnerHtml += helpButton.ToHtmlString();
            //------------------

            TagBuilder emptyHeaderBuilder = new TagBuilder("div");

            emptyHeaderBuilder.AddCssClass(StyleKind.HeaderRow);
            var msg = string.Empty;
            _constantService.TryGetValue<string>("ThereIsntAnyData", out msg);
            emptyHeaderBuilder.SetInnerText(msg/*Messages.ThereIsntAnyData*/);


            boxBuilder.InnerHtml = toolBarBuilder.ToString() + emptyHeaderBuilder.ToString() + ComponentHtmlString;//+ globalTreeScript;

            return MvcHtmlString.Create(boxBuilder.ToString());
        }

        private static MvcHtmlString CreateToolBox<TModel>(HtmlHelper helper, TreeInfo info, string treeName, string ComponentHtmlString)
            where TModel : IViewModel
        {
            return CreateToolBox<TModel>(helper, info, treeName, ComponentHtmlString, string.Empty, null, null);
        }

        private static MvcHtmlString CreateToolBox<TModel>(HtmlHelper helper, TreeInfo info, string treeName, string ComponentHtmlString, int width, int height)
            where TModel : IViewModel
        {
            return CreateToolBox<TModel>(helper, info, treeName, ComponentHtmlString, string.Empty, width, width);
        }

        private static string CreateTemplateScriptForCRUD(string treeName, string modelName, string title, Template tempInfo, string httpVerb)
        {
            string buttonName = string.Empty;
            switch (httpVerb)
            {
                case "Post":
                    buttonName = "btnInsert_tree_" + treeName + "_ns";
                    break;

                case "Put":
                    buttonName = "btnUpdate_tree_" + treeName + "_ns";
                    break;

                case "Delete":
                    buttonName = "btnDelete_tree_" + treeName + "_ns";
                    break;

                case "Help":
                    buttonName = "btnHelp_tree_" + treeName + "_ns";
                    break;
                
            }

            var text = string.Empty;
            _constantService.TryGetValue<string>(title, out text);
            var helpUrl = string.Empty;
            _constantService.TryGetValue<string>("HelpUrl", out helpUrl);
            return "<script> $(document).ready( function(){" +
                                                                               "$('#"+ buttonName+"').click( function(){" +

                                                                                string.Format("Template.show( '{0}','{1}','{2}','{3}','{4}','{5}',{6},{7} );"

                                                                                , modelName

                                                                                 , text

                                                                                 , treeName

                                                                                 , tempInfo.Name

                                                                                 , httpVerb

                                                                                 , helpUrl/*Constants.HelpUrl*/

                                                                                 , tempInfo.Width

                                                                                 , tempInfo.Height
                                                                                 ) +

                                                                                                                         "});" +

                                                                                  "}); </script>";

        }
    }
}
