using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Serialization.BinaryConverters
{
    public abstract class BinaryConverter<T> : BinaryConverterBase
    {
        private bool? _isNullableType;

        private bool? _isSimpleType;

        public Type CurrentType
        {
            get;
            protected set;
        }

        public bool IsNullableType
        {
            get
            {
                if (_isNullableType == null)
                {
                    _isNullableType = CurrentType.IsNullable();
                }

                return _isNullableType.Value;
            }
        }

        public bool IsSimpleOrStructureType
        {
            get
            {
                if (_isSimpleType == null)
                {
                    _isSimpleType = ItemType.IsSimple() || ItemType.IsValueType;
                }

                return _isSimpleType.Value;
            }
        }

        public override Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        public override BinaryConverterBase Copy()
        {
            return CopyBase();
        }

        public override object Deserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            BeforDeserialize(reader, objectType, context);
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
                                referenceIds[currentReferenceId] = obj = DeserializeBase(reader, objectType, context);
                            }
                        }
                        else
                        {
                            context.ReferenceObjs[CurrentType] = referenceIds = new Dictionary<int, object>();
                            referenceIds[currentReferenceId] = obj = DeserializeBase(reader, objectType, context);
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
            var isNull = obj == null;
            if (isNull)
            {
                writer.Write(isNull);
            }
            else
            {
                var castedObject = (T)obj;
                BeforSerialize(castedObject, writer, context);
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
                            SerializeBase(castedObject, writer, context);
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

        protected virtual void BeforDeserialize(BinaryReader reader, Type objectType, DeserializationContext context)
        {
            if (CurrentType == null)
                CurrentType = objectType;
        }

        protected virtual void BeforSerialize(T obj, BinaryWriter writer, SerializationContext context)
        {
            if (CurrentType == null)
                CurrentType = obj.GetType();
        }

        protected abstract BinaryConverter<T> CopyBase();
        protected abstract T DeserializeBase(BinaryReader reader, Type objectType, DeserializationContext context);

        protected abstract void SerializeBase(T objectItem, BinaryWriter writer, SerializationContext context);
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
    }
}