using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers
{
    public class TreeViewBuilder : TreeViewBuilderBase<TreeView, TreeViewBuilder>
    {
        private TreeView Component;
        public TreeViewBuilder(TreeView component)
            : base(component)
        {
            this.Component = component;
        }

        public IDictionary<string, object> Events { get; private set; }


        public TreeViewBuilder DataImageUrlField(string field)
        {
            base.TreeViewEntity.DataImageUrlField = field;
            return this;
        }

        public TreeViewBuilder DataSpriteCssClassField(string field)
        {
            base.TreeViewEntity.DataSpriteCssClassField = field;
            return this;
        }

        public TreeViewBuilder DataTextField(string field)
        {
            base.TreeViewEntity.DataTextField = field;
            return this;
        }

        public TreeViewBuilder DataUrlField(string field)
        {
            base.TreeViewEntity.DataUrlField = field;
            return this;
        }

        public TreeViewBuilder DragAndDrop(bool value)
        {
            base.TreeViewEntity.DragAndDrop = value;
            return this;
        }

        public TreeViewBuilder ExpandAll(bool value)
        {
            base.TreeViewEntity.ExpandAll = value;
            return this;
        }

        public TreeViewBuilder HighlightPath(bool value)
        {
            base.TreeViewEntity.HighlightPath = value;
            return this;
        }
        
        public TreeViewBuilder LoadOnDemand(bool value)
        {
            base.TreeViewEntity.LoadOnDemand = value;
            return this;
        }

        public TreeViewBuilder Template(string template)
        {
            base.TreeViewEntity.Template = template;
            return this;
        }

        public TreeViewBuilder TemplateId(string templateId)
        {
            base.TreeViewEntity.TemplateId = templateId;
            return this;
        }

        public TreeViewBuilder AutoBind(bool autoBind)
        {
            base.TreeViewEntity.AutoBind = autoBind;
            return this;
        }

        //public TreeViewBuilder SecurityTrimming(bool value)
        //{
        //    base.SecurityTrimming.Enabled = value;
        //    return this;
        //}

        //public TreeViewBuilder SecurityTrimming(Action<SecurityTrimmingBuilder> securityTrimmingAction)
        //{
        //    securityTrimmingAction(new SecurityTrimmingBuilder(base.Component.SecurityTrimming));
        //    return this;
        //}


    }
}
