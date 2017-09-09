
using System;
using System.Threading;

namespace Core.Cmn
{
    public class DependencyInjectionFactory : IDependencyInjectionFactory
    {

        private Type dbContextType;

        public Type DbContextType
        {
            get
            {

                if (dbContextType == null)
                    dbContextType = AppBase.DependencyInjectionManager.Resolve<IDbContextBase>().GetType();
                return dbContextType;
            }
        }

        public IDbContextBase CreateContextInstance()
        {
            return Activator.CreateInstance(DbContextType) as IDbContextBase;
        }

        public T CreateInjectionInstance<T>()
        {
            return AppBase.DependencyInjectionManager.Resolve<T>();
        }
        public IRequest TryToResolveIRequest()
        {

            IRequest request = null;
            Core.Cmn.AppBase.AllRequests.TryGetValue(Thread.CurrentThread.ManagedThreadId, out request);
            return request;
        }
    }
}