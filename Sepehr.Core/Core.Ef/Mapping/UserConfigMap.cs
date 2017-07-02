using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef.Mapping
{
    

    public class UserConfigMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserConfig>
    {
        public UserConfigMap()
        {

            this.HasKey(cp => new { cp.UserId, cp.ConfigKey });

            this.HasRequired(t => t.User)
                   .WithMany(t => t.UserConfigs)
                   .HasForeignKey(d => d.UserId)
                   .WillCascadeOnDelete(true);


            this.HasRequired(t => t.Config)
                 .WithMany(t => t.UserConfigs)
                 .HasForeignKey(d => d.ConfigKey)
                 .WillCascadeOnDelete(false);


        }

    }
}
