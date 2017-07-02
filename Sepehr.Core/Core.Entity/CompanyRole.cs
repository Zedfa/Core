using Core.Cmn;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("CompanyRoles", Schema = "core")] 

    public class CompanyRole : CoreEntity<CompanyRole>
    {
        [Key, Column(Order = 1)]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Key, Column(Order = 2)]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        [NotMapped]
        public Company CurrentCompany { get; set; }
       

    }



}
