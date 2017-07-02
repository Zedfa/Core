using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig
{
    [Serializable()]
    public class GridCommandCr {
        public GCommandCr type { get; set; }
        public ColumnCommand info { get; set; }
    }
    [Serializable()]
    public enum GCommandCr
    {
        Create ,
        Edit ,
        Delete , 
        Refresh ,
        Search ,
        RemoveFilter,
        Custom ,
        UserGuide , 
        Excel , 
        Pdf
    }
}
