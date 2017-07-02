using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Core.Entity;
using Core.Cmn;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration.Conventions;
using Core.Ef.Mapping;
using System.Collections.Generic;
using System.Linq;
using Core.Cmn.Extensions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Core.Cmn.Attributes;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Entity.ModelConfiguration.Configuration;
using Core.Ef.Exceptions;

namespace Core.Ef
{
    [Injectable(DomainName = "core", InterfaceType = typeof(IDbContextBase), Version = -1)]
    public class DbContextBase : DbContext, IDbContextBase
    {
        static DbContextBase()
        {
            /* be sorate pish farz EntityFramework be EntityFramework.SqlServer.dll niaz darad ama ehtemalan be sorate reflectioni SqlServer ro call mikone
            banabarin  EntityFramework.SqlServer.dll dar zamane publish dar bin copy nemishavad. be hamin dalil instance iii az sqlprovider gerefte shod
            */
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            DbInterception.Add(new DbCommandInfoProvider());
            System.Data.Entity.Database.SetInitializer<DbContext>(null);
        }
        // FKK EDIT BEGIN
        //public DbContextBase()
        //    : base("Name=CoreDbContext")
        //{

        //}
        public DbContextBase()
            : base("Name=CoreDbContext")
        {

        }
        // FKK EDIT END

        public DbContextBase(string connectionString)
            : base(connectionString)
        {

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
        }





        public DbSet<Constant> Constants { get; set; }
        public DbSet<ConstantCategory> ConstantCategories { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyChart> CompanyCharts { get; set; }
        public DbSet<ViewElement> ViewElements { get; set; }
        public DbSet<TableInfo> TableNames { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CompanyChartRole> CompanyChartRoles { get; set; }
        public DbSet<ViewElementRole> ViewElementRoles { get; set; }
        public DbSet<UserConfig> UserConfigs { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<CoreUserLog> CoreUserLog { get; set; }
        public DbSet<CompanyViewElement> CompanyViewElements { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RouteMapConfig> RouteMapConfigs { get; set; }
        public DbSet<DeletedCachedRecord> DeletedCachedRecords { get; set; }

        public static int DefualtMaxRetriesCountForDeadlockRetry { get; set; }

        private int _maxRetriesCountForDeadlockRetry;
        public int MaxRetriesCountForDeadlockRetry
        {
            get
            {
                if (_maxRetriesCountForDeadlockRetry == 0)
                {
                    _maxRetriesCountForDeadlockRetry = DefualtMaxRetriesCountForDeadlockRetry;
                }

                return _maxRetriesCountForDeadlockRetry;
            }

            set
            {
                _maxRetriesCountForDeadlockRetry = value;
            }
        }

        public bool DisableExceptionLogger
        {
            get;
            set;
        }

        private Cmn.IDatabase _database;
        Cmn.IDatabase IDbContextBase.Database
        {
            get
            {
                if (_database == null)
                    _database = new Database(this.Database);
                return _database;
            }
        }

        private Cmn.IDbContextConfigurationBase _configuration;
        Cmn.IDbContextConfigurationBase IDbContextBase.Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new DbContextConfigurationBase(this.Configuration);
                }
                return _configuration;
            }
        }

        private Cmn.IDbChangeTrackerBase _changeTracker;
        Cmn.IDbChangeTrackerBase IDbContextBase.ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new DbChangeTrackerBase(this.ChangeTracker);
                }
                return _changeTracker;
            }
        }

        bool IDbContextBase.DisableExceptionLogger
        {
            get;
            set;
        }



        private static void CheckConstraint(DbUpdateConcurrencyException dbUpdateEx)
        {
            var exp = new DbUpdateConcurrencyExceptionBase(dbUpdateEx);

            if (dbUpdateEx != null
                    && dbUpdateEx.InnerException != null
                    && dbUpdateEx.InnerException.InnerException != null)
            {
                SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 2627:  // Unique constraint error
                        case 547:   // Constraint check violation
                        case 2601:  // Duplicated key row error
                                    // Constraint violation exception
                            {
                                ILogService _logService = Core.Cmn.AppBase.LogService;
                                var eLog = _logService.GetEventLogObj();
                                eLog.OccuredException = dbUpdateEx;
                                eLog.UserId = "SaveChanges()!";
                                eLog.CustomMessage = "An Constraint violation exception ocuured during saving changes.";
                                eLog.LogFileName = "DbContextBase";
                                _logService.Handle(eLog);
                                throw dbUpdateEx;
                            }
                    }
                }
            }
            throw exp;
        }

        private void CorrectPersianCharsContentEntities()
        {
            //System.Data.Entity.Validation.DbEntityValidationException
            //DbUpdateConcurrencyException

            var modifiedOrAddedEntities = GetAllModifiendOrAddEntities();

            foreach (_EntityBase entry in modifiedOrAddedEntities)
            {
                foreach (var prop in entry.EntityInfo().WritableMappedProperties)
                {
                    if (prop.Value.PropertyType == typeof(string))
                    {
                        var valuestr = entry[prop.Value.Name] as string;
                        if (valuestr != null)
                        {
                            entry[prop.Value.Name] = valuestr.CorrectPersianChars();
                        }
                    }
                }
            }

        }
        private List<object> GetAllModifiendOrAddEntities()
        {

            List<object> modifiedOrAddedEntities = (this as DbContext).ChangeTracker.Entries()
                .Where(entry => entry.State == System.Data.Entity.EntityState.Modified || entry.State == System.Data.Entity.EntityState.Added)
                .Select(entry => entry.Entity).ToList();
            return modifiedOrAddedEntities;
        }


        private static void ConfigureDecimalProperties(DbModelBuilder modelBuilder)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a =>
                a.ManifestModule.Name != "<In Memory Module>"
                && !a.FullName.StartsWith("System")
                && !a.FullName.StartsWith("Microsoft")
                && a.Location.IndexOf("App_Web") == -1
                && a.Location.IndexOf("App_global") == -1
                && a.FullName.IndexOf("CppCodeProvider") == -1
                && a.FullName.IndexOf("WebMatrix") == -1
                && a.FullName.IndexOf("SMDiagnostics") == -1
                && a.FullName.IndexOf("Stimulsoft") == -1
                && !string.IsNullOrEmpty(a.Location)
                );

            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    IEnumerable<Type> entityTypes = assembly.GetTypes()
                        .Where(
                        t => t.IsClass && t.GetType().IsAssignableFrom(typeof(EntityBase<>))
                        );
                    foreach (Type entityType in entityTypes)
                    {
                        var propAttrList = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<DecimalPrecisionAttribute>() != null).Select(
                             p => new { prop = p, attr = p.GetCustomAttribute<DecimalPrecisionAttribute>(true) });

                        foreach (var propAttr in propAttrList)
                        {

                            var entityConfig = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(entityType).Invoke(modelBuilder, null);

                            ParameterExpression param = ParameterExpression.Parameter(entityType, "c");
                            Expression property = Expression.Property(param, propAttr.prop.Name);
                            LambdaExpression lambdaExpression = Expression.Lambda(property, true,
                                                                                     new ParameterExpression[]
                                                                                         {param});
                            DecimalPropertyConfiguration decimalConfig;
                            if (propAttr.prop.PropertyType.IsGenericType && propAttr.prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[7];
                                decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                            }
                            else
                            {
                                MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[6];
                                decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                            }

                            decimalConfig.HasPrecision(propAttr.attr.Precision, propAttr.attr.Scale);
                        }
                    }
                }
                catch
                {
                    /// chon bazi az assembliha hich vaght load nemishan va azashoun estefade nemishe
                    /// dar halate aadi khataii daryaft nemikonim, vali inja ma hameye assembliha ra load mikonim
                    /// va chon assemblie dependencye an yafte nemishavad ba khata movajeh mishavad.
                }
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("core");


            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UserMap());

            modelBuilder.Configurations.Add(new CompanyChartMap());

            modelBuilder.Configurations.Add(new ViewElementMap());

            modelBuilder.Configurations.Add(new UserRoleMap());

            modelBuilder.Configurations.Add(new ViewElementRoleMap());


            ConfigureDecimalProperties(modelBuilder);

        }

        IDbSetBase<T> IDbContextBase.Set<T>()
        {
            return new DBSetBase<T>(this.Set<T>());
        }

        int IDbContextBase.SaveChanges()
        {

            //this.Set<User>().AsNoTracking().Include("UserProfile")

            int retryCount = 0;
            int delayTimeSeconds = 0;
            int rowEffect = 0;
            bool isDone = false;

            CorrectPersianCharsContentEntities();


            try
            {
                rowEffect = base.SaveChanges();
                isDone = true;
            }

            catch (DbUpdateConcurrencyException dbUpdateCncurrencyEx)
            {

                CheckConstraint(dbUpdateCncurrencyEx);
            }
            catch (DbUpdateException dDbUpdateException)
            {
                var exp = new DbUpdateExceptionBase(dDbUpdateException);
                throw exp;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEntityValidationEx)
            {
                var exp = new DbEntityValidationExceptionBase(dbEntityValidationEx);
                throw exp;
            }
            catch (System.Exception ex)
            {
                if (!DisableExceptionLogger)
                {
                    ILogService _logService = Core.Cmn.AppBase.LogService;
                    var eLog = _logService.GetEventLogObj();
                    eLog.OccuredException = ex;
                    eLog.UserId = "SaveChanges()!";
                    eLog.CustomMessage = "An exception ocuured during saving changes.";
                    eLog.LogFileName = "DbContextBase";
                    _logService.Handle(eLog);
                }
                throw ex;
            }

            if (!isDone)
                while (retryCount < MaxRetriesCountForDeadlockRetry)
                {
                    try
                    {
                        rowEffect = base.SaveChanges();
                    }

                    catch (DbUpdateConcurrencyException e)
                    {
                        CheckConstraint(e);
                    }
                    catch (DbUpdateException dDbUpdateException)
                    {
                        var exp = new DbUpdateExceptionBase(dDbUpdateException);
                        throw exp;
                    }
                    catch (SqlException e)
                    {

                        if (e.Number == 1205)  // SQL Server error code for deadlock
                        {
                            Task.Delay(delayTimeSeconds).Wait();
                            retryCount++;
                            delayTimeSeconds = retryCount * 5;
                        }
                        else
                        {
                            try
                            {
                                rowEffect = base.SaveChanges();
                            }
                            catch (System.Exception ex)
                            {
                                if (!DisableExceptionLogger)
                                {
                                    var eLog = Core.Cmn.AppBase.LogService.GetEventLogObj();
                                    eLog.OccuredException = ex;
                                    eLog.UserId = "SaveChanges()!";
                                    eLog.CustomMessage = "An exception for saving changes has ocuured.";
                                    Core.Cmn.AppBase.LogService.Handle(eLog);
                                }
                                throw ex;
                            }

                            throw e;  // Not a deadlock so throw the exception
                        }
                        // Add some code to do whatever you want with the exception once you've exceeded the max. retries
                    }
                }

            Task.Delay(delayTimeSeconds).Wait();

            //return base.SaveChanges();

            return rowEffect;

        }

        void IDbContextBase.SetContextState<T>(EntityBase<T> entity, Cmn.EntityState entityState)
        {
            var entry = this.Entry(entity);
            switch (entityState)
            {
                case Core.Cmn.EntityState.Added:
                    {
                        entry.State = System.Data.Entity.EntityState.Added;

                        break;
                    }
                case Core.Cmn.EntityState.Deleted:
                    {
                        entry.State = System.Data.Entity.EntityState.Deleted;

                        break;
                    }
                case Core.Cmn.EntityState.Detached:
                    {
                        entry.State = System.Data.Entity.EntityState.Detached;

                        break;
                    }
                case Core.Cmn.EntityState.Modified:
                    {
                        entry.State = System.Data.Entity.EntityState.Modified;

                        break;
                    }
                case Core.Cmn.EntityState.Unchanged:
                    {
                        entry.State = System.Data.Entity.EntityState.Unchanged;

                        break;
                    }

            }
        }

        //DbEntityEntry<TEntity> IDbContextBase.Entry<TEntity>(TEntity entity)
        //{
        //   return this.Entry(entity);
        //}
    }
}
