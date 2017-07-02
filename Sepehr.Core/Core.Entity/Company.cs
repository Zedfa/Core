using Core.Cmn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Companies", Schema = "core")] 
    public  class Company : EntityBase<Company>
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(12)]
        [Required]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

       

        [MaxLength(50)]
        public string Family { get; set; }

        [MaxLength(50)]
        public string FatherName { get; set; }

        
       
        [MaxLength(10)]
        public string NationalId { get; set; }

       
        public virtual ICollection<CompanyRole> CompanyRoles { get; set; }

        public virtual ICollection<CompanyViewElement> CompanyViewElements { get; set; }

       
        //[NotMapped]
        //public Company CurrentCompany { get; set; }
      
        public bool Active { get; set; }



    }
}
