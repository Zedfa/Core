namespace Kendo.Mvc.UI
{
    using System.Collections.Generic;
    using Kendo.Mvc.UI.Html;
using System.Web.Mvc;

    public interface IGridActionCommand
    {
        IDictionary<string, object> Serialize(IGridUrlBuilder urlBuilder);

        GridButtonType ButtonType { get; }
        
        string Name
        {
            get;
        }

        IDictionary<string, object> HtmlAttributes
        {
            get;
        }

        //TODO: Implement command button image html attributes
        //IDictionary<string, object> ImageHtmlAttributes
        //{
        //    get;
        //}

        IEnumerable<IGridButtonBuilder> CreateDisplayButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper);

        IEnumerable<IGridButtonBuilder> CreateEditButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper);
        
        IEnumerable<IGridButtonBuilder> CreateInsertButtons(IGridUrlBuilder urlBuilder, IGridHtmlHelper htmlHelper);
    }
}