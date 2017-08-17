using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using Core.UnitTesting.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Repository
{
    [TestClass()]
    public class LogRepositoryTests : RepositoryUnitTestBase<ILogRepository, Log>
    {
        private static IDbContextBase BuildNewContext()
        {
            return Mock.MockHelperBase.BuildMockContext();
        }

        protected override ILogRepository BuildRepository()
        {
            return new LogRepository(CreateNewContext());
        }

        protected override IDbContextBase CreateNewContext()
        {
            return BuildNewContext();
        }

        protected override string GetTableName()
        {
            return "Logs";
        }

        protected override string GetSchemaName()
        {
            return "core";
        }

        private EntityUnitTestHelperBase<Log> _entityUnitTestHelper;

        protected override EntityUnitTestHelperBase<Log> EntityUnitTestHelper
        {
            get
            {
                return _entityUnitTestHelper ?? (_entityUnitTestHelper = new LogUnitTestHelper());
            }
        }

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Core.Cmn.AppBase.LogService = new Core.Service.LogService(BuildNewContext());
            // Core.Cmn.AppBase.BuildEntityInfoDic(Core.Cmn.AppBase.GetAlltypes());
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
            using (ILogRepository repository = BuildRepository())
            {
                // assemble
                Log log = EntityUnitTestHelper.CreateSampleEntity();
                log = repository.Create(log);

                // act
                Log found = repository.Find(log.Id);

                // assert
                Assert.IsNotNull(found);
                EntityUnitTestHelper.AssertEntitiesAreEqual(log, found);
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
    }
}