using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Cmn;
namespace Core.Entity
{
    [Table("CompanyViewElements", Schema = "core")] 
    public class CompanyViewElement : EntityBase<CompanyViewElement>
    {
         [Key, Column(Order = 1)]
        public int ViewElementId { get; set; }
        public virtual ViewElement ViewElement { get; set; }
         [Key, Column(Order = 2)]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public OrganizationBase CurrentOrganization { get; set; }
        


    }
}
