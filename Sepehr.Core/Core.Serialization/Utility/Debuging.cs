using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Utility
{
    public static class Debuging
    {
        public static void WriteLine(object value)
        {
#if DEBUGOUTPUT
            System.Diagnostics.Debug.WriteLine(value);
#endif
        }
    }
}
