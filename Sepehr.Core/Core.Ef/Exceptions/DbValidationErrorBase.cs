using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Exceptions
{
    public class DbValidationErrorBase
    {
        private DbValidationError _dbValidationError;
        //
        // Summary:
        //     Validation error. Can be either entity or property level validation error.

        //
        // Summary:
        //     Creates an instance of System.Data.Entity.Validation.DbValidationError.
        //
        // Parameters:
        //   propertyName:
        //     Name of the invalid property. Can be null.
        //
        //   errorMessage:
        //     Validation error message. Can be null.
        public DbValidationErrorBase(string propertyName, string errorMessage)
        {
            _dbValidationError = new DbValidationError(propertyName, errorMessage);
        }

        //
        // Summary:
        //     Gets validation error message.
        public string ErrorMessage {
            get
            {
                return _dbValidationError.ErrorMessage;
            }
        }
        //
        // Summary:
        //     Gets name of the invalid property.
        public string PropertyName
        {
            get
            {
                return _dbValidationError.PropertyName;
            }
        }

    }
}
