using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public static class IntegerExt
    {
        public static string FixNumber(this long number, int desiredLength)
        {
            string stringNumber = number.ToString();

            if (stringNumber.Length < desiredLength)
            {
                while (stringNumber.Length < desiredLength)
                {
                    stringNumber = "0" + stringNumber;
                }
            }

            return stringNumber;
        }

        public static string FixNumber(this int number, int desiredLength)
        {
            return Convert.ToInt64(number).FixNumber(desiredLength);
        }
    }
}
