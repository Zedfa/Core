using EdgeJs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class EdgeJs
    {
        public static async Task<object> CallFunction(string function, string arg)
        {
            var func = Edge.Func(function);
            var result = await func(arg);
            return result;
        }
    }
}
