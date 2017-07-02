using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Core.Entity;
using Core.Cmn;

namespace Core.Entity
{
    [Table("CompanyCharts", Schema = "core")] 

     [DataContract(Name = "CompanyChart")]
    public class CompanyChart : EntityBase<CompanyChart>
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        
        public int? ParentId { get; set; }
       
        public Int16? Level { get; set; }
        public virtual ICollection<CompanyChart> ChildCompanyChart { get; set; }
        public virtual ICollection<CompanyChartRole> CompanyChartRoles { get; set; }
        public string Lineage { get; set; }
        public int Depth { get; set; }
       
        
    }
}