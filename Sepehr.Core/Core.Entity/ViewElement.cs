using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entity;
using System.Runtime.Serialization;
using Core.Cmn;
using Core.Entity.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("ViewElements", Schema = "core")]
    [DataContract]
    public class ViewElement : EntityBase<ViewElement>, IViewElement
    {
    

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        [DataMember]
        public int Id { get; set; }

        [MaxLength(250)]
        [Required]
        [DataMember]
        public string UniqueName { get; set; }

        [MaxLength(100)]
        [Required]
        [DataMember]
        public string Title { get; set; }

        [Required]
        [DataMember(IsRequired = true)]      
        public ElementType ElementType { get; set; }

        [DataMember]
        public string XMLViewData { get; set; }
        [ForeignKey("ParentId")]
        [DataMember]
        public virtual ViewElement ParentViewElement { get; set; }
        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public virtual ICollection<ViewElement> ChildrenViewElement { get; set; }

        [DataMember]
        public virtual ICollection<ViewElementRole> ViewElementRoles { get; set; }

        [DataMember]
        public virtual ICollection<CompanyViewElement> CompanyViewElements { get; set; }
        [DataMember]
        public bool IsHidden { get; set; }
        [DataMember]
        public bool InVisible { get; set; }
        [DataMember]
        public int SortOrder { get; set; }

    }
}
