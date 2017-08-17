using Core.Cmn;

namespace Core.UnitTesting.Mock
{
    public class MockDbContextConfiguration : IDbContextConfigurationBase
    {
        public bool AutoDetectChangesEnabled
        {
            get; set;
        }

        public bool EnsureTransactionsForFunctionsAndCommands
        {
            get; set;
        }

        public bool LazyLoadingEnabled
        {
            get; set;
        }

        public bool ProxyCreationEnabled
        {
            get; set;
        }

        public bool UseDatabaseNullSemantics
        {
            get; set;
        }

        public bool ValidateOnSaveEnabled
        {
            get; set;
        }
    }
}