using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CoreKendoGrid.Infrastructure
{
    public interface IUrlBuilderCr
    {
         string ControllerName { get; set; }

         string ActionName { get; set; }

         string Url { get; set; }
    }
}
