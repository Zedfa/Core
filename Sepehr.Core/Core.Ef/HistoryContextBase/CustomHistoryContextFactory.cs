using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations;
using System.Data.Common;


namespace Core.Ef.HistoryContextBase
{
    public class CustomHistoryContextFactory  //IHistoryContextFactory
    {
        //private DbMigrationsConfiguration _customMigrationsConfigurations;

        //public CustomHistoryContextFactory(){
        //    _customMigrationsConfigurations<>
        //}

        //public static void Create<THistoryContext>(DbMigrationsConfiguration dbMigrationsConfiguration , DbConnection existingConnection, bool contextOwnsConnection, string defaultSchema) where THistoryContext : HistoryContext
        //{
        //   // as T; typeof(THistoryContext)
        //    dbMigrationsConfiguration.SetHistoryContextFactory("System.Data.SqlClient", (existingConnection, contextOwnsConnection, defaultSchema) => { return GetHistoryContext<THistoryContext>(existingConnection, contextOwnsConnection, defaultSchema); });
        //    new HistoryContextBase(existingConnection, contextOwnsConnection, defaultSchema);
        //}


        //public static Func<DbConnection, string, HistoryContext> InitializeHistoryContext<THistoryContext>(DbConnection existingConnection, bool contextOwnsConnection, string defaultSchema)
        //{
        //    Func<DbConnection, string, HistoryContext> historyContextFunc ;
        //    historyContextFunc = (a, b) => {


        //        return GetHistoryContext<THistoryContext>(existingConnection, contextOwnsConnection, defaultSchema); 
            
        //    }; 
        //    return  historyContextFunc;
        //}

        //private static HistoryContext GetHistoryContext<THistoryContext>(DbConnection existingConnection, bool contextOwnsConnection, string defaultSchema)
        //{
        //    var historyContext = Activator.CreateInstance(typeof(THistoryContext), new object[] { existingConnection, defaultSchema }) as HistoryContext;
        //    return historyContext;
        //}
   }
}
