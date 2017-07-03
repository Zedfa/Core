using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace Core.Serialization.BinaryConverters
{
    public abstract class BinaryConverterBase
    {
        private static bool? _fullyTrusted;

        private static Dictionary<int, BinaryConverterBase> _serializeItemsCachedByEveryTypeHachcode;

        private static List<BinaryConverterBase> _serializeItemsForAllTypes;

        static BinaryConverterBase()
        {
            BinaryConvertersForAllTypes = new List<BinaryConverterBase>();
            BinaryConvertersCachedByEveryTypeHachcode = new Dictionary<int, BinaryConverterBase>();
            BinaryConvertersCachedByEveryTypeHachcode_Copy = new Dictionary<int, BinaryConverterBase>();
            var allBinaryConverterBaseInCoreCmn = typeof(BinaryConverterBase).Assembly.GetTypes().Where(type =>
            (typeof(BinaryConverterBase)).IsAssignableFrom(type) &&
            type != typeof(BinaryConverter<>) && type != typeof(BinaryConverterBase)).ToList();
            allBinaryConverterBaseInCoreCmn.ForEach(type =>
            {
                var serializeItem = (Activator.CreateInstance(type) as BinaryConverterBase);
                BinaryConvertersForAllTypes.Add(serializeItem);
            });
            BinaryConvertersForAllTypes.ToList().ForEach(item =>
            {
                BinaryConvertersForAllTypes.ToList().ForEach(item1 =>
                {
                    if (item.ItemType.IsAssignableFrom(item1.ItemType))
                        item.SortOrder++;
                });
            });
            BinaryConvertersForAllTypes = BinaryConvertersForAllTypes.OrderBy(item => item.SortOrder).ToList();
        }

        public static Dictionary<int, BinaryConverterBase> BinaryConvertersCachedByEveryTypeHachcode
        {
            get { return _serializeItemsCachedByEveryTypeHachcode; }
            private set { _serializeItemsCachedByEveryTypeHachcode = value; }
        }

        public static Dictionary<int, BinaryConverterBase> BinaryConvertersCachedByEveryTypeHachcode_Copy { get; private set; }

        public static List<BinaryConverterBase> BinaryConvertersForAllTypes
        {
            get { return _serializeItemsForAllTypes; }
            private set { _serializeItemsForAllTypes = value; }
        }

        public static bool FullyTrusted
        {
            get
            {
                if (_fullyTrusted == null)
                {
                    //AppDomain appDomain = AppDomain.CurrentDomain;
                    //_fullyTrusted = appDomain.IsHomogenous && appDomain.IsFullyTrusted;
                    try
                    {
                        new SecurityPermission(PermissionState.Unrestricted).Demand();
                        _fullyTrusted = true;
                    }
                    catch (Exception)
                    {
                        _fullyTrusted = false;
                    }
                }

                return _fullyTrusted.GetValueOrDefault();
            }
        }

        public abstract Type ItemType { get; }

        public int SortOrder { get; set; }

        public static BinaryConverterBase GetBinaryConverter(Type typeToSerialize)
        {
            BinaryConverterBase result;
            if (!BinaryConvertersCachedByEveryTypeHachcode.TryGetValue(typeToSerialize.GetHashCode(), out result))
            {
                var underlyingType = Nullable.GetUnderlyingType(typeToSerialize);
                if (underlyingType != null)
                    typeToSerialize = underlyingType;
                result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType == typeToSerialize);
                if (result == null)
                {
                    result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType.IsAssignableFrom(typeToSerialize));
                }
                if (result == null)
                    throw new ArgumentException($"The type '{typeToSerialize}' not found in implemented BinaryConverters for serializing! you should impelement a custom BinaryConverter for this Type in Core.Serialization.");

                lock (BinaryConvertersCachedByEveryTypeHachcode_Copy)
                {
                    if (!BinaryConvertersCachedByEveryTypeHachcode_Copy.ContainsKey(typeToSerialize.GetHashCode()))
                    {
                        BinaryConvertersCachedByEveryTypeHachcode_Copy[result.GetHashCode()] = result;
                        BinaryConvertersCachedByEveryTypeHachcode = BinaryConvertersCachedByEveryTypeHachcode_Copy.ToDictionary(item => item.Key, item => item.Value);
                    }
                }
            }

            return result;
        }

        public abstract BinaryConverterBase Copy();

        public abstract object Deserialize(BinaryReader reader, Type objectType, DeserializationContext context);

        public abstract void Serialize(object obj, BinaryWriter writer, SerializationContext context);

        public void SerializeChildItem(BinaryConverterBase serializeItem, object obj, BinaryWriter writer, SerializationContext context)
        {
            serializeItem.Serialize(obj, writer, context);
        }

        protected static BinaryConverterBase GetBinaryConverter(Type objectType, int i)
        {
            return ObjectMetaData.GetEntityMetaData(objectType).BinaryConverterList[i];
        }
    }
}