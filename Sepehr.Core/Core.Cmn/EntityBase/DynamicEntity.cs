
using System;
using System.Collections.Generic;
using System.Text;
using System.Dynamic;
using System.IO;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
//using Core.Entity;
namespace Tools.Cmn
{


    [Serializable]
    public abstract class DynamicEntity : Core.Cmn.EntityBase<DynamicEntity>, IDynamicMetaObjectProvider
    {
        #region Dynamic

        #region IDynamicMetaObjectProvider Members
        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(
          System.Linq.Expressions.Expression parameter)
        {
            return new DynamicDictionaryMetaObject(parameter, this);
        }
        #endregion

        private class DynamicDictionaryMetaObject : DynamicMetaObject
        {
            internal DynamicDictionaryMetaObject(
                System.Linq.Expressions.Expression parameter,
                DynamicEntity value)
                : base(parameter, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindSetMember(SetMemberBinder binder,
                DynamicMetaObject value)
            {
                // Method to call in the containing class:
                string methodName = "SetDictionaryEntry";

                // setup the binding restrictions.
                BindingRestrictions restrictions =
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType);

                // setup the parameters:
                Expression[] args = new Expression[2];
                // First parameter is the name of the property to Set
                args[0] = Expression.Constant(binder.Name);
                // Second parameter is the value
                args[1] = Expression.Convert(value.Expression, typeof(object));

                // Setup the 'this' reference
                Expression self = Expression.Convert(Expression, LimitType);

                // Setup the method call expression
                Expression methodCall = Expression.Call(self,
                        typeof(DynamicEntity).GetMethod(methodName),
                        args);

                // Create a meta object to invoke Set later:
                DynamicMetaObject setDictionaryEntry = new DynamicMetaObject(
                    methodCall,
                    restrictions);
                // return that dynamic object
                return setDictionaryEntry;
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                // Method call in the containing class:
                string methodName = "GetDictionaryEntry";

                // One parameter
                Expression[] parameters = new Expression[]
            {
                Expression.Constant(binder.Name)
            };

                DynamicMetaObject getDictionaryEntry = new DynamicMetaObject(
                    Expression.Call(
                        Expression.Convert(Expression, LimitType),
                        typeof(DynamicEntity).GetMethod(methodName),
                        parameters),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType));
                return getDictionaryEntry;
            }

            public override DynamicMetaObject BindInvokeMember(
                InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                StringBuilder paramInfo = new StringBuilder();
                paramInfo.AppendFormat("Calling {0}(", binder.Name);
                foreach (var item in args)
                    paramInfo.AppendFormat("{0}, ", item.Value);
                paramInfo.Append(")");

                Expression[] parameters = new Expression[]
            {
                Expression.Constant(paramInfo.ToString())
            };
                DynamicMetaObject methodInfo = new DynamicMetaObject(
                    Expression.Call(
                    Expression.Convert(Expression, LimitType),
                    typeof(DynamicEntity).GetMethod("WriteMethodInfo"),
                    parameters),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType));
                return methodInfo;
            }
        }

        private static object _ignoreValueObject = new object();
        public override void OnColumnChanging(string columnName, ref object value)
        {
            SetDictionaryEntry(columnName, _ignoreValueObject);
            base.OnColumnChanging(columnName, ref value);
        }



        public object SetDictionaryEntry(string key, object value)
        {
            if (value != _ignoreValueObject)
                this[key] = value;
            return value;
        }

        public object GetDictionaryEntry(string key)
        {
            return this[key];
        }

        //public object WriteMethodInfo(string methodInfo)
        //{
        //    Console.WriteLine(methodInfo);
        //    return 42; // because it is the answer to everything
        //}

        #endregion


        public DynamicEntity()
        {

        }
    }
}
