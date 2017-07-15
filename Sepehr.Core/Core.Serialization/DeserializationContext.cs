using System;
using System.Collections.Generic;

namespace Core.Serialization
{
    public class DeserializationContext
    {
        public DeserializationContext()
        {
            ReferenceObjs = new Dictionary<Type, Dictionary<int, object>>();
        }

        public Dictionary<Type, Dictionary<int, object>> ReferenceObjs { get; set; }
        public object CurrentReferenceTypeObject { get; set; }
    }
}