using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef
{
    public class DbContextSepehr : DbContext
    {
        static DbContextSepehr()
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public DbContextSepehr() { }

        public DbContextSepehr(string nameOrConnectionString) 
            : base(nameOrConnectionString) { }
    }
}
