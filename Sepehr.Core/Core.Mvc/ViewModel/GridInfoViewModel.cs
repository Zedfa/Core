using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;

namespace Core.Mvc.ViewModel
{
    public class GridInfoViewModel
    {
        public string GridName { get; set; }

        public string ModelTypeFullName { get; set; }

        public GridInfo GridInfo { get; set; }
    }
}
