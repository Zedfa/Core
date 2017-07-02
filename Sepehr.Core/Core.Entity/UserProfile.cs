using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;
using System;
using Core.Cmn;
using System.Runtime.Serialization;

namespace Core.Entity
{
    [Table("UserProfiles", Schema = "core")] 
    [DataContract]
    public class UserProfile : EntityBase<UserProfile>, IUserProfile
    {
        [DataMember]
        public int Id { get; set; }
        [ForeignKey("Id")]
        [DataMember]
        public virtual User User { get; set; }
        [Required]
        [MaxLength(20)]
        [DataMember]
        public string UserName { get; set; }
        [Required]
        [DataMember]
        public string Password { get; set; }

        
    }
}
