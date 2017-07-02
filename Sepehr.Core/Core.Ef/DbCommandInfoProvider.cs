using Core.Cmn.Cache;
using System;

using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;

using System.Web;

namespace Core.Ef
{
    public class DbCommandInfoProvider : IDbCommandInterceptor,IDbInterceptor
    {
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<Int32> interceptionContext)
        {

        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<Int32> interceptionContext)
        {
            AddRequestHeadersToSqlCommand(command);
        }

        private void AddRequestHeadersToSqlCommand(DbCommand command)
        {
            var hashKey = command.CommandText.GetHashCode();
            if (CacheDataProvider.TimeStamps.ContainsKey(hashKey))
            {
                CacheDataProvider.TimeStamps[hashKey].ToList().ForEach(item =>
                {
                    if (command.CommandText.Contains(item + "  ="))
                    {
                        command.CommandText = command.CommandText.Replace(item + "  =", item + "  <");
                    }
                    else
                        if (command.CommandText.Contains("=  " + item))
                        {
                            command.CommandText = command.CommandText.Replace("=  " + item, "  >" + item);
                        }
                });
            }

            String url = "", ip = "", queryString = "", user = "", stackTrace = "";
            // if (bool.Parse(Core.Cmn.ConfigHelper.GetConfigValue("WritingLogInSqlQueryByEFEnabled")))
            if (Core.Cmn.ConfigHelper.GetConfigValue<bool>("WritingLogInSqlQueryByEFEnabled"))
                stackTrace = new StackTrace().ToString();

            if (HttpContext.Current != null && HttpContext.Current.Handler != null && HttpContext.Current.Request != null)
            {
                url = HttpContext.Current.Request.Url.ToString();
                queryString = HttpContext.Current.Request.Url.Query;
                // user = Sepehr360CookieHelper.UserNameAgency ?? Sepehr360CookieHelper.UserNamePanel;
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrEmpty(ip)) ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (String.IsNullOrEmpty(ip)) ip = HttpContext.Current.Request.UserHostAddress;
            }

            command.CommandText += Environment.NewLine + " /* " +
                String.Format("{0} URL : {1} , {0} IP : {2} , {0} QueryString : {3} , {0} User : {4} , {0} Stack Trace : {5} */", Environment.NewLine, url, ip, queryString, user, stackTrace);
            //Core.Cmn.ExceptionHandler.Handle(new Exception(), ip, command.CommandText);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            AddRequestHeadersToSqlCommand(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<Object> interceptionContext)
        {

        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<Object> interceptionContext)
        {
            AddRequestHeadersToSqlCommand(command);
        }
    }

}
