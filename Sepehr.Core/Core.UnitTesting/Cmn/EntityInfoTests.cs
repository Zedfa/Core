using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.UnitTesting;
using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Core.UnitTesting.Cmn
{
    [TestClass()]
    public class EntityInfoTests
    {


        [DataContract]
        public class ObjectT_est_For_Out_of_Range_Exception : EntityBase<ObjectT_est_For_Out_of_Range_Exception>
        {
            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 0)]
            public int Id { get; set; }

            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 1)]
            public string FName { get; set; }

            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 3)]
            public string LName { get; set; }

        }
            [DataContract]
        public class User : CoreEntity<User>, IUser
        {
            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 0)]
            public int Id { get; set; }

            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 1)]
            public string FName { get; set; }

            [DataMember]
            [IndexableProperty(GroupName = "x", IndexOrder = 2)]
            public string LName { get; set; }

            [DataMember]
            public virtual ICollection<UserRole> UserRoles { get; set; }

            [DataMember]
            [IndexableNavigationProperty(NavigationPath = "UserProfile.Id")]
            public virtual UserProfile UserProfile { get; set; }

            [DataMember]
            public virtual CompanyChart CompanyChart { get; set; }

            [DataMember]
            public virtual ICollection<UserConfig> UserConfigs { get; set; }

            [DataMember]
            [IndexableNavigationProperty(NavigationPath = "CompanyChartId")]
            public int? CompanyChartId { get; set; }

            [DataMember]
            public bool Active { get; set; }

            [DataMember]
            public string Email { get; set; }

            [DataMember]
            public int Count { get; set; }
            private static UnitTestBase testBase = new UnitTestBase();
            public static TEntity BuildSampleEntity<TEntity>() where TEntity : EntityBase<TEntity>, new()
            {
                TEntity entity = new TEntity();
                PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetSetMethod() != null)
                    {
                        object value;

                        Type propertyType = property.PropertyType;

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            value = testBase.GetRandomValue(Nullable.GetUnderlyingType(propertyType));
                        }
                        else
                        {
                            value = testBase.GetRandomValue(propertyType);
                        }

                        property.SetValue(entity, value);
                    }
                }
                return entity;
            }
        }
    }
}