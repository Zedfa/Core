using Core.Cmn;
using Core.Cmn.Attributes;
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
        [FillNavigationProperyByCache(
            CacheName = "ViewElementRepository.AllViewElementsCache",
            ThisEntityRefrencePropertyName = "ViewElementId",
            OtherEntityRefrencePropertyName = "Id"
            )]
        public virtual ViewElement ViewElement
        {
            get
            {
                return GetNavigationPropertyDataItemFromCache<ViewElement>();
            }
            set
            {
                SetNavigationPropertyDataList(value);
            }
        }
        [Key, Column(Order = 1)]
        [DataMember]
        public int ViewElementId { get; set; }

        [ForeignKey("RoleId")]
        [DataMember]
        [FillNavigationProperyByCache(
            CacheName = "RoleRepository.AllRolesCache",
            ThisEntityRefrencePropertyName = "RoleId",
            OtherEntityRefrencePropertyName = "ID"
            )]
        public virtual Role Role
        {
            get
            {
                return GetNavigationPropertyDataItemFromCache<Role>();
            }
            set
            {
                SetNavigationPropertyDataList(value);
            }
        }
        [Key, Column(Order = 2)]
        [DataMember]
        public int RoleId { get; set; }


    }
}
