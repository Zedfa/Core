﻿using Core.Cmn;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;


namespace Core.Ef
{
    public class Database : IDatabase
    {
        private System.Data.Entity.Database _database;
        public Database(System.Data.Entity.Database database)
        {
            _database = database;
        }

        //
        // Summary:
        //     The connection factory to use when creating a System.Data.Common.DbConnection
        //     from just a database name or a connection string.
        //
        // Remarks:
        //     This is used when just a database name or connection string is given to System.Data.Entity.DbContext
        //     or when the no database name or connection is given to DbContext in which case
        //     the name of the context class is passed to this factory in order to generate
        //     a DbConnection. By default, the System.Data.Entity.Infrastructure.IDbConnectionFactory
        //     instance to use is read from the application's .config file from the "EntityFramework
        //     DefaultConnectionFactory" entry in appSettings. If no entry is found in the config
        //     file then System.Data.Entity.Infrastructure.SqlConnectionFactory is used. Setting
        //     this property in code always overrides whatever value is found in the config
        //     file.
        [Obsolete("The default connection factory should be set in the config file or using the DbConfiguration class. (See http://go.microsoft.com/fwlink/?LinkId=260883)")]
        public static IDbConnectionFactory DefaultConnectionFactory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        //
        // Summary:
        //     Gets or sets the timeout value, in seconds, for all context operations. The default
        //     value is null, where null indicates that the default value of the underlying
        //     provider will be used.
        public int? CommandTimeout
        {
            get
            {
                return _database.CommandTimeout;
            }
            set
            {
                _database.CommandTimeout = value;
            }
        }
        //
        // Summary:
        //     Returns the connection being used by this context. This may cause the connection
        //     to be created if it does not already exist.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        public DbConnection Connection
        {
            get
            {
                return _database.Connection;
            }

        }
        //
        // Summary:
        //     Gets the transaction the underlying store connection is enlisted in. May be null.
        public DbContextTransaction CurrentTransaction
        {
            get
            {
                return _database.CurrentTransaction;
            }

        }
        //
        // Summary:
        //     Set this property to log the SQL generated by the System.Data.Entity.DbContext
        //     to the given delegate. For example, to log to the console, set this property
        //     to System.Console.Write(System.String).
        //
        // Remarks:
        //     The format of the log text can be changed by creating a new formatter that derives
        //     from System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter and
        //     setting it with System.Data.Entity.DbConfiguration.SetDatabaseLogFormatter(System.Func{System.Data.Entity.DbContext,System.Action{System.String},System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter}).
        //     For more low-level control over logging/interception see System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor
        //     and System.Data.Entity.Infrastructure.Interception.DbInterception.
        public Action<string> Log
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        //
        // Summary:
        //     Deletes the database on the database server if it exists, otherwise does nothing.
        //
        // Parameters:
        //   existingConnection:
        //     An existing connection to the database.
        //
        // Returns:
        //     True if the database did exist and was deleted{throw new NotImplementedException();} false otherwise.
        public static bool Delete(DbConnection existingConnection) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Deletes the database on the database server if it exists, otherwise does nothing.
        //     The connection to the database is created using the given database name or connection
        //     string in the same way as is described in the documentation for the System.Data.Entity.DbContext
        //     class.
        //
        // Parameters:
        //   nameOrConnectionString:
        //     The database name or a connection string to the database.
        //
        // Returns:
        //     True if the database did exist and was deleted{throw new NotImplementedException();} false otherwise.
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool Delete(string nameOrConnectionString) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Checks whether or not the database exists on the server.
        //
        // Parameters:
        //   existingConnection:
        //     An existing connection to the database.
        //
        // Returns:
        //     True if the database exists{throw new NotImplementedException();} false otherwise.
        public static bool Exists(DbConnection existingConnection) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Checks whether or not the database exists on the server. The connection to the
        //     database is created using the given database name or connection string in the
        //     same way as is described in the documentation for the System.Data.Entity.DbContext
        //     class.
        //
        // Parameters:
        //   nameOrConnectionString:
        //     The database name or a connection string to the database.
        //
        // Returns:
        //     True if the database exists{throw new NotImplementedException();} false otherwise.
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static bool Exists(string nameOrConnectionString) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Sets the database initializer to use for the given context type. The database
        //     initializer is called when a the given System.Data.Entity.DbContext type is used
        //     to access a database for the first time. The default strategy for Code First
        //     contexts is an instance of System.Data.Entity.CreateDatabaseIfNotExists`1.
        //
        // Parameters:
        //   strategy:
        //     The initializer to use, or null to disable initialization for the given context
        //     type.
        //
        // Type parameters:
        //   TContext:
        //     The type of the context.
        public static void SetInitializer<TContext>(IDatabaseInitializer<TContext> strategy) where TContext : DbContext { throw new NotImplementedException(); }
        //
        // Summary:
        //     Begins a transaction on the underlying store connection
        //
        // Returns:
        //     a System.Data.Entity.DbContextTransaction object wrapping access to the underlying
        //     store's transaction object
        public IDbContextTransactionBase BeginTransaction()
        {
            return new DbContextTransactionBase(_database.BeginTransaction());

        }
        //
        // Summary:
        //     Begins a transaction on the underlying store connection using the specified isolation
        //     level
        //
        // Parameters:
        //   isolationLevel:
        //     The database isolation level with which the underlying store transaction will
        //     be created
        //
        // Returns:
        //     a System.Data.Entity.DbContextTransaction object wrapping access to the underlying
        //     store's transaction object
        public IDbContextTransactionBase BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DbContextTransactionBase(_database.BeginTransaction(isolationLevel));
        }
        //
        // Summary:
        //     Checks whether or not the database is compatible with the the current Code First
        //     model.
        //
        // Parameters:
        //   throwIfNoMetadata:
        //     If set to true then an exception will be thrown if no model metadata is found
        //     in the database. If set to false then this method will return true if metadata
        //     is not found.
        //
        // Returns:
        //     True if the model hash in the context and the database match{throw new NotImplementedException();} false otherwise.
        //
        // Remarks:
        //     Model compatibility currently uses the following rules. If the context was created
        //     using either the Model First or Database First approach then the model is assumed
        //     to be compatible with the database and this method returns true. For Code First
        //     the model is considered compatible if the model is stored in the database in
        //     the Migrations history table and that model has no differences from the current
        //     model as determined by Migrations model differ. If the model is not stored in
        //     the database but an EF 4.1/4.2 model hash is found instead, then this is used
        //     to check for compatibility.
        public bool CompatibleWithModel(bool throwIfNoMetadata) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Creates a new database on the database server for the model defined in the backing
        //     context. Note that calling this method before the database initialization strategy
        //     has run will disable executing that strategy.
        public void Create() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Creates a new database on the database server for the model defined in the backing
        //     context, but only if a database with the same name does not already exist on
        //     the server.
        //
        // Returns:
        //     True if the database did not exist and was created{throw new NotImplementedException();} false otherwise.
        public bool CreateIfNotExists() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Deletes the database on the database server if it exists, otherwise does nothing.
        //     Calling this method from outside of an initializer will mark the database as
        //     having not been initialized. This means that if an attempt is made to use the
        //     database again after it has been deleted, then any initializer set will run again
        //     and, usually, will try to create the database again automatically.
        //
        // Returns:
        //     True if the database did exist and was deleted{throw new NotImplementedException();} false otherwise.
        public bool Delete() { throw new NotImplementedException(); }

        //
        // Summary:
        //     Executes the given DDL/DML command against the database. As with any API that
        //     accepts SQL it is important to parameterize any user input to protect against
        //     a SQL injection attack. You can include parameter place holders in the SQL query
        //     string and then supply parameter values as additional arguments. Any parameter
        //     values you supply will automatically be converted to a DbParameter. context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively,
        //     you can also construct a DbParameter and supply it to SqlQuery. This allows you
        //     to use named parameters in the SQL query string. context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author",
        //     userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   sql:
        //     The command string.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     The result returned by the database after executing the command.
        //
        // Remarks:
        //     If there isn't an existing local or ambient transaction a new transaction will
        //     be used to execute the command.
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _database.ExecuteSqlCommand(sql, parameters);
        }
        //
        // Summary:
        //     Executes the given DDL/DML command against the database. As with any API that
        //     accepts SQL it is important to parameterize any user input to protect against
        //     a SQL injection attack. You can include parameter place holders in the SQL query
        //     string and then supply parameter values as additional arguments. Any parameter
        //     values you supply will automatically be converted to a DbParameter. context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively,
        //     you can also construct a DbParameter and supply it to SqlQuery. This allows you
        //     to use named parameters in the SQL query string. context.Database.ExecuteSqlCommand("UPDATE
        //     dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author",
        //     userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   transactionalBehavior:
        //     Controls the creation of a transaction for this command.
        //
        //   sql:
        //     The command string.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     The result returned by the database after executing the command.
        public int ExecuteSqlCommand(Core.Cmn.TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            var behavior = GetTransactionalBehavior(transactionalBehavior);
            return _database.ExecuteSqlCommand(behavior, sql, parameters);
        }
        private System.Data.Entity.TransactionalBehavior GetTransactionalBehavior(Core.Cmn.TransactionalBehavior transactionalBehavior)
        {

            switch (transactionalBehavior)
            {
                case Core.Cmn.TransactionalBehavior.EnsureTransaction:
                    return System.Data.Entity.TransactionalBehavior.EnsureTransaction;
                case Core.Cmn.TransactionalBehavior.DoNotEnsureTransaction:
                    return System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction;
                default:
                    return System.Data.Entity.TransactionalBehavior.EnsureTransaction;
            }
        }


        //
        // Summary:
        //     Asynchronously executes the given DDL/DML command against the database. As with
        //     any API that accepts SQL it is important to parameterize any user input to protect
        //     against a SQL injection attack. You can include parameter place holders in the
        //     SQL query string and then supply parameter values as additional arguments. Any
        //     parameter values you supply will automatically be converted to a DbParameter.
        //     context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating = 5 WHERE
        //     Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively, you can also construct a DbParameter
        //     and supply it to SqlQuery. This allows you to use named parameters in the SQL
        //     query string. context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating
        //     = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   sql:
        //     The command string.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     A task that represents the asynchronous operation. The task result contains the
        //     result returned by the database after executing the command.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context. If there isn't an existing local transaction
        //     a new transaction will be used to execute the command.
        public Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Asynchronously executes the given DDL/DML command against the database. As with
        //     any API that accepts SQL it is important to parameterize any user input to protect
        //     against a SQL injection attack. You can include parameter place holders in the
        //     SQL query string and then supply parameter values as additional arguments. Any
        //     parameter values you supply will automatically be converted to a DbParameter.
        //     context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating = 5 WHERE
        //     Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively, you can also construct a DbParameter
        //     and supply it to SqlQuery. This allows you to use named parameters in the SQL
        //     query string. context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating
        //     = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   transactionalBehavior:
        //     Controls the creation of a transaction for this command.
        //
        //   sql:
        //     The command string.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     A task that represents the asynchronous operation. The task result contains the
        //     result returned by the database after executing the command.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        public Task<int> ExecuteSqlCommandAsync(Core.Cmn.TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {

            var behavior = GetTransactionalBehavior(transactionalBehavior);
            return _database.ExecuteSqlCommandAsync(behavior, sql, parameters);
        }
        //
        // Summary:
        //     Asynchronously executes the given DDL/DML command against the database. As with
        //     any API that accepts SQL it is important to parameterize any user input to protect
        //     against a SQL injection attack. You can include parameter place holders in the
        //     SQL query string and then supply parameter values as additional arguments. Any
        //     parameter values you supply will automatically be converted to a DbParameter.
        //     context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating = 5 WHERE
        //     Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively, you can also construct a DbParameter
        //     and supply it to SqlQuery. This allows you to use named parameters in the SQL
        //     query string. context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating
        //     = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   sql:
        //     The command string.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     A task that represents the asynchronous operation. The task result contains the
        //     result returned by the database after executing the command.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context. If there isn't an existing local transaction
        //     a new transaction will be used to execute the command.
        public Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters) { throw new NotImplementedException(); }
        //
        // Summary:
        //     Asynchronously executes the given DDL/DML command against the database. As with
        //     any API that accepts SQL it is important to parameterize any user input to protect
        //     against a SQL injection attack. You can include parameter place holders in the
        //     SQL query string and then supply parameter values as additional arguments. Any
        //     parameter values you supply will automatically be converted to a DbParameter.
        //     context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating = 5 WHERE
        //     Author = @p0", userSuppliedAuthor){throw new NotImplementedException();} Alternatively, you can also construct a DbParameter
        //     and supply it to SqlQuery. This allows you to use named parameters in the SQL
        //     query string. context.Database.ExecuteSqlCommandAsync("UPDATE dbo.Posts SET Rating
        //     = 5 WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor)){throw new NotImplementedException();}
        //
        // Parameters:
        //   transactionalBehavior:
        //     Controls the creation of a transaction for this command.
        //
        //   sql:
        //     The command string.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        //   parameters:
        //     The parameters to apply to the command string.
        //
        // Returns:
        //     A task that represents the asynchronous operation. The task result contains the
        //     result returned by the database after executing the command.
        //
        // Remarks:
        //     Multiple active operations on the same context instance are not supported. Use
        //     'await' to ensure that any asynchronous operations have completed before calling
        //     another method on this context.
        public Task<int> ExecuteSqlCommandAsync(Core.Cmn.TransactionalBehavior transactionalBehavior, string sql, CancellationToken cancellationToken, params object[] parameters)
        {

            var behavior = GetTransactionalBehavior(transactionalBehavior);
            return _database.ExecuteSqlCommandAsync(behavior, sql, cancellationToken, parameters);
        }
        //
        // Summary:
        //     Checks whether or not the database exists on the server.
        //
        // Returns:
        //     True if the database exists{throw new NotImplementedException();} false otherwise.
        public bool Exists() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Runs the the registered System.Data.Entity.IDatabaseInitializer`1 on this context.
        //     If "force" is set to true, then the initializer is run regardless of whether
        //     or not it has been run before. This can be useful if a database is deleted while
        //     an app is running and needs to be reinitialized. If "force" is set to false,
        //     then the initializer is only run if it has not already been run for this context,
        //     model, and connection in this app domain. This method is typically used when
        //     it is necessary to ensure that the database has been created and seeded before
        //     starting some operation where doing so lazily will cause issues, such as when
        //     the operation is part of a transaction.
        //
        // Parameters:
        //   force:
        //     If set to true the initializer is run even if it has already been run.
        public void Initialize(bool force) { throw new NotImplementedException(); }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : _EntityBase
        {
            foreach (var item in _database.SqlQuery<T>(sql, parameters))
            {
                yield return item;
            }
        }
        //

        public override string ToString() { throw new NotImplementedException(); }
        //
        // Summary:
        //     Enables the user to pass in a database transaction created outside of the System.Data.Entity.Database
        //     object if you want the Entity Framework to execute commands within that external
        //     transaction. Alternatively, pass in null to clear the framework's knowledge of
        //     that transaction.
        //
        // Parameters:
        //   transaction:
        //     the external transaction
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Thrown if the transaction is already completed
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the connection associated with the System.Data.Entity.Database object
        //     is already enlisted in a System.Transactions.TransactionScope transaction
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the connection associated with the System.Data.Entity.Database object
        //     is already participating in a transaction
        //
        //   T:System.InvalidOperationException:
        //     Thrown if the connection associated with the transaction does not match the Entity
        //     Framework's connection
        public void UseTransaction(DbTransaction transaction) { throw new NotImplementedException(); }
    }
}

