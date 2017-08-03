using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Core.Cmn.EntityBase;
using Core.Cmn;

namespace Core.Rep.DTO
{
    [DataContract(Name = "ExceptionLogDTO")]
    public class ExceptionLogDTO : EntityBase<ExceptionLogDTO> , IDto
    {
        //[DataMember]
        public int Id { get; set; }
        
        //[DataMember]
        public string ExceptionType { get; set; }
        
        //[DataMember]
        public string Message { get; set; }
       
        //[DataMember]
        public String StackTrace { get; set; }
       
        //[DataMember]
        public string Source { get; set; }

        //[DataMember(Name = "hasChildren")]
        public bool   HasChildren { get; set; }

        //[DataMember]
        public int?  ParentId { get; set; }
        //public ExceptionLogDTO ExceptionLog { get; set; }
    }
}