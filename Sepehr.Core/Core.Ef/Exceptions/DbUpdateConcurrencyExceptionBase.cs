using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;


namespace Core.Ef.Exceptions
{
    public class DbUpdateConcurrencyExceptionBase : Exception
    {
        private DbUpdateConcurrencyException _dbUpdateConcurrencyException;
        public DbUpdateConcurrencyExceptionBase()
        {
            _dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

        }
        public DbUpdateConcurrencyExceptionBase(Exception exception)
        {
            _dbUpdateConcurrencyException =exception as DbUpdateConcurrencyException;
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Entity.Infrastructure.DbUpdateException
        //     class.
        //
        // Parameters:
        //   message:
        //     The message.
        public DbUpdateConcurrencyExceptionBase(string message)
        {
            _dbUpdateConcurrencyException = new DbUpdateConcurrencyException(message);
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Entity.Infrastructure.DbUpdateException
        //     class.
        //
        // Parameters:
        //   message:
        //     The message.
        //
        //   innerException:
        //     The inner exception.
        public DbUpdateConcurrencyExceptionBase(string message, Exception innerException)
        {
            _dbUpdateConcurrencyException = new DbUpdateConcurrencyException(message, innerException);
        }
    }
}
