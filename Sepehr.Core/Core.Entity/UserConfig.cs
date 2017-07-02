using Core.Cmn;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Entity
{

    [Table("UserConfigs", Schema = "core")] 
    [DataContract]
    public class UserConfig : EntityBase<UserConfig>
    {
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Key, Column(Order = 2)]
        public string ConfigKey { get; set; }
        [ForeignKey("ConfigKey")]
        public virtual Config Config { get; set; }

        [MaxLength(50)]
        public string ConfigValue { get; set; }

    }
}
