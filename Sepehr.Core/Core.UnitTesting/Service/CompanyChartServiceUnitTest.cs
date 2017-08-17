using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep;
using Core.Service;
using Core.UnitTesting.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.UnitTesting.Service
{
    [TestClass()]
    public class CompanyChartServiceUnitTest : ServiceUnitTestBase<ICompanyChartService, ICompanyChartRepository, CompanyChart>
    {
        protected override ICompanyChartService ConstructService()
        {
            IDbContextBase ctx = Mock.MockHelperBase.BuildMockContext();

            return new CompanyChartService(ctx, null);
        }

        private EntityUnitTestHelperBase<CompanyChart> _entityUnitTestHelper;

        protected override EntityUnitTestHelperBase<CompanyChart> EntityUnitTestHelper
        {
            get
            {
                return _entityUnitTestHelper ?? (_entityUnitTestHelper = new CompanyChartUnitTestHelper());
            }
        }

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // Core.Cmn.AppBase.LogService = new LogService(Core.Service.ServiceBase.DependencyInjectionFactory.CreateContextInstance());
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
    }
}