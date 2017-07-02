using Core.Cmn.EntityBase;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep.DTO.UserRoleDTO
{
    [DataContract]
    public class UserRoleDTO : DtoBase<UserRole>
    {
        [DataMember]
        public int UserId { get { return Model.UserId; } set { Model.UserId = value; } }
        [DataMember]
        public int RoleId { get { return Model.RoleID; } set { Model.RoleID = value; } }
        [DataMember]
        public int OldRoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
    }
}
