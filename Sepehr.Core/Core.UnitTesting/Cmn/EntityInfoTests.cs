using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitTesting.Cmn
{
    [TestClass()]
    public class EntityInfoTests
    {
        [DataContract]
        public class User : CoreEntity<User>, IUser
        {
            [DataMember]
            public int Id { get; set; }
            [DataMember]
            public string FName { get; set; }
            [DataMember]
            public string LName { get; set; }
            [DataMember]
            public virtual ICollection<UserRole> UserRoles { get; set; }
            [DataMember]
            public virtual UserProfile UserProfile { get; set; }
            [DataMember]
            public virtual CompanyChart CompanyChart { get; set; }
            [DataMember]
            public virtual ICollection<UserConfig> UserConfigs { get; set; }
            [DataMember]
            public int? CompanyChartId { get; set; }
            [DataMember]
            public bool Active { get; set; }
            [DataMember]
            public string Email { get; set; }
            [DataMember]
            public int Count { get; set; }

        }

        [TestMethod]
        public void SerializableComplexPropertiesGetterIsCorrectForUserEntity()
        {

            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableComplexProperties.Count, 5);
            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableComplexProperties.First().Name, "CompanyChart");
            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableComplexProperties.Last().Name, "UserRoles");
        }


        [TestMethod]
        public void SerializableSimplePropertiesGetterIsCorrectForUserEntity()
        {
            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableSimpleProperties.Count, 7);
            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableSimpleProperties.First().Name, "Active");
            Assert.AreEqual(new EntityInfoTests.User().EntityInfo().SerializableSimpleProperties.Last().Name, "LName");
        }
    }
}
