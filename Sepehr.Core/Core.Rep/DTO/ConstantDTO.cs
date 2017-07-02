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
    public class ConstantDTO : DtoBase<Constant>
    {
        public ConstantDTO()
        {

        }
        public ConstantDTO(Constant model) : base(model)
        {

        }
        public int ID
        {
            get { return Model.ID; }
            set { Model.ID = value; }
        }
        [DataMember]
        public string Key
        {
            get { return Model.Key; }
            set { Model.Key = value; }
        }
        [DataMember]
        public string Value
        {
            get { return Model.Value; }
            set { Model.Value = value; }
        }
    }
}
