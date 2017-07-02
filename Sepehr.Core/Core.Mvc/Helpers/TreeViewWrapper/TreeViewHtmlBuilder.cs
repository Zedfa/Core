using System.Collections.Generic;
using Core.Cmn.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;

namespace Core.Mvc.Helpers
{
    public class TreeViewHtmlBuilder : EssentialHtmlBuilderBase<TreeView,TreeViewItem>//NavigationHtmlBuilderBase<TreeView, TreeViewItem>
    {
        public TreeViewHtmlBuilder(TreeView treeView)
            : base(treeView)
        {
        }

        public IHtmlNode CheckboxFor(TreeViewItem item)
        {
            string[] classes = new string[] { "k-checkbox" };
            IHtmlNode parent = new HtmlElement("span").AddClass(classes);
            new HtmlElement("input", TagRenderMode.SelfClosing)
                .Attributes(new Dictionary<string, string>{{"checkbox", "checkedNodes"}})
                .ToggleAttribute("value", item.Id, item.Id != null)
                .ToggleAttribute("checked", "checked", item.Checked)
                .ToggleAttribute("disabled", "disabled", !item.Enabled).AppendTo(parent);
            return parent;
        }

        public IHtmlNode ChildrenTag(TreeViewItem item)
        {
            return base.ListTag().ToggleAttribute("style", "display:none", !item.Expanded);
        }

        public IHtmlNode IconFor(TreeViewItem item)
        {
            string[] classes = new string[] { "k-icon" };
            return new HtmlElement("span").AddClass(classes).ToggleClass("k-plus", item.Enabled && !item.Expanded).ToggleClass("k-minus", item.Enabled && item.Expanded).ToggleClass("k-plus-disabled", !item.Enabled && !item.Expanded).ToggleClass("k-minus-disabled", !item.Enabled && item.Expanded);
        }

        public IHtmlNode ItemContentTag(TreeViewItem item)
        {
            IHtmlNode node = base.ContentTag(item);
            if (!item.Expanded || !item.Enabled)
            {
                node.Attribute("style", "display:none");
            }
            return node;
        }

        public IHtmlNode ItemInnerContent(TreeViewItem item)
        {
            string str = base.Component.GetItemUrl<TreeView, TreeViewItem>(item, string.Empty);
            bool flag =  str.HasValue() ;
            IHtmlNode parent = new HtmlElement(!flag ? "span" : "a");
            if (flag && item.Enabled)
            {
                parent.Attribute("href", str);
            }
            string[] classes = new string[] { "k-in" };
            parent.Attributes<string, object>(item.LinkHtmlAttributes).PrependClass(classes).ToggleClass("k-state-disabled", !item.Enabled).ToggleClass("k-state-selected", item.Enabled && item.Selected);
            if (flag)
            {
                string[] textArray2 = new string[] { "k-link" };
                parent.PrependClass(textArray2);
            }
            if ( item.ImageUrl.HasValue())
            {
                base.ImageTag(item).AppendTo(parent);
            }
            if (item.SpriteCssClasses.HasValue())
            {
                base.SpriteTag(item).AppendTo(parent);
            }
            base.Text(item).AppendTo(parent);
            return parent;
        }

        public IHtmlNode ItemTag(TreeViewItem item, bool hasAccessibleChildren)
        {
            IHtmlNode parent = new HtmlElement("li").Attributes<string, object>(item.HtmlAttributes);
            if (item.Id.HasValue())
            {
                parent.Attribute("data-id", item.Id);
            }
            parent.ToggleAttribute("data-hasChildren", "true", item.HasChildren).ToggleAttribute("data-expanded", "true", item.Expanded);
            if (item.NextSibling == null)
            {
                string[] textArray1 = new string[] { "k-last" };
                parent.PrependClass(textArray1);
            }
            if ((item.Parent == null) && (item.PreviousSibling == null))
            {
                string[] textArray2 = new string[] { "k-first" };
                parent.PrependClass(textArray2);
            }
            string[] classes = new string[] { "k-item" };
            parent.PrependClass(classes);
            IHtmlNode node2 = new HtmlElement("div").ToggleClass("k-top", item.PreviousSibling == null).ToggleClass("k-bot", item.NextSibling == null).ToggleClass("k-mid", (item.PreviousSibling != null) && (item.NextSibling != null)).AppendTo(parent);
            if ((item.HasChildren || hasAccessibleChildren) || ( item.Template.HasValue()))
            {
                this.IconFor(item).AppendTo(node2);
            }
            if (base.Component.Checkboxes.Enabled)
            {
                this.CheckboxFor(item).AppendTo(node2);
            }
            return parent;
        }

        public IHtmlNode TreeViewTag()
        {
            IHtmlNode parent = base.ComponentTag("div");
            if (!base.Component.UsesTemplates())
            {
                string[] classes = new string[] { "k-widget", "k-treeview", "k-reset" };
                parent.PrependClass(classes);
                if (base.Component.Items.Count > 0)
                {
                    base.ListTag().AppendTo(parent);
                }
            }
            return parent;
        }
    }
}

    
