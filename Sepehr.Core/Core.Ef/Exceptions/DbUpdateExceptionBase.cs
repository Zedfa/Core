using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Exceptions
{
    public class DbUpdateExceptionBase : Exception
    {
        private DbUpdateException _dbUpdateException;
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Entity.Infrastructure.DbUpdateException
        //     class.
        public DbUpdateExceptionBase()
        {
            _dbUpdateException = new DbUpdateException();
        }
        public DbUpdateExceptionBase(Exception exception)
        {
            _dbUpdateException = exception as DbUpdateException;
        }
        //
        // Summary:
        //     Initializes a new instance of the System.Data.Entity.Infrastructure.DbUpdateException
        //     class.
        //
        // Parameters:
        //   message:
        //     The message.
        public DbUpdateExceptionBase(string message)
        {
            _dbUpdateException = new DbUpdateException(message);
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
        public DbUpdateExceptionBase(string message, Exception innerException)
        {
            _dbUpdateException = new DbUpdateException(message, innerException);

        }

        //
        // Summary:
        //     Gets System.Data.Entity.Infrastructure.DbEntityEntry objects that represents
        //     the entities that could not be saved to the database.
        //
        // Returns:
        //     The entries representing the entities that could not be saved.
        public IEnumerable<DbEntityEntryBase> Entries { get; }
    }
}
