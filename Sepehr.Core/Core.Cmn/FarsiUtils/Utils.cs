using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Cmn.FarsiUtils
{
    internal static class Util
    {
        /// <summary>
        /// Adds a preceding zero to single day or months
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static string toDouble(int i)
        {
            if (i > 9)
            {
                return i.ToString();
            }
            else
            {
                return "0" + i.ToString();
            }
        }
    }
}
