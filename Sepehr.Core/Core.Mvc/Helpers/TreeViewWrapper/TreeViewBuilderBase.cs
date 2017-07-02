using System.Collections.Generic;
using System.Web;
using Core.Cmn.Extensions;
using System.Web.Mvc;

namespace Core.Mvc.Helpers
{
    public class TreeViewBuilderBase<TView, TBuilder> : IHtmlString//, IHideObjectMembers
        where TView : TreeViewBase
        where TBuilder : TreeViewBuilderBase<TView, TBuilder>
    {
        public bool HasDeferredInitialization { get; set; }

        private TView treeViewComponent;

        public TreeViewBuilderBase(TView info)
        {
            this.treeViewComponent = info;
        }

        public TView ToComponent()
        {
            return treeViewComponent;
        }

        public virtual TBuilder Deferred()
        {
            this.HasDeferredInitialization = true;
            return (this as TBuilder);
        }

        protected internal TView TreeViewEntity
        {
            get
            {
                return this.treeViewComponent;
            }
            set
            {
                this.treeViewComponent = value;
            }
        }

        public virtual TBuilder HtmlAttributes(IDictionary<string, object> attributes)
        {
           
            treeViewComponent.HtmlAttributes.Clear();
            treeViewComponent.HtmlAttributes.Merge(attributes,true);

            return this as TBuilder;
        }

        public virtual void Render()
        {
            treeViewComponent.Render();
        }

        public string ToHtmlString()
        {
            return ToComponent().ToHtmlString();
        }

        public MvcHtmlString ToClientTemplate()
        {
            return ToComponent().ToClientTemplate();
        }

        
    }
}
