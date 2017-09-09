using System;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Cmn.Cache.SqlDependency
{
    public interface IImmediateSqlNotificationRegisterBase
    {
        event EventHandler<SqlNotificationEventArgs> OnChanged;

        void Init(IDbContextBase context, IQueryable query);
    }
}