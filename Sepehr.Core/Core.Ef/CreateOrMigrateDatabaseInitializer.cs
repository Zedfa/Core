using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Core.Ef
{
    public class CreateOrMigrateDatabaseInitializer<TContext, TConfiguration> : CreateDatabaseIfNotExists<TContext>,
        IDatabaseInitializer<TContext>
        where TContext : DbContextBase

        where TConfiguration : DbMigrationsConfiguration<TContext>, new()
    {

        private readonly DbMigrationsConfiguration _configuration;
        private IEnumerable<string> _pendingMigrations;

        public CreateOrMigrateDatabaseInitializer()
        {
            _configuration = new TConfiguration();


        }


        public void CreateOrUpdateDatabase(TContext context, string queryFolder, string connectionString)
        {
           var migrator = new DbMigrator(_configuration);
            _pendingMigrations = migrator.GetPendingMigrations();

            if (context.Database.Exists())
            {
                if (!context.Database.CompatibleWithModel(throwIfNoMetadata: false) || _pendingMigrations.Any())
                {

                    foreach (var pmigration in _pendingMigrations)
                    {
                        migrator.Update(pmigration);
                        CustomCommandInMigrationUp(pmigration, queryFolder, connectionString);
                    }

                }
            }
            else
            {

                foreach (var pmigration in _pendingMigrations)
                {
                    migrator.Update(pmigration);
                    CustomCommandInMigrationUp(pmigration, queryFolder, connectionString);
                }


              
                //context.Database.Create();
                //Seed(context);
                //context.SaveChanges();
            }

        }

        public virtual void CustomCommandInMigrationUp(string pendingMigrationName, string queryFolder, string connectionString)
        {
            var migration = pendingMigrationName.Split('_');
            var migrationName = migration[1];

            string scriptDirectory = queryFolder + "\\" + migrationName;
            string sqlConnectionString = connectionString;
            string firstStepSp = scriptDirectory + "\\" + "FirstStepSp";
            string secondStepSp = scriptDirectory + "\\" + "SecondStepSp";
            var connection = new SqlConnection(sqlConnectionString);
            var server = new Server(new ServerConnection(connection));
            try
            {
                using (var scope = new TransactionScope())
                {
                    ExecuteFileInFirstStepSp(firstStepSp, server);
                    ExecuteFileInSecondStepSp(secondStepSp, server);
                    scope.Complete();
                }

            }
         
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }




        }

        private void ExecuteFileInSecondStepSp(string secondStepSp, Server server)
        {
            var di = new DirectoryInfo(secondStepSp);
            if (!di.Exists)
                return;
            FileInfo[] sqlFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in sqlFiles)
            {
                var fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();

                server.ConnectionContext.ExecuteNonQuery(script);
            }
        }

        private void ExecuteFileInFirstStepSp(string firstStepSp, Server server)
        {

            var di = new DirectoryInfo(firstStepSp);
            if (!di.Exists)
                return;

            FileInfo[] sqlFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in sqlFiles)
            {
                var fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();
                //var connection = new SqlConnection(sqlConnectionString);
                //var server = new Server(new ServerConnection(connection));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
        }

       
       
        //void IDatabaseInitializer<TContext>.InitializeDatabase(TContext context)
        //{

        //    if (context.Database.Exists())
        //    {
        //        if (!context.Database.CompatibleWithModel(throwIfNoMetadata: false))
        //        {
        //            var migrator = new DbMigrator(_configuration);
        //            migrator.Update();

        //        }
        //    }
        //    else
        //    {
        //        context.Database.Create();
        //        Seed(context);
        //        context.SaveChanges();
        //    }
        //}


       // protected virtual void Seed(TContext context)
       // {
            //For Create Index..
            // context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_Category_Title ON Categories (Title)");
            //string scriptDirectory = "c:\\temp\\sqltest\\";
            //string sqlConnectionString = "Integrated Security=SSPI;" +
            //    "Persist Security Info=True;Initial Catalog=Northwind;Data Source=(local)";
            //DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            //FileInfo[] rgFiles = di.GetFiles("*.sql");
            //foreach (FileInfo fi in rgFiles)
            //{
            //    FileInfo fileInfo = new FileInfo(fi.FullName);
            //    string script = fileInfo.OpenText().ReadToEnd();
            //    SqlConnection connection = new SqlConnection(sqlConnectionString);
            //    Server server = new Server(new ServerConnection(connection));
            //    server.ConnectionContext.ExecuteNonQuery(script);
            //}



        //}
    }
}