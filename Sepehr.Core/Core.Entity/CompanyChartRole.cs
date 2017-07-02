using Core.Cmn;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entity
{
    [Table("CompanyChartRoles", Schema = "core")] 

    public class CompanyChartRole : EntityBase<CompanyChartRole>
    {
        [Key, Column(Order = 1)]
        public int CompanyChartId { get; set; }
        public virtual CompanyChart CompanyChart { get; set; }
        [Key, Column(Order = 2)]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}
