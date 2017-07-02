using Core.Cmn;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Configs", Schema = "core")] 
   
    public class Config : EntityBase<Config>
    {
        [MaxLength(50)]
        [Key]
        public string ConfigKey { get; set; }

        public virtual ICollection<UserConfig> UserConfigs { get; set; }
    }
}
