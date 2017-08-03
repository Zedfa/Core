using Core.Ef;
using Core.Entity;
using Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

using Core.UnitTesting.Entity;
using System.Globalization;
using Core.Cmn.Attributes;

using Core.Rep;
using Core.Cmn;
using Core.Cmn.DependencyInjection;

namespace Core.UnitTesting.Service
{
    [TestClass()]
    public class ConstantServiceUnitTest : ServiceUnitTestBase<IConstantService, IConstantRepository, Constant>
    {
        protected override IConstantService ConstructService()
        {
            IDbContextBase ctx = Mock.MockHelperBase.BuildMockContext();

            return new ConstantService(ctx);
        }

        private EntityUnitTestHelperBase<Constant> _entityUnitTestHelper;
        protected override EntityUnitTestHelperBase<Constant> EntityUnitTestHelper
        {
            get
            {
                return _entityUnitTestHelper ?? (_entityUnitTestHelper = new ConstantUnitTestHelper());
            }
        }


        public override void Initialize(TestContext testContext)
        {
            Core.Cmn.AppBase.StartApplication();         
        }

        [TestInitialize]
        public void TestInitialize()
        {
            EntityUnitTestHelper.SeedData();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            EntityUnitTestHelper.CleanData();
        }

        [ClassCleanup]
        public static void CleanUp()
        {

        }

        [UnitTest]
        [TestMethod()]
        public override void ConstructorTest()
        {
            base.ConstructorTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void GetContextTest()
        {
            base.GetContextTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void AllTest()
        {
            base.AllTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void CreateByEntityTest()
        {
            base.CreateByEntityTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void CreateByListTest()
        {
            base.CreateByListTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void DeleteByEntityTest()
        {
            base.DeleteByEntityTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void DeleteByEntityListTest()
        {
            base.DeleteByEntityListTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void UpdateByEntityTest()
        {
            base.DeleteByEntityListTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void GetCountTest()
        {
            base.GetCountTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByPredicateTest()
        {
            base.FilterByPredicateTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByExpressionTest()
        {
            base.FilterByExpressionTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FindByPredicateTest()
        {
            base.FindByPredicateTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void GetAppBaseTest()
        {
            base.GetAppBaseTest();
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantsTest()
        {
            // assemble
            IConstantService service = ConstructService();

            // act
            List<Constant> list = service.GetConstants(false);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, service.All(false).ToList().Count);
        }

        [UnitTest]
        [TestMethod()]
        public void GetDefaultCultureTest()
        {
            // assemble
            IConstantService service = ConstructService();

            // act
            string defaultCulture = service.GetDefaultCulture(false);

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(defaultCulture));

            Constant constant = service
                .All(false)
                .FirstOrDefault(c => c.Key == "DefaultLanguage" && c.ConstantCategory.Name == "GeneralConfig");

            if (constant == null)
            {
                Assert.AreEqual("fa-IR", defaultCulture);
            }
            else
            {
                if (constant.Value == "fa")
                {
                    Assert.AreEqual("fa-IR", defaultCulture);
                }
                else if (constant.Value == "ar")
                {
                    Assert.AreEqual("ar-SA", defaultCulture);
                }
                else if (constant.Value == "en")
                {
                    Assert.AreEqual("en-US", defaultCulture);
                }
                else
                {
                    Assert.AreEqual("fa-IR", defaultCulture);
                }
            }
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantByNameOfCategoryOnlyTest()
        {
            // assemble
            IConstantService service = ConstructService();

            List<Constant> all = service.All(false).ToList();

            foreach (Constant constant in all)
            {
                // act
                List<Constant> list1 = service.GetConstantByNameOfCategory(constant.ConstantCategory.Name, true, false);

                // assert
                List<Constant> list2 = service.GetConstantByNameOfCategoryAndCulture(constant.ConstantCategory.Name, service.CurrentCulture.Name, false);
                Assert.IsNotNull(list2);
                if (list2.Count == 0)
                {
                    list2 = service.GetConstantByNameOfCategoryAndCulture(constant.ConstantCategory.Name, service.GetDefaultCulture(false), false);
                    Assert.IsNotNull(list2);
                }

                CollectionAssert.AreEqual(list2, list1);
            }
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantByNameOfCategoryAndCultureTest()
        {
            // assemble
            IConstantService service = ConstructService();

            List<Constant> all = service.All(false).ToList();

            foreach (Constant constant in all)
            {

                // act
                List<Constant> list1 = service.GetConstantByNameOfCategoryAndCulture(constant.ConstantCategory.Name, constant.Culture, false);

                // assert
                Assert.IsNotNull(list1);
                foreach (Constant c in list1)
                {
                    Assert.AreEqual(constant.ConstantCategory.Name, c.ConstantCategory.Name);
                    Assert.AreEqual(constant.Culture, c.Culture);
                }
            }
        }

        [UnitTest]
        [TestMethod()]
        public void TryGetValueByKeyOneConstantTest()
        {
            // assemble
            IConstantService service = ConstructService();

            Constant entity = EntityUnitTestHelper.CreateSampleEntity();
            entity = service.Create(entity);

            string value;

            // act
            bool tryResult = service.TryGetValue(entity.Key, out value, false);

            // assert
            Assert.IsTrue(tryResult);
            Assert.AreEqual(entity.Value, value);

            // assemble
            service.Delete(entity);

            // act
            tryResult = service.TryGetValue(entity.Key, out value, false);

            // assert
            Assert.IsFalse(tryResult);
            Assert.IsNull(value);
        }

        [UnitTest]
        [TestMethod()]
        public void TryGetValueByKeyMoreConstantsTest()
        {
            // assemble
            IConstantService service = ConstructService();
            IList<Constant> list = EntityUnitTestHelper.CreateSampleEntityList(CultureCount, CultureCount);
            for (int i = 0; i < list.Count; i++)
            {
                Constant c = list[i];
                c.Key = list[0].Key;
                c.Culture = CULTURES[i];
            }

            list[0].ConstantCategory = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY;
            list[0].ConstantCategoryID = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY.ID;

            service.CurrentCulture = CultureInfo.CreateSpecificCulture(list[0].Culture);

            service.Create(list.ToList());
            string value;

            // act
            bool tryResult = service.TryGetValue(list[0].Key, out value, false);

            // assert
            Assert.IsTrue(tryResult);
            Assert.AreEqual(list[0].Value, value);

            for (int i = 1; i < list.Count; i++)
            {
                Assert.AreNotEqual(list[i].Value, value);
            }
        }

        [UnitTest]
        [TestMethod()]
        public void TryGetValueByKeyAndCategoryOneConstantTest()
        {
            // assemble
            IConstantService service = ConstructService();

            Constant entity = EntityUnitTestHelper.CreateSampleEntity();
            entity.ConstantCategory = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY;
            entity.ConstantCategoryID = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY.ID;
            entity = service.Create(entity);

            string value;

            // act
            bool tryResult = service.TryGetValue(entity.Key, ConstantCategoryUnitTestHelper.GENERAL_CONFIG_NAME, out value, false);

            // assert
            Assert.IsTrue(tryResult);
            Assert.AreEqual(entity.Value, value);

            // assemble
            service.Delete(entity);

            // act
            tryResult = service.TryGetValue(entity.Key, ConstantCategoryUnitTestHelper.GENERAL_CONFIG_NAME, out value, false);

            // assert
            Assert.IsFalse(tryResult);
            Assert.IsNull(value);
        }


        [UnitTest]
        [TestMethod()]
        public void TryGetValueByKeyAndCategoryMoreConstantsTest()
        {
            // assemble
            IConstantService service = ConstructService();

            IList<Constant> list = EntityUnitTestHelper.CreateSampleEntityList(CultureCount, CultureCount);
            for (int i = 0; i < list.Count; i++)
            {
                Constant c = list[i];
                c.ConstantCategoryID = 1;
                c.Key = list[0].Key;
                c.Culture = CULTURES[i];
            }

            service.CurrentCulture = CultureInfo.CreateSpecificCulture(list[0].Culture);

            list[0].ConstantCategory = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY;
            list[0].ConstantCategoryID = ConstantCategoryUnitTestHelper.GENERAL_CONFIG_CONSTANT_CATEGORY.ID;

            service.Create(list.ToList());

            // act
            string value;
            bool tryResult = service.TryGetValue(list[0].Key, ConstantCategoryUnitTestHelper.GENERAL_CONFIG_NAME, out value, false);

            // assert
            Assert.IsTrue(tryResult);
            Assert.AreEqual(list[0].Value, value);

            for (int i = 1; i < list.Count; i++)
            {
                Assert.AreNotEqual(list[i].Value, value);
            }
        }

        [UnitTest]
        [TestMethod()]
        public void TryGetValueByKeyAndCategoryAndCultureTest()
        {
            // assemble
            IConstantService service = ConstructService();

            Constant entity = EntityUnitTestHelper.CreateSampleEntity();

            entity = service.Create(entity);

            string value;

            // act
            bool tryResult = service.TryGetValue(entity.Key, entity.ConstantCategory.Name, entity.Culture, out value, false);

            // assert
            Assert.IsTrue(tryResult);
            Assert.AreEqual(entity.Value, value);

            // assemble
            service.Delete(entity);

            // act
            tryResult = service.TryGetValue(entity.Key, entity.ConstantCategory.Name, entity.Culture, out value, false);
            Assert.IsFalse(tryResult);
            Assert.IsNull(value);
        }
    }
}
