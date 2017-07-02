using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Core.Ef.Exceptions
{
    public class DbEntityValidationExceptionBase:Exception
    {
        private System.Data.Entity.Validation.DbEntityValidationException _dbEntityValidationException;
        //
        // Summary:
        //     Initializes a new instance of DbEntityValidationException.
        public DbEntityValidationExceptionBase()
        {
            _dbEntityValidationException = new DbEntityValidationException();
        }
        //
        // Summary:
        //     Initializes a new instance of DbEntityValidationException.
        //
        // Parameters:
        //   message:
        //     The exception message.
        public DbEntityValidationExceptionBase(string message)
        {
            _dbEntityValidationException = new DbEntityValidationException();

        }
        public DbEntityValidationExceptionBase(Exception exception) {
            _dbEntityValidationException = exception as DbEntityValidationException;
        }
        //
        // Summary:
        //     Initializes a new instance of DbEntityValidationException.
        //
        // Parameters:
        //   message:
        //     The exception message.
        //
        //   entityValidationResults:
        //     Validation results.
        public DbEntityValidationExceptionBase(string message, IEnumerable<DbEntityValidationResultBase> entityValidationResults)
        {
            //_dbEntityValidationException = new DbEntityValidationException(message, entityValidationResults);
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Initializes a new instance of DbEntityValidationException.
        //
        // Parameters:
        //   message:
        //     The exception message.
        //
        //   innerException:
        //     The inner exception.
        public DbEntityValidationExceptionBase(string message, Exception innerException)
        {
            _dbEntityValidationException = new DbEntityValidationException(message, innerException);

        }
        //
        // Summary:
        //     Initializes a new instance of DbEntityValidationException.
        //
        // Parameters:
        //   message:
        //     The exception message.
        //
        //   entityValidationResults:
        //     Validation results.
        //
        //   innerException:
        //     The inner exception.
        public DbEntityValidationExceptionBase(string message, IEnumerable<DbEntityValidationResultBase> entityValidationResults, Exception innerException)
        {
            // _dbEntityValidationException = new DbEntityValidationException(message, entityValidationResults, innerException);
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Validation results.
        public IEnumerable<DbEntityValidationResultBase> EntityValidationErrors
        {
            get
            {

                return _dbEntityValidationException.EntityValidationErrors.Select(error=>new DbEntityValidationResultBase(error));
            }
        }
    }
}
