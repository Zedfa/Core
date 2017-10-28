using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Cache.SqlDependency;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Core.UnitTesting.Mock
{
    [Injectable(InterfaceType = typeof(IImmediateSqlNotificationRegister<>), Version = 1)]
    public class MockImmediateSqlNotificationRegister<T> : IImmediateSqlNotificationRegister<T> where T : class
    {
        public event EventHandler<SqlNotificationEventArgs> OnChanged;

        public void Init(IDbContextBase context, IQueryable query)
        {
        }
    }
}