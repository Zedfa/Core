namespace Kendo.Mvc.UI.Html
{
    using System;
    using System.Collections.Generic;
    
    public class GridGroupingData
    {
        public IGridUrlBuilder UrlBuilder
        {
            get;
            set;
        }

        public IEnumerable<GroupDescriptor> GroupDescriptors
        {
            get;
            set;
        }

        public Func<string, string> GetTitle
        {
            get;
            set;
        }

        public GroupableMessages Messages
        {
            get;
            set;
        }
    }
}