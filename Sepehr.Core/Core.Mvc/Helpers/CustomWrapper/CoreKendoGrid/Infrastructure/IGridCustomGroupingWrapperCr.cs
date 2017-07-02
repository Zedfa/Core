using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Infrastructure
{
    public interface IGridCustomGroupingWrapperCr
    {
        IEnumerable GroupedEnumerable { get; }
    }
}
