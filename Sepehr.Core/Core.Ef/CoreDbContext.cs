using Core.Entity;
using Core.Entity.ConstantMapping;
using Core.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Ef
{
    public class CoreDbContext: DbContextBase
    {
        public CoreDbContext()
            : base("name=Main_ConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Constant> Constants { get; set; }

        public DbSet<ConstantCategory> ConstantCategories { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // CustomHistoryContextFactory.Create<HistoryContextBase>(this , );
            //modelBuilder.Entity<HistoryRow>().ToTable(tableName: "__MigrationHistory");
            //modelBuilder.Entity<HistoryRow>().HasKey(k=>k.MigrationId);
            modelBuilder.HasDefaultSchema("core");
            //modelBuilder.Configurations.Add(new ConstantMap());
            //modelBuilder.Configurations.Add(new ConstantCategoryMap());
            //modelBuilder.Configurations.Add(new ExceptionLogMap());
            //modelBuilder.Configurations.Add(new LogMap());
            base.OnModelCreating(modelBuilder);
        }
       
    }
}
