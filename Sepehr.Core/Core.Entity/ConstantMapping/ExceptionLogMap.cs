using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ConstantMapping
{
    public class ExceptionLogMap : EntityTypeConfiguration<ExceptionLog>
    {
        public ExceptionLogMap()
        {
            //this.HasKey(excLog => excLog.ID)
            //    .HasOptional(excLog => excLog.InnerException)
            //    .WithMany()
            //    .HasForeignKey(excLog => excLog.ExceptionLogId);

            this.HasRequired(excLog => excLog.EventLog)
                .WithRequiredDependent(logE => logE.ExceptionLog);

            this.Property(excLog => excLog.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }
    }
}
