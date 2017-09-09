using Core.Cmn;
using Core.Cmn.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Entity
{
   
    [Table("Requests", Schema = "core")]
    public class Request : EntityBase<Request> 
    {
       
        [Key]
        [ForeignKey("Log")]
        public int LogId { get; set; }
       
        public string Url { get; set; }
       
        public string Data { get; set; }
       
        public string Method { get; set; }

        [DataMember]
        public string IP { get; set; }

        public virtual Log Log { get; set; }

    }
}
