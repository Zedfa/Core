using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq;
using Kendo.Mvc.Extensions;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.ViewModel;
namespace Core.Mvc.Helpers
{
    public class TreeView : TreeViewBase, IEssentialItem<TreeViewItem>
    {
        //internal bool isPathHighlighted;

        public TreeView(ViewContext viewContext, Kendo.Mvc.Infrastructure.IJavaScriptInitializer initializer,TreeInfo info , bool hasCheckBox, ViewDataDictionary viewData = null) 
            : base(viewContext, initializer)
        {
            this.Name = info.Name;

            this.DragAndDrop = false;

            this.Items = new LinkedObjectCollection<TreeViewItem>(null);

            this.SelectedIndex = -1;

            this.LoadOnDemand = true;

            this.AutoBind = info.AutoBind;

            dynamic vmValues = null;

            if (info.DataSource.ModelCr.ModelType.Equals(typeof(TreeViewModelBase)))
            {
                info.DataSource.ModelCr.ModelType = typeof(TreeViewModelBase);

                vmValues = new TreeViewModelBase();
            }

            else if (ViewData.Model != null)
            {
                vmValues = ViewData.Model;
            }

            this.DataSource = new TreeViewDataSource(info, this.Name, info.DataSource.ModelCr.ModelType);




            // this.DataSource = new TreeViewDataSource(info.DataSource, this.Name , ViewData.Model );

            if (hasCheckBox)
            {
                this.Checkboxes = new TreeViewCheckboxesSettings();
            }
            // this.SecurityTrimming = new Kendo.Mvc.UI.SecurityTrimming();
            //this.UrlGenerator = urlGenerator;
            //this.Authorization = authorization;
        }



        //public INavigationItemAuthorization Authorization { get; private set; }
        // public TreeViewCheckboxesSettings Checkboxes { get; set; }
        // public Kendo.Mvc.UI.Effects Effects { get; set; }
        // public Kendo.Mvc.UI.SecurityTrimming SecurityTrimming { get; set; }

        public bool AutoBind { get; set; }

        public string DataTextField { get; set; }

        public string DataUrlField { get; set; }

        public string DataImageUrlField { get; set; }

        public TreeViewDataSource DataSource { get; private set; }

        public string DataSpriteCssClassField { get; set; }

        public bool DragAndDrop { get; set; }


        public bool ExpandAll { get; set; }

        public bool HighlightPath { get; set; }

        public IList<TreeViewItem> Items { get; private set; }

        public bool LoadOnDemand { get; set; }

        public TreeViewCheckboxesSettings Checkboxes { get; set; }

        public int SelectedIndex { get; set; }

        public string Template { get; set; }

        public string TemplateId { get; set; }

        private void ExpandAllChildrens(TreeViewItem treeViewItem)
        {
            treeViewItem.Expanded = true;
            foreach (TreeViewItem item in treeViewItem.Items)
            {
                this.ExpandAllChildrens(item);
            }
        }

        internal bool UsesTemplates()
        {
            bool hasTemplate = false;

            if (this.TemplateId.HasValue() || this.Template.HasValue())
            {
                hasTemplate = true;
            }

            if (this.Checkboxes != null)
            {
                if ((this.Checkboxes.Template as string) != "<input type='checkbox' name='checkedNodes' #= item.checked ? 'checked' : '' # value='#= item.id #' />")
                    hasTemplate = true;
            }
            //return ((this.TemplateId.HasValue() || this.Template.HasValue()) || ((this.Checkboxes.Template as string) != "<input type='checkbox' name='checkedNodes' #= item.checked ? 'checked' : '' # value='#= item.id #' />"));
            return hasTemplate;
        }

        private void WriteItem(TreeViewItem item, IHtmlNode parentTag, TreeViewHtmlBuilder builder)
        {
            //if (ItemAction != null)
            //{
            //    ItemAction(item);
            //}
            if (item.Visible)
            {
                var accessible = true;
                //if (this.SecurityTrimming.Enabled)
                //{
                //    accessible = item.IsAccessible(Authorization, ViewContext);
                //}


                //if (accessible)
                //{
                //    var hasAccessibleChildren = item.Items.Any(x => x.Visible);
                //    if (hasAccessibleChildren && this.SecurityTrimming.Enabled)
                //    {
                //        hasAccessibleChildren = item.Items.IsAccessible(Authorization, ViewContext);

                //        if (this.SecurityTrimming.HideParent && !hasAccessibleChildren)
                //        {
                //            return;
                //        }
                //    }
                if (accessible)
                {
                    var hasAccessibleChildren = item.Items.Any(x => x.Visible);
                    if (!hasAccessibleChildren)
                        return;


                    IHtmlNode itemTag = builder.ItemTag(item, hasAccessibleChildren).AppendTo(parentTag);

                    builder.ItemInnerContent(item).AppendTo(itemTag.Children[0]);

                    if (item.Template.HasValue())
                    {
                        builder.ItemContentTag(item).AppendTo(itemTag);
                    }
                    else if (hasAccessibleChildren)
                    {
                        IHtmlNode ul = builder.ChildrenTag(item).AppendTo(itemTag);

                        item.Items.Each(child => WriteItem(child, ul, builder));
                    }
                }
            }
        }

        //private IEnumerable SerializeItems(IList<TreeViewItem> items)
        //{
        //    Dictionary<string, object> jsonData = new Dictionary<string, object>();
        //    if (this.Items.Any())
        //    {

        //        TreeViewItem.SetItemsTreeToJson(jsonData, this.Items);
        //        jsonData["items"] = this.Items;
        //    }
        //    return jsonData;
        //}

        protected override void WriteHtml(HtmlTextWriter writer)
        {

            var builder = new TreeViewHtmlBuilder(this);

            IHtmlNode treeViewTag = builder.TreeViewTag();

            if (Items.Any())
            {
                if (SelectedIndex != -1 && Items.Count < SelectedIndex)
                {
                    throw new ArgumentOutOfRangeException("شاخص خارج از محدوده داده های موجود است");
                }

                //this loop is required because of SelectedIndex feature.
                //if (HighlightPath)
                //{
                //    Items.Each(HighlightSelectedItem);
                //}

                Items.Each((item, index) =>
                {
                    if (!this.HighlightPath)
                    {
                        if (index == this.SelectedIndex)
                        {
                            item.Selected = true;

                            if (item.Items.Any() || item.Template.HasValue())
                            {
                                item.Expanded = true;
                            }
                        }
                    }

                    if (item.HasChildren)
                    {
                        item.Expanded = false;
                    }

                    if (ExpandAll)
                    {
                        ExpandAllChildrens(item);
                    }

                    if (string.IsNullOrEmpty(item.Id))
                    {
                        item.Id = item.Text;
                    }

                    WriteItem(item, treeViewTag.Children[0], builder);
                });
            }

            treeViewTag.WriteTo(writer);

            base.WriteHtml(writer);
        }

        public override void WriteInitializationScript(TextWriter writer)
        {
            var options = new Dictionary<string, object>(Events);


            if (DataSource.Transport.Read.Url.HasValue())
            {
                options["dataSource"] = DataSource.ToJson();

            }
            else if (DataSource.Data != null)
            {
                options["dataSource"] = DataSource.Data;
            }

            if (DragAndDrop)
            {
                options["dragAndDrop"] = true;
            }

            if (AutoBind)
            {
                options["autoBind"] = true;
            }
            else
            {
                options["autoBind"] = false;
            }

            if (!LoadOnDemand)
            {
                options["loadOnDemand"] = false;
            }

            //else
            //{
            //    options["loadOnDemand"] = true;
            //}

            if (DataTextField.HasValue())
            {
                options["dataTextField"] = DataTextField;
            }

            if (DataUrlField.HasValue())
            {
                options["dataUrlField"] = DataUrlField;
            }

            if (DataSpriteCssClassField.HasValue())
            {
                options["dataSpriteCssClassField"] = DataSpriteCssClassField;
            }

            if (DataImageUrlField.HasValue())
            {
                options["dataImageUrlField"] = DataImageUrlField;
            }

            var idPrefix = "#";
            if (IsInClientTemplate)
            {
                idPrefix = "\\" + idPrefix;
            }

            if (TemplateId.HasValue())
            {
                options["template"] = new ClientHandlerDescriptor { HandlerName = string.Format("$('{0}{1}').html()", idPrefix, TemplateId) };
            }
            else if (Template.HasValue())
            {
                options["template"] = Template;
            }

            if (Checkboxes != null)
            {
                var checkboxes = Checkboxes.ToJson();

                if (checkboxes.Keys.Any())
                {
                    options["checkboxes"] = checkboxes["checkboxes"];
                }
            }
            //var animation = Animation.ToJson();

            //if (animation.Keys.Any())
            //{
            //    options["animation"] = animation["animation"];
            //}

            writer.Write(Initializer.Initialize(Selector, "TreeView", options));

            base.WriteInitializationScript(writer);
        }


    }

}
