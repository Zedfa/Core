using Core.Cmn;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    //[Table("OrganizationBase", Schema = "core")] 
    public class OrganizationBase : EntityBase<OrganizationBase>
    {
        public int ID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? CurrentOrganizationId { get; set; }

        public bool Active { get; set; }
       
    }

  




}
