using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Cache.SqlDependency
{
    public interface IImmediateSqlNotificationRegisterBase
    {
        event EventHandler OnChanged;
        void Init(IDbContextBase context, IQueryable query);
    }
}
