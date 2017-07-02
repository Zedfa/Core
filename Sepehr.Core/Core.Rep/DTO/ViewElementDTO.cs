using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep.DTO
{
    [DataContract]
    public class ViewElementDTO
    {
        [DataMember]
        public string UniqueName { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
