using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
   
    public interface IClientEventBehavior
    {
        string FuncBlock { get; set; }
        bool CancelDefaultBehavior { get; set; }
    }
}
