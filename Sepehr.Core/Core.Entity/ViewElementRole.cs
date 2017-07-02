using Core.Cmn;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Entity
{

    [Table("ViewElementRoles", Schema = "core")]
    [DataContract]
    public class ViewElementRole : EntityBase<ViewElementRole>
    {
        [ForeignKey("ViewElementId")]
        [DataMember]
        public virtual ViewElement ViewElement { get; set; }
        [Key, Column(Order = 1)]
        [DataMember]
        public int ViewElementId { get; set; }

        [ForeignKey("RoleId")]
        [DataMember]
        public virtual Role Role { get; set; }
        [Key, Column(Order = 2)]
        [DataMember]
        public int RoleId { get; set; }


    }
}
