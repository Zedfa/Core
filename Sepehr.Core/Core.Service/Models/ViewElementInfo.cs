using Core.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Core.Service.Models
{
    public class ViewElementInfo
    {
        public string ConceptualName { get; set; }
        public string Url { get; set; }
        public ElementType ElementType { get; set; }

    }
}
