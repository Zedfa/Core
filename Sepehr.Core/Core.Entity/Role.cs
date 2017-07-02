using Core.Cmn;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.Entity
{
    [Table("Roles", Schema = "core")]
    [DataContract]
    public class Role : CoreEntity<Role>
    {
        //public Role()
        //{
        //    this.UserRoles = new List<UserRole>();
        //}
        [DataMember]
        public int ID { get; set; }
        [DataMember]

        public string Name { get; set; }

        // public string Title { get; set; }
        [DataMember]

        public bool IsCompanyRole { get; set; }
        [DataMember]

        public virtual ICollection<UserRole> UserRoles { get; set; }
        [DataMember]

        public virtual ICollection<ViewElementRole> ViewElementRoles { get; set; }
        [DataMember]

        public virtual ICollection<CompanyChartRole> CompanyChartRoles { get; set; }
        [DataMember]

        public virtual ICollection<CompanyRole> CompanyRoles { get; set; }


    }
}
