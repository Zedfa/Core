using Core.Cmn;
using Core.Cmn.EntityBase;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep.DTO
{
    [DataContract]
    public class RoleDTO : DtoBase<Role>
    {
        [DataMember]
        public int Id { get { return Model.ID; } set { Model.ID = value; } }
        [DataMember]
        public string Name { get { return Model.Name; } set { Model.Name = value; } }
    }
}
