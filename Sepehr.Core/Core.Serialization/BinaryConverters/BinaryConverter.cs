using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Serialization.BinaryConverters
{
    public abstract class BinaryConverter<T> : BinaryConverterBase
    {
        public override Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        public override object Deserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            object obj;
            if (IsNullableType)
            {
                var isNull = reader.ReadBoolean();
                if (isNull)
                {
                    obj = null;
                }
                else
                {
                    if (!IsSimpleOrStructureType)
                    {
                        var currentReferenceId = reader.ReadInt32();
                        Dictionary<int, object> referenceIds;
                        if (context.ReferenceObjs.TryGetValue(CurrentType, out referenceIds))
                        {
                            if (!referenceIds.TryGetValue(currentReferenceId, out obj))
                            {
                                if (IsInherritable)
                                {
                                    var isFullTypeNameWritten = reader.ReadBoolean();
                                    if (isFullTypeNameWritten)
                                    {
                                        var objTypeString = reader.ReadString();
                                        var currentBinaryConverter = GetBinaryConverter(objTypeString);
                                        var dynamicGivenType = currentBinaryConverter.CurrentType;
                                        referenceIds[currentReferenceId] = currentBinaryConverter.CreateInstance(reader, dynamicGivenType, context);
                                        obj = currentBinaryConverter.DeserializeBaseCaller(reader, dynamicGivenType, context);
                                    }
                                    else
                                    {
                                        referenceIds[currentReferenceId] = CreateInstance(reader, objectType, context);
                                        obj = DeserializeBase(reader, objectType, context);
                                    }
                                }
                                else
                                {
                                    referenceIds[currentReferenceId] = CreateInstance(reader, objectType, context);
                                    obj = DeserializeBase(reader, objectType, context);
                                }
                            }
                        }
                        else
                        {
                            context.ReferenceObjs[CurrentType] = referenceIds = new Dictionary<int, object>();
                            if (IsInherritable)
                            {
                                var isFullTypeNameWritten = reader.ReadBoolean();
                                if (isFullTypeNameWritten)
                                {
                                    var objTypeString = reader.ReadString();
                                    var currentBinaryConverter = GetBinaryConverter(objTypeString);
                                    var dynamicGivenType = currentBinaryConverter.CurrentType;
                                    referenceIds[currentReferenceId] = currentBinaryConverter.CreateInstance(reader, objectType, context);
                                    obj = currentBinaryConverter.DeserializeBaseCaller(reader, dynamicGivenType, context);
                                }
                                else
                                {
                                    referenceIds[currentReferenceId] = CreateInstance(reader, objectType, context);
                                    obj = DeserializeBase(reader, objectType, context);
                                }
                            }
                            else
                            {
                                referenceIds[currentReferenceId] = CreateInstance(reader, objectType, context);
                                obj = DeserializeBase(reader, objectType, context);
                            }
                        }
                    }
                    else
                    {
                        obj = DeserializeBase(reader, objectType, context);
                    }
                }
            }
            else
            {
                obj = DeserializeBase(reader, objectType, context);
            }

            return obj;
        }

        public override void Serialize(object obj, BinaryWriter writer, SerializationContext context)
        {
            var isFirstItem = context.IsFirstItem;
            context.IsFirstItem = false;
            var isNull = obj == null;
            if (isNull)
            {
                writer.Write(isNull);
            }
            else
            {
                var castedObject = (T)obj;
                if (IsNullableType)
                {
                    writer.Write(isNull);
                    if (!IsSimpleOrStructureType)
                    {
                        bool isObjReferenced = false;
                        Dictionary<object, int> referenceIds;
                        int currentReferenceId;

                        if (context.ReferenceIds.TryGetValue(CurrentType, out referenceIds))
                        {
                            int objReferenceId;
                            if (referenceIds.TryGetValue(obj, out objReferenceId))
                            {
                                isObjReferenced = true;
                                currentReferenceId = objReferenceId;
                            }
                            else
                            {
                                referenceIds[obj] = currentReferenceId = ++context.CurrentReferenceId;
                            }
                        }
                        else
                        {
                            context.ReferenceIds[CurrentType] = referenceIds = new Dictionary<object, int>();
                            referenceIds[obj] = currentReferenceId = ++context.CurrentReferenceId;
                        }

                        writer.Write(currentReferenceId);
                        if (!isObjReferenced)
                        {
                            if (IsInherritable)
                            {
                                Type objType = obj.GetType();
                                if (CurrentType != objType || isFirstItem)
                                {
                                    writer.Write(/*Is FullTypeName Written*/true);
                                    writer.Write(/*Is FullTypeName Written*/objType.FullName + "," + objType.Assembly.FullName);
                                    var currentBinaryConverter = GetBinaryConverter(objType);
                                    currentBinaryConverter.SerializeBaseCaller(castedObject, writer, context);
                                }
                                else
                                {
                                    writer.Write(/*Is FullTypeName Written*/false);
                                    SerializeBase(castedObject, writer, context);
                                }
                            }
                            else
                            {
                                SerializeBase(castedObject, writer, context);
                            }
                        }
                    }
                    else
                    {
                        SerializeBase(castedObject, writer, context);
                    }
                }
                else
                {
                    SerializeBase(castedObject, writer, context);
                }
            }
        }

        protected abstract T DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context);
        protected abstract void SerializeBase(T objectItem, BinaryWriter writer, SerializationContext context);

        public override object DeserializeBaseCaller(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return DeserializeBase(reader, objectType, context);
        }

        public override void SerializeBaseCaller(object objectItem, BinaryWriter writer, SerializationContext context)
        {
            SerializeBase((T)objectItem, writer, context);
        }
        protected virtual void UpdateDeserializeContext(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            //if (IsNullableType)
            //{
            //    context.IsCurrentValueNull = reader.ReadBoolean();
            //    if (context.IsCurrentValueNull)
            //        context.SerializationPlan = context.SerializationPlan.NextSerializationPlanIfValueNull;
            //}
            //else
            //{
            //    context.IsCurrentValueNull = false;
            //    context.SerializationPlan = context.SerializationPlan.NextSerializationPlan;
            //}
        }

        protected virtual void UpdateSerializeContext(object obj, BinaryWriter writer, SerializationContext context)
        {
            //if (IsNullableType && obj == null)
            //{
            //    context.IsCurrentValueNull = true;
            //    writer.Write(true);
            //    context.SerializationPlan = context.SerializationPlan.NextSerializationPlanIfValueNull;
            //}
            //else
            //{
            //    context.IsCurrentValueNull = false;
            //    context.SerializationPlan = context.SerializationPlan.NextSerializationPlan;
            //}
        }
        /// <summary>
        ///It must be implemented and called just for reference Types.
        /// </summary>
        public virtual T CreateInstanceBase(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            throw new NotImplementedException("It must be implemented and called just for reference Types.");
        }

        public override object CreateInstance(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            return CreateInstanceBase(reader, objectType, context);
        }


    }
}