using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{
    public class DeserializationContext
    {
        public DeserializationContext()
        {
            ReferenceObjs = new Dictionary<Type, Dictionary<int, object>>();
        }
        public Dictionary<Type, Dictionary<int, object>> ReferenceObjs { get; set; }
    }
}
