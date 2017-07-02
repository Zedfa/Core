﻿
using Core.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Serialization.Test
{
    [TestClass()]
    public class ObjectMetadataTests
    {
        public enum TestEnum : long
        {
            a, b, c, d, e, f, g
        }
        public enum TestEnum2 : uint
        {
            a, b, c, d, e, f, g
        }

        public class User<T> 
        {
            [DataMember]
            public int Id
            {
                get; set;
            }
            [DataMember]
            public string FName
            {
                get; set;
            }
            [DataMember]
            public string LName
            {
                get; set;
            }
            [DataMember]
            public virtual List<T> UserRoles
            {
                get; set;
            }
            [DataMember]
            public int? CompanyChartId
            {
                get; set;
            }
            [DataMember]
            public bool Active
            {
                get; set;
            }
            public string Email
            {
                get; set;
            }
            [DataMember]
            public int Count
            {
                get; set;
            }
            [DataMember]
            public UserDefined UserDefined
            {
                get; set;
            }

            [DataMember]
            public TestEnum TestEnum
            {
                get; set;
            }

            [DataMember]
            public TestEnum? TestEnumNullable
            {
                get; set;
            }
        }

        [DataContract]
        public class UserRole 
        {
            [DataMember]
            public int Id
            {
                get; set;
            }
            [DataMember]
            public int RoleId
            {
                get; set;
            }
            [DataMember]
            public int UserId
            {
                get; set;
            }
            [DataMember]
            public User<UserRole> User
            {
                get; set;
            }
            [DataMember]
            public Role Role
            {
                get; set;
            }

            [DataMember]
            public TestEnum2 TestEnum2
            {
                get; set;
            }

            [DataMember]
            public int[] SomeArray
            {
                get; set;
            }
        }

        [DataContract]
        public class Role 
        {
            [DataMember]
            public int Id
            {
                get;set;
            }
            [DataMember]
            public string Name
            {
                get; set;
            }
        }

        public class UserDefined
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public uint UId { get; set; }
            [DataMember]
            public ulong ULongId { get; set; }
            [DataMember]
            public long LongId { get; set; }
            [DataMember]
            public long? NullableLongId { get; set; }
            [DataMember]
            public Point Pint { get; set; }
            [DataMember]
            public Point? NullablePint { get; set; }
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public decimal Grade { get; set; }
            [DataMember]
            public float Mony { get; set; }
            [DataMember]
            public byte ClassCount { get; set; }
            [DataMember]
            public sbyte UClassCount { get; set; }
            [DataMember]
            public DateTime Date1 { get; set; }
            [DataMember]
            public DateTime? Date2 { get; set; }
        }

        [TestMethod]
        public void SerializablePropertiesGetterIsCorrectForUserEntity()
        {

            Assert.AreEqual(ObjectMetaData.GetEntityMetaData(typeof(ObjectMetadataTests.User<UserRole>)).WritablePropertyList.Count, 11);
            Assert.AreEqual(ObjectMetaData.GetEntityMetaData(typeof(ObjectMetadataTests.User<UserRole>)).WritablePropertyList.First().Name, "Active");
        }


    }
}