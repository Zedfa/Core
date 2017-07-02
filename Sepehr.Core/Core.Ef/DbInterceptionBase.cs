
using System.Data.Entity.Infrastructure.Interception;


namespace Core.Ef
{

    public class DbInterceptionBase
    {
        // Summary:
        //     Registers a new System.Data.Entity.Infrastructure.Interception.IDbInterceptor
        //     to receive notifications. Note that the interceptor must implement some interface
        //     that extends from System.Data.Entity.Infrastructure.Interception.IDbInterceptor
        //     to be useful.
        //
        // Parameters:
        //   interceptor:
        //     The interceptor to add.
        public static void Add(DbCommandInfoProvider interceptor)
        {
            DbInterception.Add(interceptor);
        }
        //
        // Summary:
        //     Removes a registered System.Data.Entity.Infrastructure.Interception.IDbInterceptor
        //     so that it will no longer receive notifications. If the given interceptor is
        //     not registered, then this is a no-op.
        //
        // Parameters:
        //   interceptor:
        //     The interceptor to remove.
        public static void Remove(DbCommandInfoProvider interceptor)
        {
            DbInterception.Remove(interceptor);
        }
    }
    
}
