using Core.Serialization.BinaryConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.SerializationPlan
{
    public static class SerializationPlan
    {

        static SerializationPlan()
        {

            SerializationPlansCachedByEveryTypeHachcode = new Dictionary<int, BinaryConverterBase>();
            SerializationPlansCachedByEveryTypeHachcode_Copy = new Dictionary<int, BinaryConverterBase>();
        }

        private static Dictionary<int, BinaryConverterBase> SerializationPlansCachedByEveryTypeHachcode { get; set; }
        private static Dictionary<int, BinaryConverterBase> SerializationPlansCachedByEveryTypeHachcode_Copy { get; set; }
        public static void SetSerializePlan(Type typeToSerialize, BinaryConverterBase value)
        {
            BinaryConverterBase result;
            if (!SerializationPlansCachedByEveryTypeHachcode.TryGetValue(typeToSerialize.GetHashCode(), out result))
            {
                if (typeToSerialize == null)
                    throw new NullReferenceException($"The parameter '{nameof(typeToSerialize)}' can't be null.");

                lock (SerializationPlansCachedByEveryTypeHachcode_Copy)
                {
                    if (!SerializationPlansCachedByEveryTypeHachcode_Copy.ContainsKey(typeToSerialize.GetHashCode()))
                    {
                        SerializationPlansCachedByEveryTypeHachcode_Copy[typeToSerialize.GetHashCode()] = value;
                        SerializationPlansCachedByEveryTypeHachcode = SerializationPlansCachedByEveryTypeHachcode_Copy.ToDictionary(item => item.Key, item => item.Value);
                    }
                }
            }
        }


        public static bool TryGetBinaryConverter(Type typeToSerialize, out BinaryConverterBase result)
        {
            if (typeToSerialize == null)
                throw new NullReferenceException($"The parameter '{nameof(typeToSerialize)}' can't be null.");
            return SerializationPlansCachedByEveryTypeHachcode.TryGetValue(typeToSerialize.GetHashCode(), out result);
        }
    }
}
