using Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users","core")
           .HasOptional(x => x.UserProfile)
           .WithRequired(x => x.User)
           .WillCascadeOnDelete();
        }
       
    }
}
