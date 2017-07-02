using System;

namespace Core.Cmn.FarsiUtils.Exceptions
{
    public class InvalidPersianDateException : Exception
    {
        public InvalidPersianDateException()
            : base()
        {
        }

        public InvalidPersianDateException(string message)
            : base(message)
        {
        }
    }
}
