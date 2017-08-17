using Core.Cmn;

namespace Core.UnitTesting.Mock
{
    public static class MockHelperBase
    {
        public static IDbContextBase BuildMockContext()
        {
            return new MockDbContextBase();
        }

        public static IDatabase BuildMockDatabase()
        {
            return new MockDatabase();
        }

        public static IDbContextConfigurationBase BuildMockDbContextConfiguration()
        {
            return new MockDbContextConfiguration();
        }
    }
}