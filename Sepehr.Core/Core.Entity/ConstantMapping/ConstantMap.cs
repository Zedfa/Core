using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ConstantMapping
{
    public class ConstantMap : EntityTypeConfiguration<Constant>
    {
        public ConstantMap()
        {
             this.ToTable("Constant");

             this.HasRequired(c => c.ConstantCategory)
                 .WithMany(category => category.Constants)
                 .HasForeignKey(cf => cf.ConstantCategoryID)
                 .WillCascadeOnDelete(true); 

       }
    }
}
