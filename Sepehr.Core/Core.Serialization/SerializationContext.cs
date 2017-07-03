using System;
using System.Collections.Generic;

namespace Core.Serialization
{
    public class SerializationContext
    {
        public SerializationContext()
        {
            ReferenceIds = new Dictionary<Type, Dictionary<object, int>>();
        }

        public int CurrentReferenceId { get; set; }
        public Dictionary<Type, Dictionary<object, int>> ReferenceIds { get; set; }
    }
}