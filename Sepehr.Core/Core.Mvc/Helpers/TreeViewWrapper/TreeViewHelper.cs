using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Mvc.Helpers;
using Kendo.Mvc.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Kendo.Mvc.UI;
using Core.Cmn;
namespace Core.Mvc.Helpers
{
    public static class TreeViewHelper
    {

        public static MvcHtmlString TreeViewCr(this HtmlHelper helper, TreeInfo info, string name, TreeViewEventBuilder events, bool hasCheckBox)
        {
            info.DataSource.ModelCr.ModelType = typeof(TreeViewModelBase);

            var initializer = DI.Current.Resolve<IJavaScriptInitializer>();

            TreeView tree;

            info.Name = name;

            tree = new TreeView(helper.ViewContext, initializer, info, hasCheckBox);

            if (events != null)
            {
                tree.Events = events.handler;
            }

            tree.DataTextField = info.DataTextField;

            var builder = new TreeViewBuilder(tree);

            return MvcHtmlString.Create(builder.ToHtmlString());

        }

        public static MvcHtmlString TreeViewCr(this HtmlHelper helper, TreeInfo info, string name, bool hasCheckBox)
        {

            return TreeViewCr(helper, info, name, null, hasCheckBox);

        }
   
        public static MvcHtmlString TreeViewCr<TModel>(this HtmlHelper helper, TreeInfo info, string name, TreeViewEventBuilder events, bool hasCheckBox) where TModel : IViewModel, new()
        {
            info.DataSource.ModelCr.ModelType = typeof(TModel);

            var initializer = DI.Current.Resolve<IJavaScriptInitializer>();

            info.Name = name;

            TreeView tree = new TreeView(helper.ViewContext, initializer, info, hasCheckBox);

            tree.Events = events.handler;

            tree.DataTextField = info.DataTextField;

            var builder = new TreeViewBuilder(tree);

            return MvcHtmlString.Create(builder.ToHtmlString());

        }


        public static MvcHtmlString TreeViewCr<TModel>(this HtmlHelper helper, TreeInfo info, string name, TreeViewEventBuilder events) where TModel : IViewModel, new()
        {
            return TreeViewCr<TModel>(helper, info, name, events, false);

        }

    }
}
