using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace Core.Mvc.Helpers
{
    public abstract class EssentialHtmlBuilderBase<TComponent, TItem> : IEssentialHtmlBuilder<TComponent, TItem> where TComponent : TreeViewBase,
        IEssentialItem<TItem>
        where TItem : EssentialItem<TItem>
        
    {
        public EssentialHtmlBuilderBase(TComponent component )
        {
            this.Component = component;
        }
        public TComponent Component { get; private set; }

       public IHtmlNode ContentTag(TItem item)
       {
           string[] classes = new string[] { "k-content" };
           IHtmlNode node = new HtmlElement("div").Attributes<string, object>(item.ContentHtmlAttributes).PrependClass(classes).Attribute("id", this.Component.GetItemContentId<TComponent, TItem>(item));
           if ( item.Template.HasValue())
           {
               item.Template.Apply(node);
           }
           return node;
       }

       public IHtmlNode ListTag()
       {
           string[] classes = new string[] { "k-group" };
           return new HtmlElement("ul").AddClass(classes);
       }

       public IHtmlNode ComponentTag(string tagName)
       {
           return new HtmlElement(tagName).Attribute("id", this.Component.Id).Attributes<string, object>(this.Component.HtmlAttributes);
       }

       public IHtmlNode SpriteTag(TItem item)
       {
           string[] classes = new string[] { "k-sprite", item.SpriteCssClasses };
           return new HtmlElement("span").AddClass(classes);
       }
       public IHtmlNode Text(TItem item)
       {
           if (item.Encoded)
               return new TextNode(item.Text);
           else
               return new LiteralNode(item.Text);
       }
       public IHtmlNode ImageTag(TItem item)
       {
           string[] classes = new string[] { "k-image" };
           return new HtmlElement("img", TagRenderMode.SelfClosing)
               .Attribute("alt", "image", false).Attributes<string, object>(item.ImageHtmlAttributes)
               .PrependClass(classes).Attribute("src", item.GetImageUrl<TItem>(this.Component.ViewContext));
       }
      

        
    }
}
