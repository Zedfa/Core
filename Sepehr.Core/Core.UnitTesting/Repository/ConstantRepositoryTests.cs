using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.DependencyInjection;
using Core.Entity;
using Core.Mvc.Helpers;
using Core.Rep;
using Core.UnitTesting.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.UnitTesting.Repository
{
    [TestClass()]
    public class ConstantRepositoryTests : RepositoryUnitTestBase<IConstantRepository, Constant>
    {
        private static IDbContextBase BuildNewContext()
        {
            return Mock.MockHelperBase.BuildMockContext();
        }

        protected override IConstantRepository BuildRepository()
        {
            return new ConstantRepository(CreateNewContext());
        }

        protected override IDbContextBase CreateNewContext()
        {
            return BuildNewContext();
        }

        protected override string GetTableName()
        {
            return "Constants";
        }

        protected override string GetSchemaName()
        {
            return "core";
        }

        private EntityUnitTestHelperBase<Constant> _entityUnitTestHelper;

        protected override EntityUnitTestHelperBase<Constant> EntityUnitTestHelper
        {
            get
            {
                return _entityUnitTestHelper ?? (_entityUnitTestHelper = new ConstantUnitTestHelper());
            }
        }

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            List<Type> allTypes = AppBase.GetAlltypes().ToList();
            AppBase.DependencyInjectionManager = new UnityDependencyInjectionManager(allTypes);

            Core.Cmn.AppBase.LogService = new Core.Service.LogService(BuildNewContext());
            //Core.Cmn.AppBase.BuildEntityInfoDic(Core.Cmn.AppBase.GetAlltypes());
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
        public override void AllTest()
        {
            base.AllTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByExpressionTest()
        {
            base.FilterByExpressionTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByStringTest()
        {
            base.FilterByStringTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByExpressionIndexSizeTest()
        {
            base.FilterByExpressionIndexSizeTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void FilterByExpressionInfoTest()
        {
            base.FilterByExpressionInfoTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void ContainsCreatedEntityTest()
        {
            base.ContainsCreatedEntityTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void ContainsDeletedEntityTest()
        {
            base.ContainsDeletedEntityTest();
        }

        [UnitTest]
        [TestMethod()]
        public void FindByKeyTest()
        {
            // assemble
            using (IConstantRepository repository = BuildRepository())
            {
                // assemble
                Constant constant = EntityUnitTestHelper.CreateSampleEntity();
                constant = repository.Create(constant);

                // act
                Constant found = repository.Find(constant.ID);

                // assert
                Assert.IsNotNull(found);
                EntityUnitTestHelper.AssertEntitiesAreEqual(constant, found);
            }
        }

        [UnitTest]
        [TestMethod()]
        public override void FindByPredicateTest()
        {
            base.FindByPredicateTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void CreateTest()
        {
            base.CreateTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void CreateListTest()
        {
            base.CreateListTest();
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
        public override void GetCountTest()
        {
            base.GetCountTest();
        }

        [UnitTest]
        [TestMethod()]
        public override void UpdateByEntityTest()
        {
            base.UpdateByEntityTest();
        }

        [UnitTest]
        [TestMethod()]
        public void AllConstantCacheTest()
        {
            // TODO:
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantTest()
        {
            // TODO:
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantsOfCategoryTest()
        {
            // assemble
            using (IConstantRepository repository = BuildRepository())
            {
                List<Constant> all = repository.All(false).ToList();
                foreach (Constant constant in all)
                {
                    // assemble
                    int count = all.Where(c => c.ConstantCategoryID == constant.ConstantCategoryID).Count();

                    // act
                    List<Constant> categoryConstants = repository.GetConstantsOfCategory(constant.ConstantCategory.Name, false);

                    // assert
                    Assert.AreEqual(count, categoryConstants.Count);
                }
            }
        }

        [UnitTest]
        [TestMethod()]
        public void GetConstantsOfCategoryAndCultureTest()
        {
            // assemble
            using (IConstantRepository repository = BuildRepository())
            {
                List<Constant> all = repository.All(false).ToList();
                foreach (Constant constant in all)
                {
                    // assemble
                    int count = all.Where(c => c.ConstantCategoryID == constant.ConstantCategoryID && c.Culture == constant.Culture).Count();

                    // act
                    List<Constant> categoryConstants = repository.GetConstantsByCategoryAndCulture(constant.ConstantCategory.Name, constant.Culture, false);

                    // assert
                    Assert.AreEqual(count, categoryConstants.Count);
                }
            }
        }
    }
}