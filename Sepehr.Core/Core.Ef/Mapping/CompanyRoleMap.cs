using Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;


namespace Core.Ef.Mapping
{
    public class CompanyRoleMap : EntityTypeConfiguration<CompanyRole>
    {
        public CompanyRoleMap()
        {
            this.HasKey(cp => new { cp.RoleId, cp.CompanyId });

            this.HasRequired(t => t.Company)
                   .WithMany(t => t.CompanyRoles)
                   .HasForeignKey(d => d.CompanyId)
                   .WillCascadeOnDelete(true);

            this.HasRequired(t => t.Role)
                 .WithMany(t => t.CompanyRoles)
                 .HasForeignKey(d => d.RoleId)
                 .WillCascadeOnDelete(true);

        }
    }
}
