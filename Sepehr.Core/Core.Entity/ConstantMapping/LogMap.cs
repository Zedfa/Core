using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ConstantMapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.HasKey(log => log.Id)
                .HasRequired(log => log.ExceptionLog)
                .WithRequiredPrincipal(logE => logE.EventLog);

            this.Property(log => log.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
