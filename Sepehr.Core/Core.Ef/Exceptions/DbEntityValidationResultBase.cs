using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Core.Ef.Exceptions
{
    public class DbEntityValidationResultBase
    {
        private System.Data.Entity.Validation.DbEntityValidationResult _dbEntityValidationResult;

        public DbEntityValidationResultBase(System.Data.Entity.Validation.DbEntityValidationResult dbEntityValidationResult)
        {
            _dbEntityValidationResult = dbEntityValidationResult;
        }
        //
        // Summary:
        //     Creates an instance of System.Data.Entity.Validation.DbEntityValidationResult
        //     class.
        //
        // Parameters:
        //   entry:
        //     Entity entry the results applies to. Never null.
        //
        //   validationErrors:
        //     List of System.Data.Entity.Validation.DbValidationError instances. Never null.
        //     Can be empty meaning the entity is valid.
        //public DbEntityValidationResultBase(IDbEntityEntryBase entry, IEnumerable<DbValidationErrorBase> validationErrors)
        //{
        //    _dbEntityValidationResult = new DbEntityValidationResult(entry, validationErrors);

        //}

        //
        // Summary:
        //     Gets an instance of System.Data.Entity.Infrastructure.DbEntityEntry the results
        //     applies to.
        public IDbEntityEntryBase Entry
        {
            get
            {

                return new DbEntityEntryBase(_dbEntityValidationResult.Entry);
            }
        }
        //
        // Summary:
        //     Gets an indicator if the entity is valid.
        public bool IsValid
        {
            get
            {
                return _dbEntityValidationResult.IsValid;
            }
        }
        //
        // Summary:
        //     Gets validation errors. Never null.
        public ICollection<DbValidationErrorBase> ValidationErrors { get; }
    }
}
