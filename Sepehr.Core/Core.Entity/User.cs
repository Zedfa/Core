using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Core.Entity;
using System;
using Core.Cmn;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Users", Schema = "core")]
    [DataContract]
    public class User : CoreEntity<User>, IUser
    {
        [DataMember]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DataMember]
        public string FName { get; set; }

        [MaxLength(50)]
        [DataMember]
        public string LName { get; set; }
        [DataMember]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        [DataMember]
        public virtual UserProfile UserProfile { get; set; }
        [DataMember]
        public virtual CompanyChart CompanyChart { get; set; }
        [DataMember]
        public virtual ICollection<UserConfig> UserConfigs { get; set; }
        [DataMember]
        public int? CompanyChartId { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [MaxLength(40)]
        [DataMember]
        public string Email { get; set; }
        //...Add For Publish .....
        [DataMember]
        public int Count { get; set; }




    }
}
