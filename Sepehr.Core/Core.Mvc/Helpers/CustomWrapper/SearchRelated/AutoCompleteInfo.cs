using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Helpers.CustomWrapper.SearchRelated
{
    [Serializable]
    public class AutoCompleteInfo
    {
        public string DisplayName { get; set; }

        public string ValueName { get; set; }

        public string SearchProperty { get; set; }

        public string Url { get; set; }

        public string PropertyNameForBinding { get; set; }

        public string Watermark { get; set; }
          
    }
}