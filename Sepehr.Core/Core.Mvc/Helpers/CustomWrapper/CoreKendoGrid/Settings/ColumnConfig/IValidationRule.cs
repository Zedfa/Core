using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.CoreKendoGrid.Settings.ColumnConfig
{

    public interface IValidationRule 
    {
        IDictionary<string, object> ToJson();
       
    }
}
