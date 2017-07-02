
using System;



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
    }
}