namespace Kendo.Mvc.UI
{
    using System.Web.Mvc;
    using System.Collections.Generic;

    public interface IChart : ISeriesContainer
    {
        /// <summary>
        /// The component UrlGenerator
        /// </summary>
        IUrlGenerator UrlGenerator
        {
            get;
        }

        /// <summary>
        /// The component view context
        /// </summary>
        ViewContext ViewContext
        {
            get;
        }

        IList<ChartPane> Panes
        {
            get;
        }
    }
}
