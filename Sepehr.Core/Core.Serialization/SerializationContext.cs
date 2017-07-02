using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization
{
    public class SerializationContext
    {
        public SerializationContext()
        {
            ReferenceIds = new Dictionary<Type, Dictionary<object, int>>();
        }
        public Dictionary<Type,Dictionary<object,int>> ReferenceIds { get; set; }
        public int CurrentReferenceId { get; set; }
    }
}
