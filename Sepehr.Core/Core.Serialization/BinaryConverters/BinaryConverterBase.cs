using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace Core.Serialization.BinaryConverters
{
    public abstract class BinaryConverterBase
    {
        private static bool? _fullyTrusted;

        private static Dictionary<Type, BinaryConverterBase> _serializeItemsCachedByEveryType;

        private static List<BinaryConverterBase> _serializeItemsForAllTypes;
        private static Dictionary<string, BinaryConverterBase> _serializeItemsCachedByEveryTypeName;

        static BinaryConverterBase()
        {
            GenericTypeDefinitionForConverters = new ConcurrentDictionary<Type, Type>();
            BinaryConverterBase.GenericTypeDefinitionForConverters.TryAdd(typeof(HashSet<>), typeof(HashSetBinaryConverter<>));
            BinaryConvertersForAllTypes = new List<BinaryConverterBase>();
            BinaryConvertersCachedByEveryType = new Dictionary<Type, BinaryConverterBase>();
            BinaryConvertersCachedByEveryType_Copy = new Dictionary<Type, BinaryConverterBase>();
            BinaryConvertersCachedByEveryTypeName = new Dictionary<string, BinaryConverterBase>();
            BinaryConvertersCachedByEveryTypeName_Copy = new Dictionary<string, BinaryConverterBase>();
            AllBinaryConverterTypes = typeof(BinaryConverterBase).Assembly.GetTypes().Where(type =>
            (typeof(BinaryConverterBase)).IsAssignableFrom(type) &&
            type != typeof(BinaryConverter<>) && type != typeof(NullBinaryConverter) && type != typeof(BinaryConverterBase)).ToList();
            AllGenericBinaryConverterTypes = AllBinaryConverterTypes.Where(converterType => converterType.IsGenericType).ToList();
            AllBinaryConverterTypes.Where(converterType => !converterType.IsGenericType).ToList().ForEach(type =>
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

        public static Dictionary<Type, BinaryConverterBase> BinaryConvertersCachedByEveryType
        {
            get { return _serializeItemsCachedByEveryType; }
            private set { _serializeItemsCachedByEveryType = value; }
        }
        public static Dictionary<string, BinaryConverterBase> BinaryConvertersCachedByEveryTypeName
        {
            get { return _serializeItemsCachedByEveryTypeName; }
            private set { _serializeItemsCachedByEveryTypeName = value; }
        }
        public static Dictionary<Type, BinaryConverterBase> BinaryConvertersCachedByEveryType_Copy { get; private set; }
        public static Dictionary<string, BinaryConverterBase> BinaryConvertersCachedByEveryTypeName_Copy { get; private set; }
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
        public Type CurrentType
        {
            get;
            protected set;
        }
        public bool IsInherritable { get; private set; }
        public abstract Type ItemType { get; }
        public bool IsNullableType { get; private set; }
        public bool IsSimpleOrStructureType { get; private set; }
        public int SortOrder { get; set; }
        public static List<Type> AllBinaryConverterTypes { get; private set; }
        public static List<Type> AllGenericBinaryConverterTypes { get; private set; }
        public static ConcurrentDictionary<Type, Type> GenericTypeDefinitionForConverters { get; private set; }
        public static BinaryConverterBase GetBinaryConverter(Type typeToSerialize)
        {
            BinaryConverterBase result;
            if (!BinaryConvertersCachedByEveryType.TryGetValue(typeToSerialize, out result))
            {
                Type typeToFineConverter = typeToSerialize;
                var underlyingType = Nullable.GetUnderlyingType(typeToSerialize);
                if (underlyingType != null)
                    typeToFineConverter = underlyingType;
                result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType == typeToFineConverter);
                if (result == null)
                {
                    if (typeToSerialize.IsGenericType)
                    {
                        Type definitionGenericType;
                        GenericTypeDefinitionForConverters.TryGetValue(typeToSerialize.GetGenericTypeDefinition(), out definitionGenericType);
                        if (definitionGenericType != null)
                        {
                            result = (BinaryConverterBase)Activator.CreateInstance(definitionGenericType.MakeGenericType(typeToSerialize.GetGenericArguments()));
                        }
                    }

                    if (result == null)
                        result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType.IsAssignableFrom(typeToFineConverter));
                }
                if (result == null)
                    throw new ArgumentException($"The type '{typeToSerialize}' not found in implemented BinaryConverters for serializing! you should impelement a custom BinaryConverter for this Type in Core.Serialization.");

                lock (BinaryConvertersCachedByEveryType_Copy)
                {
                    BinaryConverterBase foundResult;
                    if (!BinaryConvertersCachedByEveryType_Copy.TryGetValue(typeToSerialize, out foundResult))
                    {
                        result = result.Copy(typeToSerialize);
                        BinaryConvertersCachedByEveryType_Copy[typeToSerialize] = result;
                        BinaryConvertersCachedByEveryType = BinaryConvertersCachedByEveryType_Copy.ToDictionary(item => item.Key, item => item.Value);
                    }
                    else
                    {
                        result = foundResult;
                    }
                }
            }

            return result;
        }
        public static BinaryConverterBase GetBinaryConverter(string fullTypeNameToSerialize)
        {
            BinaryConverterBase result;
            if (!BinaryConvertersCachedByEveryTypeName.TryGetValue(fullTypeNameToSerialize, out result))
            {
                Type typeToFineConverter = Type.GetType(fullTypeNameToSerialize);
                var typeToSerialize = typeToFineConverter;
                var underlyingType = Nullable.GetUnderlyingType(typeToSerialize);
                if (underlyingType != null)
                    typeToFineConverter = underlyingType;
                result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType == typeToFineConverter);
                if (result == null)
                {
                    result = BinaryConvertersForAllTypes.FirstOrDefault(item => item.ItemType.IsAssignableFrom(typeToFineConverter));
                }
                if (result == null)
                    throw new ArgumentException($"The type '{typeToSerialize}' not found in implemented BinaryConverters for serializing! you should impelement a custom BinaryConverter for this Type in Core.Serialization.");

                lock (BinaryConvertersCachedByEveryTypeName_Copy)
                {
                    BinaryConverterBase foundResult;
                    if (!BinaryConvertersCachedByEveryTypeName_Copy.TryGetValue(fullTypeNameToSerialize, out foundResult))
                    {
                        result = result.Copy(typeToSerialize);
                        BinaryConvertersCachedByEveryTypeName_Copy[fullTypeNameToSerialize] = result;
                        BinaryConvertersCachedByEveryTypeName = BinaryConvertersCachedByEveryTypeName_Copy.ToDictionary(item => item.Key, item => item.Value);
                    }
                    else
                    {
                        result = foundResult;
                    }
                }
            }

            return result;
        }
        protected BinaryConverterBase Init(Type type)
        {
            CurrentType = type;
            IsInherritable = type.IsInheritable();
            IsNullableType = type.IsNullable();
            IsSimpleOrStructureType = ItemType.IsSimple() || ItemType.IsValueType;
            return this;
        }
        public abstract BinaryConverterBase Copy(Type objectType);

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