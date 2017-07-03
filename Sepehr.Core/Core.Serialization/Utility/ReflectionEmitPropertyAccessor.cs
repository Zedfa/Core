using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Core.Serialization
{
    public class ReflectionEmitPropertyAccessor
    {
        private readonly Type targetType;
        private Dictionary<Type, OpCode> typeOpCodes;

        public ReflectionEmitPropertyAccessor(Type targetType, ObjectMetaData objectMetaData)
        {
            this.targetType = targetType;
            ObjectMetaData = objectMetaData;
            Init();
        }

        public Func<SerializationInfo, StreamingContext, object> CreateInstanceForISerialzableObject
        {
            get;
            private set;
        }

        public Func<object> EmittedObjectInstanceCreator
        {
            get;
            private set;
        }

        public Func<object, object>[] EmittedPropertyGetters
        {
            get;
            private set;
        }

        public Action<object, object>[] EmittedPropertySetters
        {
            get;
            private set;
        }

        public Type TargetType
        {
            get { return targetType; }
        }

        private ObjectMetaData ObjectMetaData { get; set; }

        private Func<object> CreateInstanceMethodIL()
        {
            var createInstanceReturnType = typeof(object);
            var createInstanceDynamicMethod
              = new DynamicMethod("CreateInstance" + Guid.NewGuid(),
                                    createInstanceReturnType,
                                    Type.EmptyTypes, true);

            var createInstanceIL = createInstanceDynamicMethod.GetILGenerator();
            if (TargetType.IsValueType)
            {
                createInstanceIL.Emit(OpCodes.Ldloca_S, createInstanceIL.DeclareLocal(TargetType));
                createInstanceIL.Emit(OpCodes.Initobj, TargetType);
                createInstanceIL.Emit(OpCodes.Ldloc_0);
                createInstanceIL.Emit(OpCodes.Box, TargetType);
                createInstanceIL.Emit(OpCodes.Ret);
            }
            else
            {
                var targetCreateInstanceMethod = targetType.GetConstructor(Type.EmptyTypes);
                if (targetCreateInstanceMethod != null)
                {
                    createInstanceIL.Emit(OpCodes.Newobj, targetCreateInstanceMethod);
                    createInstanceIL.Emit(OpCodes.Ret);
                }
                else
                {
                    createInstanceIL.ThrowException(typeof(MissingMethodException));
                }
            }
            return (Func<object>)createInstanceDynamicMethod.CreateDelegate(typeof(Func<object>));
        }

        private void Init()
        {
            InitTypeOpCodes();
            EmittedObjectInstanceCreator = CreateInstanceMethodIL();
            CreateInstanceForISerialzableObject = InitCreateInstanceForISerialzableObject();
            EmittedPropertyGetters = new Func<object, object>[ObjectMetaData.WritablePropertyList.Count];
            EmittedPropertySetters = new Action<object, object>[ObjectMetaData.WritablePropertyList.Count];
            var i = 0;
            ObjectMetaData.WritablePropertyList.ForEach(prop =>
            {
                var getParamTypes = new[] { typeof(object) };
                var getReturnType = typeof(object);
                var getMethod = new DynamicMethod("Get" + Guid.NewGuid(),
                                        getReturnType,
                                        getParamTypes, true);
                var getIL = getMethod.GetILGenerator();
                var targetGetMethod = prop.GetGetMethod(true);
                if (targetGetMethod != null)
                {
                    getIL.Emit(OpCodes.Ldarg_0); //Load the first argument
                    if (TargetType.IsValueType)
                    {
                        getIL.Emit(OpCodes.Unbox, TargetType);
                        getIL.EmitCall(OpCodes.Call, targetGetMethod, null); //Get the property value
                    }
                    else
                    {
                        getIL.Emit(OpCodes.Castclass, targetType); //Cast to the source type
                        getIL.Emit(OpCodes.Callvirt, targetGetMethod); //Get the property value
                    }

                    if (targetGetMethod.ReturnType.IsValueType)
                    {
                        getIL.Emit(OpCodes.Box, targetGetMethod.ReturnType); //Box
                    }
                }
                else
                {
                    getIL.ThrowException(typeof(MissingMethodException));
                }

                getIL.Emit(OpCodes.Ret);

                EmittedPropertyGetters[i] = (Func<object, object>)getMethod.CreateDelegate(typeof(Func<object, object>));

                var setParamTypes = new[] { typeof(object), typeof(object) };
                const Type setReturnType = null;
                var setMethod = new DynamicMethod("Set" + Guid.NewGuid(),
                                        setReturnType,
                                        setParamTypes, true);
                var setIL = setMethod.GetILGenerator();
                var targetSetMethod = prop.SetMethod;
                if (targetSetMethod != null)
                {
                    Type paramType = targetSetMethod.GetParameters()[0].ParameterType;
                    setIL.DeclareLocal(paramType);
                    setIL.Emit(OpCodes.Ldarg_0); //Load the first argument //(target object)
                    if (TargetType.IsValueType)
                    {
                        setIL.Emit(OpCodes.Unbox, TargetType);
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Castclass, targetType); //Cast to the source type
                    }

                    setIL.Emit(OpCodes.Ldarg_1); //Load the second argument
                                                 //(value object)
                    if (paramType.IsValueType)
                    {
                        setIL.Emit(OpCodes.Unbox, paramType); //Unbox it
                        if (typeOpCodes.ContainsKey(paramType)) //and load
                        {
                            var load = typeOpCodes[paramType];
                            setIL.Emit(load);
                        }
                        else
                        {
                            setIL.Emit(OpCodes.Ldobj, paramType);
                        }
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Castclass, paramType); //Cast class
                    }

                    if (TargetType.IsValueType)
                    {
                        setIL.EmitCall(OpCodes.Call, targetSetMethod, null); //Set the property value
                    }
                    else
                    {
                        setIL.EmitCall(OpCodes.Callvirt, targetSetMethod, null); //Set the property value
                    }
                }
                else
                {
                    setIL.ThrowException(typeof(MissingMethodException));
                }
                setIL.Emit(OpCodes.Ret);
                EmittedPropertySetters[i] = (Action<object, object>)setMethod.CreateDelegate(typeof(Action<object, object>));
                i++;
            });
        }

        private Func<SerializationInfo, StreamingContext, object> InitCreateInstanceForISerialzableObject()
        {
            if (typeof(ISerializable).IsAssignableFrom(targetType))
            {
                var createInstanceReturnType = typeof(object);
                var createInstanceParamsTypes = new[] { typeof(SerializationInfo), typeof(StreamingContext) };
                var dm = new DynamicMethod("CreateInstanceForISerialzableObject" + Guid.NewGuid(), typeof(object), createInstanceParamsTypes, true);
                var createInstanceIL = dm.GetILGenerator();
                var targetCreateInstanceMethod = TargetType.GetConstructor(createInstanceParamsTypes);
                if (targetCreateInstanceMethod == null)
                {
                    targetCreateInstanceMethod = TargetType.GetConstructor(
    BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
    null,
    createInstanceParamsTypes,
    null
    );
                }

                if (targetCreateInstanceMethod == null)
                {
                    createInstanceIL.ThrowException(typeof(MissingMethodException));
                }

                createInstanceIL.Emit(OpCodes.Ldarg_0); //Load the first argument
                createInstanceIL.Emit(OpCodes.Ldarg_1);
                createInstanceIL.Emit(OpCodes.Newobj, targetCreateInstanceMethod);
                if (TargetType.IsValueType)
                    createInstanceIL.Emit(OpCodes.Box, TargetType);
                createInstanceIL.Emit(OpCodes.Ret);
                return (Func<SerializationInfo, StreamingContext, object>)dm.CreateDelegate(typeof(Func<SerializationInfo, StreamingContext, object>));
            }

            return null;
        }

        private void InitTypeOpCodes()
        {
            typeOpCodes = new Dictionary<Type, OpCode>
                            {
                                {typeof (sbyte), OpCodes.Ldind_I1},
                                {typeof (byte), OpCodes.Ldind_U1},
                                {typeof (char), OpCodes.Ldind_U2},
                                {typeof (short), OpCodes.Ldind_I2},
                                {typeof (ushort), OpCodes.Ldind_U2},
                                {typeof (int), OpCodes.Ldind_I4},
                                {typeof (uint), OpCodes.Ldind_U4},
                                {typeof (long), OpCodes.Ldind_I8},
                                {typeof (ulong), OpCodes.Ldind_I8},
                                {typeof (bool), OpCodes.Ldind_I1},
                                {typeof (double), OpCodes.Ldind_R8},
                                {typeof (float), OpCodes.Ldind_R4}
                            };
        }
    }
}