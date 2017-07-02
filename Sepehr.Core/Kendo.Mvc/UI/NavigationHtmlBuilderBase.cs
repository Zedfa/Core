namespace Kendo.Mvc.UI
{
    using System;
    using System.Web.Mvc;

    public abstract class NavigationHtmlBuilderBase<TComponent, TItem> : INavigationHtmlBuilder<TComponent, TItem>
        where TComponent : WidgetBase, INavigationItemComponent<TItem>
        where TItem : NavigationItem<TItem>
    {
        public NavigationHtmlBuilderBase(TComponent component)
        {
            Component = component;
        }

        public TComponent Component
        {
            get;
            private set;
        }
        
        public IHtmlNode ListTag()
        {
            return new HtmlElement("ul")
                .AddClass(UIPrimitives.Group);
        }

        public IHtmlNode ComponentTag(string tagName)
        {
            return new HtmlElement(tagName)
                .Attribute("id", Component.Id)
                .Attributes(Component.HtmlAttributes);
        }

        public IHtmlNode ImageTag(TItem item)
        {
            return new HtmlElement("img", TagRenderMode.SelfClosing)
                    .Attribute("alt", "image", false)
                    .Attributes(item.ImageHtmlAttributes)
                    .PrependClass(UIPrimitives.Image)
                    .Attribute("src", item.GetImageUrl(((WidgetBase)Component).ViewContext));
        }

        public IHtmlNode Text(TItem item)
        {
            if (item.Encoded)
                return new TextNode(item.Text);
            else
                return new LiteralNode(item.Text);
        }

        public IHtmlNode SpriteTag(TItem item)
        {
            return new HtmlElement("span")
                .AddClass(UIPrimitives.Sprite, item.SpriteCssClasses);
        }

        public IHtmlNode ContentTag(TItem item)
        {
            var content = new HtmlElement("div").Attributes(item.ContentHtmlAttributes)
                .PrependClass(UIPrimitives.Content)
                .Attribute("id", Component.GetItemContentId(item));
            
            if (item.Template.HasValue())
            {
                item.Template.Apply(content);
            }

            return content;
        }

        public IHtmlNode ListItemTag(TItem item, Action<IHtmlNode> configure)
        {
            IHtmlNode li = new HtmlElement("li")
                .Attributes(item.HtmlAttributes);

            if (!item.Enabled)
            {
                li.PrependClass(UIPrimitives.DisabledState);
            }
            else
            {
                configure(li);
            }

            return li.PrependClass(UIPrimitives.Item);
        }

        public IHtmlNode LinkTag(TItem item, Action<IHtmlNode> configure)
        {
            var url = Component.GetItemUrl(item);

            IHtmlNode a;

            if (url != "#")
            {
                a = new HtmlElement("a");

                a.Attribute("href", url);
            }
            else
            {
                a = new HtmlElement("span");
            }

            a.Attributes(item.LinkHtmlAttributes);

            configure(a);

            a.PrependClass(UIPrimitives.Link);

            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                ImageTag(item).AppendTo(a);
            }

            if (!string.IsNullOrEmpty(item.SpriteCssClasses))
            {
                SpriteTag(item).AppendTo(a);
            }

            Text(item).AppendTo(a);

            return a;
        }
    }
}
