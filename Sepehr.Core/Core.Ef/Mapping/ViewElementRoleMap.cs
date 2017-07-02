using Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Mapping
{

    public class ViewElementRoleMap : EntityTypeConfiguration<ViewElementRole>
    {

        public ViewElementRoleMap()
        {

            this.HasRequired(t => t.ViewElement)
                .WithMany(t => t.ViewElementRoles)
                .HasForeignKey(d => d.ViewElementId)
                .WillCascadeOnDelete(true);

            this.HasRequired(t => t.Role)
                .WithMany(t => t.ViewElementRoles)
                .HasForeignKey(d => d.RoleId)
                .WillCascadeOnDelete(true);
        }
    }
}
