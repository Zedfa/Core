using Core.Cmn.Cache.SqlDependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cmn;
using Core.Cmn.Attributes;

namespace Core.UnitTesting.Mock
{
    [Injectable(InterfaceType = typeof(IImmediateSqlNotificationRegister<>), Version =1)]
    public class MockImmediateSqlNotificationRegister<T> : IImmediateSqlNotificationRegister<T> where T : class
    {
        public event EventHandler OnChanged;
        public void Init(IDbContextBase context, IQueryable query)
        {

        }
    }
}
