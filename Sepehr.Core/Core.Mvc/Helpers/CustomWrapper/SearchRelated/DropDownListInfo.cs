using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Helpers.CustomWrapper.SearchRelated
{
    [Serializable]
    public class DropDownListInfo
    {
        public string DisplayName { get; set; }

        public string ValueName { get; set; }

        public string Url { get; set; }

        public string PropertyNameForBinding { get; set; }
        public string DBCategoryName { get; set; }

    }
}