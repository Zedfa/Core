namespace Kendo.Mvc.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Infrastructure;
    using Kendo.Mvc.UI.Html;

    public abstract class GridActionCommandBase : IGridActionCommand
    {
        public virtual string Name
        {
            get;
            set;
        }

        public GridButtonType ButtonType
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
        
        public IDictionary<string, object> HtmlAttributes
        {
            get;
            set;
        }

        //TODO: Implement command button image html attributes
        /*
        public IDictionary<string, object> ImageHtmlAttributes
        {
            get;
            set;
        }
        */
        public GridActionCommandBase()
        {
            ButtonType = GridButtonType.ImageAndText;
            HtmlAttributes = new RouteValueDictionary();
          //  ImageHtmlAttributes = new RouteValueDictionary();
        }

        public virtual IDictionary<string, object> Serialize(IGridUrlBuilder urlBuilder)
        {
            var command = new Dictionary<string, object>();

            FluentDictionary.For(command)
                .Add("name", Name)                
                .Add("attr", HtmlAttributes.ToAttributeString(), HtmlAttributes.Any)
                .Add("buttonType", ButtonType.ToString())
                .Add("text", Text, (System.Func<bool>)Text.HasValue);
                //TODO: Implement command button image html attributes
                //.Add("imageAttr", ImageHtmlAttributes.ToAttributeString(), ImageHtmlAttributes.Any);

            return command;
        }

        protected T CreateButton<T>(string text, string @class) where T : IGridButtonBuilder, new()
        {
            var factory = new GridButtonFactory();
            var button = factory.CreateButton<T>(ButtonType);

            button.Text = text;           
            button.HtmlAttributes = HtmlAttributes;
            //TODO: Implement command button image html attributes
            //button.ImageHtmlAttributes = ImageHtmlAttributes;
            button.CssClass += " " + @class;

            return button;
        }

        public abstract IEnumerable<IGridButtonBuilder> CreateDisplayButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper);

        public virtual IEnumerable<IGridButtonBuilder> CreateEditButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper)
        {
            return new IGridButtonBuilder[0];
        }
        
        public virtual IEnumerable<IGridButtonBuilder> CreateInsertButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper)
        {
            return new IGridButtonBuilder[0];
        }
    }
}
