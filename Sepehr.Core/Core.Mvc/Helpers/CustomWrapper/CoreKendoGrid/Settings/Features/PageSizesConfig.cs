using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class PageSizesConfig
    {
        public PageSizesConfig(CultureInfo cultureInfo)
        {
            PageSizesEnabled = false;
        }
        public bool PageSizesEnabled { get; set; }
        public int[] PageSizes { get; set; }
    }
}
