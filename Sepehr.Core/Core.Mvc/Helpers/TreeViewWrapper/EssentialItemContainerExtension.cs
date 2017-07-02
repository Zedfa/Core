using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using Core.Cmn.Extensions;
using System.Web.Mvc;
using System.Web.Routing;
using Core.Mvc.Extensions;
namespace Core.Mvc.Helpers
{
    public static class EssentialItemContainerExtension
    {
        public static string GetItemContentId<TComponent, TItem>(this TComponent component, TItem item)
            where TComponent : TreeViewBase,
            IEssentialItem<TItem>
            where TItem : EssentialItem<TItem>
        {
            return (!item.ContentHtmlAttributes.ContainsKey("id") ? "{0}-{1}".FormatWith(new object[2]) : "{0}".FormatWith(new object[] { item.ContentHtmlAttributes["id"].ToString() }));
        }

        public static string GetItemUrl<TComponent, TItem>(this TComponent component, TItem item)
            where TComponent : TreeViewBase,
            IEssentialItem<TItem>
            where TItem : EssentialItem<TItem>
        {
            return component.GetItemUrl<TComponent, TItem>(item, "#");
        }

        public static string GetItemUrl<TComponent, TItem>(this TComponent component, TItem item, string defaultValue)
            where TComponent : TreeViewBase,
            IEssentialItem<TItem>
            where TItem : EssentialItem<TItem>
        {

            string str = GenerateUrl<TItem>(item, ((TreeViewBase)component).ViewContext);
            if (str != null)
            {
                return str;
            }
            IAsyncContentContainer container = item as IAsyncContentContainer;
            if ((container != null) && container.ContentUrl.HasValue())
            {
                return (!component.IsSelfInitialized ? container.ContentUrl : HttpUtility.UrlDecode(container.ContentUrl));
            }
            if (((item.Template.HasValue() && !item.RouteName.HasValue()) && (!item.Url.HasValue() && !item.ActionName.HasValue())) && !item.ControllerName.HasValue())
            {
                return ((!component.IsInClientTemplate ? "#" : @"\#") + component.GetItemContentId<TComponent, TItem>(item));
            }
            return defaultValue;
        }
        public static string GetImageUrl<T>(this T item, ViewContext viewContext) where T : EssentialItem<T>
        {
            var urlHelper = new UrlHelper(viewContext.RequestContext);

            return urlHelper.Content(item.ImageUrl);
        }

        public static string GenerateUrl<T>(T navigationItem, RequestContext requestContext, RouteValueDictionary routeValues) where T : EssentialItem<T>
        {

            UrlHelper urlHelper = new UrlHelper(requestContext);
            string generatedUrl = null;

            if (!string.IsNullOrEmpty(navigationItem.RouteName))
            {
                generatedUrl = urlHelper.RouteUrl(navigationItem.RouteName, routeValues);
            }
            else if (!string.IsNullOrEmpty(navigationItem.ControllerName) && !string.IsNullOrEmpty(navigationItem.ActionName))
            {
                generatedUrl = urlHelper.Action(navigationItem.ActionName, navigationItem.ControllerName, routeValues, null, null);
            }
            else if (!string.IsNullOrEmpty(navigationItem.Url))
            {
                generatedUrl = navigationItem.Url.StartsWith("~/") ?
                               urlHelper.Content(navigationItem.Url) :
                               navigationItem.Url;
            }
            else if (routeValues.Any())
            {
                generatedUrl = urlHelper.RouteUrl(routeValues);
            }

            return generatedUrl;
            // return urlGenerator.Generate(viewContext.RequestContext, navigatable);
        }

        public static string GenerateUrl<T>(T navigationItem, ViewContext viewContext) where T : EssentialItem<T>
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();

            if (navigationItem.RouteValues.Any())
            {
                routeValues.Merge(navigationItem.RouteValues, true);
            }

            return GenerateUrl(navigationItem, viewContext.RequestContext, routeValues);
        }

    }

    public interface IContentContainer
    {
        // Action Content { get; set; }

        IDictionary<string, object> ContentHtmlAttributes { get; }
    }
}
