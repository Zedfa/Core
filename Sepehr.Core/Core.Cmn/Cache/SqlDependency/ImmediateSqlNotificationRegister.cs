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


using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Cache.SqlDependency;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Cmn.Cache
{
    [Injectable(InterfaceType = typeof(IImmediateSqlNotificationRegister<>))]
    public class ImmediateSqlNotificationRegister<TEntity> : IImmediateSqlNotificationRegister<TEntity>, IDisposable
        where TEntity : class
    {
        private SqlConnection connection = null;
        private SqlCommand command = null;
        private IQueryable iquery = null;

        // Summary:
        //     Occurs when a notification is received for any of the commands associated
        //     with this ImmediateNotificationRegister object.
        public event EventHandler OnChanged;
        private System.Data.SqlClient.SqlDependency dependency = null;

        /// <summary>
        /// Initializes a new instance of ImmediateNotificationRegister class.
        /// </summary>
        /// <param name="context">an instance of DbContext is used to get an ObjectQuery object</param>
        /// <param name="query">an instance of IQueryable is used to get ObjectQuery object, and then get  
        /// connection string and command string to register SqlDependency nitification. </param>
        public ImmediateSqlNotificationRegister(IDbContextBase context, IQueryable query)
        {
            Init(context, query);
        }

        public void Init(IDbContextBase context, IQueryable query)
        {
            try
            {
                StartMonitor(context);
                this.iquery = query;

                // Get the ObjectQuery directly or convert the DbQuery to ObjectQuery.
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
                    "Fails to initialize a new instance of ImmediateNotificationRegister class.", ex);
            }
        }

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

        private void DependencyOnChange(object sender, SqlNotificationEventArgs e)
        {
            // Move the original SqlDependency event handler.
            System.Data.SqlClient.SqlDependency dependency = (System.Data.SqlClient.SqlDependency)sender;
            dependency.OnChange -= DependencyOnChange;

            if (OnChanged != null)
            {
                try
                {
                    OnChanged(this, null);
                }
                catch
                {

                }
            }


            // We re-register the SqlDependency.
            //  RegisterSqlDependency();
        }

        private void RegisterSqlCommand()
        {
            if (connection != null && command != null)
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
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

        /// <summary>
        /// The SqlConnection is got from the Query.
        /// </summary>
        public SqlConnection Connection
        { get { return connection; } }

        /// <summary>
        /// The SqlCommand is got from the Query.
        /// </summary>
        public SqlCommand Command
        { get { return command; } }
    }
}
