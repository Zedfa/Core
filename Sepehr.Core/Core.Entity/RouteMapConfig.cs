using Core.Cmn;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Core.Entity
{
    [Table("RouteMapConfig", Schema = "core")]
    [DataContract]
    public class RouteMapConfig : CoreEntity<RouteMapConfig>
    {
        [DataMember]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(200)]
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Pattern { get; set; }
        [DataMember]
        public string defaults { get; set; }

        [DataMember]
        public string Namespace { get; set; }


    }

}
