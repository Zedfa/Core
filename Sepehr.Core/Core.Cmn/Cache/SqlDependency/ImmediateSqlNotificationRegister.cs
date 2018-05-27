/****************************** Module Header ******************************\
Module Name:  ChangeNotificationRegister.cs
Project:      CSEFAutoUpdate
Copyright (c) Microsoft Corporation.

We can use the Sqldependency to get the notification when the data is changed
in database, but EF doesn’t have the same feature. In this sample, we will
demonstrate how to automatically update by Sqldependency in Entity Framework.
In this sample, we will demonstrate two ways that use SqlDependency to get the
change notification to auto update data.
We can get the notification immediately by this class when the data changed.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/en-us/openness/licenses.aspx#MPL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using Core.Cmn.Attributes;
using Core.Cmn.Cache.SqlDependency;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    [Injectable(InterfaceType = typeof(IImmediateSqlNotificationRegister<>))]
    public class ImmediateSqlNotificationRegister<TEntity> : IImmediateSqlNotificationRegister<TEntity>, IDisposable
        where TEntity : class
    {
        private SqlCommand command = null;
        private SqlConnection connection = null;
        private System.Data.SqlClient.SqlDependency dependency = null;
        private IQueryable iquery = null;

        /// <summary>
        /// Initializes a new instance of ImmediateNotificationRegister class.
        /// </summary>
        /// <param name="context">an instance of DbContext is used to get an ObjectQuery object</param>
        /// <param name="query">an instance of IQueryable is used to get ObjectQuery object, and then get
        /// connection string and command string to register SqlDependency nitification. </param>
        public ImmediateSqlNotificationRegister(IDbContextBase context, IQueryable query, CacheInfo cacheInfo)
        {
            Init(context, query, cacheInfo);
        }

        public void Init(IDbContextBase context, IQueryable query, CacheInfo cacheInfo)
        {
            CacheInfo = cacheInfo;
            using (var trace = new Trace.TraceDto())
            {
                trace.TraceKey = "CacheSqlDependency";
                trace.Data["CacheName"] = cacheInfo.Name;
                trace.Data["DateTime"] = DateTime.Now.ToString();
                trace.Message = $"CacheSql Dependency {cacheInfo.Name} before init at {DateTime.Now.ToString()} ...";
                trace.Data["State"] = "initialize";
                Core.Cmn.AppBase.TraceWriter.SubmitData(trace);
            }

            Init(context, query, 500, cacheInfo);

            using (var trace = new Trace.TraceDto())
            {
                trace.TraceKey = "CacheSqlDependency";
                trace.Data["CacheName"] = cacheInfo.Name;
                trace.Data["DateTime"] = DateTime.Now.ToString();
                trace.Message = $"CacheSql Dependency {cacheInfo.Name} was initilized at {DateTime.Now.ToString()} ...";
                trace.Data["State"] = "initialize";
                Core.Cmn.AppBase.TraceWriter.SubmitData(trace);
            }
        }

        // Summary:
        //     Occurs when a notification is received for any of the commands associated
        //     with this ImmediateNotificationRegister object.
        public event EventHandler<SqlNotificationEventArgs> OnChanged;
        /// <summary>
        /// The SqlCommand is got from the Query.
        /// </summary>
        public SqlCommand Command
        { get { return command; } }

        /// <summary>
        /// The SqlConnection is got from the Query.
        /// </summary>
        public SqlConnection Connection
        { get { return connection; } }

        public CacheInfo CacheInfo { get; private set; }

        /// <summary>
        /// Starts the notification of SqlDependency
        /// </summary>
        /// <param name="context">An instance of dbcontext</param>
        public static void StartMonitor(IDbContextBase context)
        {
            try
            {
                System.Data.SqlClient.SqlDependency.Start(((IDbContextInternal)context).ConnectionString);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Fails to Start the SqlDependency in the ImmediateNotificationRegister class", ex);
            }
        }

        /// <summary>
        /// Stops the notification of SqlDependency
        /// </summary>
        /// <param name="context">An instance of dbcontext</param>
        public static void StopMonitor(IDbContextBase context)
        {
            try
            {
                System.Data.SqlClient.SqlDependency.Stop((((IDbContextInternal)context).ConnectionString));
            }
            catch (Exception ex)
            {
                throw new System.Exception("Fails to Stop the SqlDependency in the ImmediateNotificationRegister class", ex);
            }
        }

        /// <summary>
        /// Releases all the resources by the ImmediateNotificationRegister.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Init(IDbContextBase context, IQueryable query, int delayForRetryOnError, CacheInfo cacheInfo)
        {
            try
            {
                StartMonitor(context);
                this.iquery = query;

                // Get the ObjectQuery directly or convert the DbQuery to ObjectQuery.
                Core.Cmn.Extensions.QueryableExt.GetSqlCommand<TEntity>(context, query, ref connection, ref command);
                RegisterSqlDependency();
            }
            catch (Exception ex)
            {
                try
                {
                    using (var trace = new Trace.TraceDto())
                    {
                        trace.TraceKey = "CacheSqlDependency";
                        trace.Data["CacheName"] = cacheInfo.Name;
                        trace.Data["DateTime"] = DateTime.Now.ToString();
                        trace.Data["Exception"] = ex.ToString();
                        trace.Message = $"CacheSql Dependency {cacheInfo.Name} has faced with an exception at {DateTime.Now.ToString()} ...";
                        trace.Data["State"] = "initialize";
                        Core.Cmn.AppBase.TraceWriter.SubmitData(trace);
                    }

                    Core.Cmn.AppBase.LogService.Handle(ex, $"Exception on initialize ImmediateNotificationRegister for cache on SqlDependency, the query is {iquery}...");

                }
                catch
                {

                }

                Task.Delay(delayForRetryOnError).Wait();
                Init(context, query, delayForRetryOnError + 1000, cacheInfo);
            }
        }
        protected void Dispose(Boolean disposed)
        {
            if (disposed)
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }

                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }

                OnChanged = null;
                iquery = null;
                dependency.OnChange -= DependencyOnChange;
                dependency = null;
            }
        }

        private void DependencyOnChange(object sender, SqlNotificationEventArgs e)
        {
            // Move the original SqlDependency event handler.
            if (e.Type == SqlNotificationType.Subscribe || e.Info == SqlNotificationInfo.Error)
            {
                using (var trace = new Trace.TraceDto())
                {
                    trace.TraceKey = "CacheSqlDependency";
                    trace.Data["CacheName"] = CacheInfo.Name;
                    trace.Data["DateTime"] = DateTime.Now.ToString();
                    trace.Message = $"CacheSql Dependency {CacheInfo.Name} has faced with an Error at {DateTime.Now.ToString()} ...";
                    trace.Data["State"] = "Error";
                    trace.Data["Type"] = e.Type.ToString();
                    trace.Data["Info"] = e.Info.ToString();
                    Core.Cmn.AppBase.TraceWriter.SubmitData(trace);
                }

                Core.Cmn.AppBase.LogService.Write($"Some thing is wrong on SqlDependency Change, the query is {iquery} and SqlNotificationInfo is {e.Info} and Source is {e.Source}...");
            }

            dependency.OnChange -= DependencyOnChange;

            if (OnChanged != null)
            {
                try
                {
                    OnChanged(this, e);
                }
                catch (Exception ex)
                {
                    using (var trace = new Trace.TraceDto())
                    {
                        trace.TraceKey = "CacheSqlDependency";
                        trace.Data["CacheName"] = CacheInfo.Name;
                        trace.Data["DateTime"] = DateTime.Now.ToString();
                        trace.Message = $"CacheSql Dependency {CacheInfo.Name} has faced with an exception at {DateTime.Now.ToString()} ...";
                        trace.Data["State"] = "Error";
                        trace.Data["Exception"] = ex.ToString();
                        trace.Data["Type"] = e.Type.ToString();
                        trace.Data["Info"] = e.Info.ToString();
                        Core.Cmn.AppBase.TraceWriter.SubmitData(trace);
                    }

                    Core.Cmn.AppBase.LogService.Handle(ex, $"Exception on executing cache on SqlDependency Change, the query is {iquery}...");
                }
            }

            // We re-register the SqlDependency.
            //  RegisterSqlDependency();
        }

        private void RegisterSqlCommand()
        {
            if (connection != null && command != null)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void RegisterSqlDependency()
        {
            if (command == null || connection == null)
            {
                throw new ArgumentException("command and connection cannot be null");
            }

            // Make sure the command object does not already have
            // a notification object associated with it.
            command.Notification = null;

            // Create and bind the SqlDependency object to the command object.
            dependency = new System.Data.SqlClient.SqlDependency(command);
            dependency.OnChange += new OnChangeEventHandler(DependencyOnChange);

            // After register SqlDependency, the SqlCommand must be executed, or we can't
            // get the notification.
            RegisterSqlCommand();
        }
    }
}