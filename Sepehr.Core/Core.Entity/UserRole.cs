using Core.Cmn;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Entity
{
    [Table("UserRoles", Schema = "core")]
    [DataContract]
    public class UserRole : CoreEntity<UserRole>
    {
        [Key, Column(Order = 2)]
        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        [DataMember]
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        [DataMember]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }


    }
}
