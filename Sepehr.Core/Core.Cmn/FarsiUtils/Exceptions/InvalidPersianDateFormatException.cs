using System;

namespace Core.Cmn.FarsiUtils.Exceptions
{
    public class InvalidPersianDateFormatException : Exception
    {
        public InvalidPersianDateFormatException(string message)
            : base(message)
        {
        }

        public InvalidPersianDateFormatException()
            : base()
        { 
        }
    }
}
