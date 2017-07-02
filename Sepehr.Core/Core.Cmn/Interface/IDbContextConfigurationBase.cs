using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IDbContextConfigurationBase
    {
        //
        // Summary:
        //     Gets or sets a value indicating whether the System.Data.Entity.Infrastructure.DbChangeTracker.DetectChanges
        //     method is called automatically by methods of System.Data.Entity.DbContext and
        //     related classes. The default value is true.
         bool AutoDetectChangesEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the value that determines whether SQL functions and commands should
        //     be always executed in a transaction.
        //
        // Remarks:
        //     This flag determines whether a new transaction will be started when methods such
        //     as System.Data.Entity.Database.ExecuteSqlCommand(System.String,System.Object[])
        //     are executed outside of a transaction. Note that this does not change the behavior
        //     of System.Data.Entity.DbContext.SaveChanges.
         bool EnsureTransactionsForFunctionsAndCommands { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether lazy loading of relationships exposed
        //     as navigation properties is enabled. Lazy loading is enabled by default.
         bool LazyLoadingEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether or not the framework will create instances
        //     of dynamically generated proxy classes whenever it creates an instance of an
        //     entity type. Note that even if proxy creation is enabled with this flag, proxy
        //     instances will only be created for entity types that meet the requirements for
        //     being proxied. Proxy creation is enabled by default.
         bool ProxyCreationEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether database null semantics are exhibited
        //     when comparing two operands, both of which are potentially nullable. The default
        //     value is false. For example (operand1 == operand2) will be translated as: (operand1
        //     = operand2) if UseDatabaseNullSemantics is true, respectively (((operand1 = operand2)
        //     AND (NOT (operand1 IS NULL OR operand2 IS NULL))) OR ((operand1 IS NULL) AND
        //     (operand2 IS NULL))) if UseDatabaseNullSemantics is false.
         bool UseDatabaseNullSemantics { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether tracked entities should be validated
        //     automatically when System.Data.Entity.DbContext.SaveChanges is invoked. The default
        //     value is true.
         bool ValidateOnSaveEnabled { get; set; }
        
      
    }
}
