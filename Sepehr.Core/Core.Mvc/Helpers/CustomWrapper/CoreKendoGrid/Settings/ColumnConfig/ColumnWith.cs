using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    [Serializable()]
    public enum ColumnWith
    {
        /// <summary>
        /// When Scrolling is enabled.
        /// </summary>
        Fixed,
        /// <summary>
        /// When Scrolling is disabled and no width is defined.
        /// </summary>
        Auto
    }
}
