using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ConstantMapping
{
    public class ConstantCategoryMap : EntityTypeConfiguration<ConstantCategory>
    {
        public ConstantCategoryMap()
        {
            this.ToTable("ConstantCategory");

            //this.HasKey(cc => new { cp.RoleId, cp.CompanyId });

            //this.HasRequired(t => t.BusinessEntity)


            this.HasKey(cat => cat.ID)
                .HasMany(cat => cat.Constants);
                
                   //.WillCascadeOnDelete(true);

            //this.HasRequired(t => t.Role)
            //     .WithMany(t => t.CompanyRoles)
            //     .HasForeignKey(d => d.RoleId)
            //     .WillCascadeOnDelete(true);
        }
    }
}
