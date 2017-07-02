using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class CustomActionInfo
    {
        public CustomActionInfo()
        {

        }
        public string ID { get; set; }
        public string ClickEventHandler { get; set; }
        public string CommandText { get; set; }
        public string IconRelativePath { get; set; }
        public string Template { get; set; }
        public string CustomActionUniqueName { get; set; }
        public string CssClass { get; set; }
        //public string MyProperty { get; set; }
    }
}
