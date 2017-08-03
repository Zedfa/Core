using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.DependencyInjection
{
    public class ParameterOverride
    {
        public ParameterOverride(string paramName, object paramValue)
        {
            ParamName = paramName;
            ParamValue = paramValue;
        }

        public string ParamName { get; private set; }
        public object ParamValue { get; private set; }
    }
}
