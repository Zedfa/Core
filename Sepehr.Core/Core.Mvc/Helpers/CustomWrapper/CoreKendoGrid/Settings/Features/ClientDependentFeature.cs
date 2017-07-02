using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.Features;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    [Serializable()]
    public class ClientDependentFeature
    {
        public ClientDependentFeature()
        {
            HtmlAttributes = new Dictionary<string, object>();
            ReadQueryParams = new Dictionary<string, object>();
            Events = new ClientEvent();
            CssStyles = new Dictionary<string, string>();
            CustomActions = new List<CustomActionInfo>();
        }
        public Dictionary<string, object> ReadQueryParams { get; private set; }
        public string PreReadFunction { get; set; }

        public Dictionary<string , object> HtmlAttributes { get;private set; }
        public Dictionary<string , string> CssStyles { get; set; }
        public ClientEvent Events { get;private set; }
        public List<CustomActionInfo> CustomActions { get;private set; }

        public string ReadFilterObject { get; set; }
        
    }
}
