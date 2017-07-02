using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Core.Cmn.Extensions;

namespace Core.Mvc.Helpers
{
    public class EssentialItemBuilder<TItem, TBuilder>   where TItem : EssentialItem<TItem>    where TBuilder : EssentialItemBuilder<TItem, TBuilder>
    {
        private readonly EssentialItem<TItem> item;

        protected EssentialItemBuilder(EssentialItem<TItem> item, ViewContext viewContext)
        {
            this.item = item;
            this.ViewContext = viewContext;
        }

        public ViewContext ViewContext
        {
            get;
            set;
        }

        protected TItem Item
        {
            get
            {
                return item as TItem;
            }
        }

        public TItem ToItem()
        {
            return item as TItem;
        }


        //public TBuilder HtmlAttributes(object attributes)
        //{
        //    return HtmlAttributes(attributes.ToDictionary());
        //}


        public TBuilder HtmlAttributes(IDictionary<string, object> attributes)
        {

            item.HtmlAttributes.Clear();
            item.HtmlAttributes.Merge(attributes);

            return this as TBuilder;
        }

        //public TBuilder LinkHtmlAttributes(object attributes)
        //{
        //    return LinkHtmlAttributes(attributes.ToDictionary());
        //}

        public TBuilder LinkHtmlAttributes(IDictionary<string, object> attributes)
        {

            item.LinkHtmlAttributes.Clear();
            item.LinkHtmlAttributes.Merge(attributes);

            return this as TBuilder;
        }

        public TBuilder Text(string value)
        {
            item.Text = value;

            return this as TBuilder;
        }

        public TBuilder Visible(bool value)
        {
            item.Visible = value;

            return this as TBuilder;
        }

        public TBuilder Enabled(bool value)
        {
            item.Enabled = value;

            return this as TBuilder;
        }

        public TBuilder Selected(bool value)
        {
            item.Selected = value;

            return this as TBuilder;
        }

       
        //public TBuilder Route(string routeName, RouteValueDictionary routeValues)
        //{
        //    item.Route(routeName, routeValues);

        //    SetTextIfEmpty(routeName);

        //    return this as TBuilder;
        //}


        //public TBuilder Route(string routeName, object routeValues)
        //{
        //    item.Route(routeName, routeValues);

        //    SetTextIfEmpty(routeName);

        //    return this as TBuilder;
        //}


        //public TBuilder Route(string routeName)
        //{
        //    return Route(routeName, (object)null);
        //}


        //public TBuilder Action(RouteValueDictionary routeValues)
        //{
        //    item.Action(routeValues);

        //    if (item.ActionName.HasValue())
        //    {
        //        SetTextIfEmpty(item.ActionName);
        //    }

        //    return this as TBuilder;
        //}


        //public TBuilder Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        //{
        //    item.Action(actionName, controllerName, routeValues);

        //    SetTextIfEmpty(actionName);
        //    return this as TBuilder;
        //}


        //public TBuilder Action(string actionName, string controllerName, object routeValues)
        //{
        //    item.Action(actionName, controllerName, routeValues);
        //    SetTextIfEmpty(actionName);

        //    return this as TBuilder;
        //}


        //public TBuilder Action(string actionName, string controllerName)
        //{
        //    return Action(actionName, controllerName, (object)null);
        //}


        //public TBuilder Url(string value)
        //{
        //    item.Url(value);

        //    return this as TBuilder;
        //}

        public TBuilder ImageUrl(string value)
        {

            item.ImageUrl = value;

            return this as TBuilder;
        }


        public TBuilder ImageHtmlAttributes(object attributes)
        {
            return ImageHtmlAttributes(attributes.ToDictionary());
        }

       
        public TBuilder ImageHtmlAttributes(IDictionary<string, object> attributes)
        {

            item.ImageHtmlAttributes.Clear();
            item.ImageHtmlAttributes.Merge(attributes);

            return this as TBuilder;
        }

        public TBuilder SpriteCssClasses(params string[] cssClasses)
        {
            Item.SpriteCssClasses = String.Join(" ", cssClasses);

            return this as TBuilder;
        }
       
        public TBuilder Content(Action value)
        {

            Item.Template.Content = value;

            return this as TBuilder;
        }

        public TBuilder Content(Func<object, object> value)
        {

            Item.Template.InlineTemplate = value;

            return this as TBuilder;
        }
    
        public TBuilder Content(string value)
        {

            Item.Template.Html = value;

            return this as TBuilder;
        }

        
        public TBuilder ContentHtmlAttributes(object attributes)
        {
            return ContentHtmlAttributes(attributes.ToDictionary());
        }

        
        public TBuilder ContentHtmlAttributes(IDictionary<string, object> attributes)
        {

            item.ContentHtmlAttributes.Clear();
            item.ContentHtmlAttributes.Merge(attributes);

            return this as TBuilder;
        }

     
        //public TBuilder Action<TController>(Expression<Action<TController>> controllerAction) where TController : Controller
        //{
        //    item.Action(controllerAction);
        //    return this as TBuilder;
        //}

        public TBuilder Encoded(bool isEncoded)
        {
            item.Encoded = isEncoded;

            return this as TBuilder;
        }

        private void SetTextIfEmpty(string value)
        {
            if (string.IsNullOrEmpty(item.Text))
            {
                item.Text = value;
            }
        }
    }
}