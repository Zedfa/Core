using Core.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.Contracts;

namespace Core.Ef.Mapping
{
    
   public class UserRoleMap: EntityTypeConfiguration<UserRole>
    {
       
        public UserRoleMap()
       {
          this.ToTable("UserRoles","core") 
          .HasKey(cp => new { cp.UserId, cp.RoleID });

           this.HasRequired(t => t.User)
                  .WithMany(t => t.UserRoles)
                  .HasForeignKey(d => d.UserId)
                  .WillCascadeOnDelete(true);



           this.HasRequired(t => t.Role)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(d => d.RoleID)
                .WillCascadeOnDelete(true);

       }
    }
}
