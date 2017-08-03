/****************************** Module Header ******************************\
Module Name:  RegularlyNotificationRegister.cs
Project:      CSEFAutoUpdate
Copyright (c) Microsoft Corporation.

We can use the Sqldependency to get the notification when the data is changed
in database, but EF doesn’t have the same feature. In this sample, we will
demonstrate how to automatically update by Sqldependency in Entity Framework.
In this sample, we will demonstrate two ways that use SqlDependency to get the
change notification to auto update data.
We can regularly detect the changes by this class.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/en-us/openness/licenses.aspx#MPL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Core.Cmn.Cache
{
    public class RegularlySqlNotificationRegister<TEntity> : IDisposable
    where TEntity : class
    {
        private SqlCommand command = null;
        private SqlConnection connection = null;
        private int count = 0;
        private System.Data.SqlClient.SqlDependency dependency = null;
        // private ObjectQuery oquery = null;
        private Int32 interval = -1;

        private IQueryable iquery = null;

        /// <summary>
        /// Initializes a new instance of RegularlyNotificationRegister class.
        /// </summary>
        /// <param name="context">an instance of DbContext is used to get an ObjectQuery object</param>
        /// <param name="query">an instance of IQueryable is used to get ObjectQuery object, and then get
        /// connection string and command string to register SqlDependency nitification. </param>
        /// <param name="interval">The time interval between invocations of callback, in milliseconds.</param>
        public RegularlySqlNotificationRegister(IDbContextBase context, IQueryable query, Int32 interval)
        {
            try
            {
                this.iquery = query;
                this.interval = interval;
                Core.Cmn.Extensions.QueryableExt.GetSqlCommand<TEntity>(context, query, ref connection, ref command);
                RegisterSqlDependency();
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "context")
                {
                    throw new ArgumentException("Paramter cannot be null", "context", ex);
                }
                else
                {
                    throw new ArgumentException("Paramter cannot be null", "query", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Fails to initialize a new instance of RegularlyNotificationRegister class.", ex);
            }
        }

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

        /// <summary>
        /// Starts the notification of SqlDependency
        /// </summary>
        /// <param name="context">An instance of dbcontext</param>
        public static void StartMonitor(IDbContextBase context)
        {
            try
            {
                System.Data.SqlClient.SqlDependency.Start(context.Database.Connection.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Fails to Start the SqlDependency in the RegularlyNotificationRegister class", ex);
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
                System.Data.SqlClient.SqlDependency.Stop(context.Database.Connection.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Fails to Stop the SqlDependency in the RegularlyNotificationRegister class", ex);
            }
        }
        /// <summary>
        /// Releases all the resources by the RegularlyNotificationRegister.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                    command = null;
                }

                iquery = null;
                dependency = null;
            }
        }

        private void RegisterSqlCommand()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            if (connection != null && command != null && count == 0)
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            s.Stop();
            System.Diagnostics.Debug.WriteLine($"connection time:{s.ElapsedMilliseconds}");
        }

        private void RegisterSqlDependency()
        {
            if (connection == null || command == null)
            {
                throw new ArgumentException("command and connection cannot be null");
            }

            // Make sure the command object does not already have
            // a notification object associated with it.
            command.Notification = null;
            // Create and bind the SqlDependency object
            // to the command object.
            dependency = new System.Data.SqlClient.SqlDependency(command);
            Console.WriteLine("Id of sqldependency:{0}", dependency.Id);
            RegisterSqlCommand();
        }
    }
}