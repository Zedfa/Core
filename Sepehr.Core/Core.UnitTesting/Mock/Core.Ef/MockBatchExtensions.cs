using Core.Cmn.Attributes;
using Core.Cmn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitTesting.Mock.Core.Ef
{
    [Injectable(InterfaceType = typeof(IBatchExtensions), Version = 1000)]
    public class MockBatchExtensions : IBatchExtensions
    {
        //
        // Summary:
        //     Executes a delete statement using the query to filter the rows to be deleted.
        //
        // Parameters:
        //   source:
        //     The IQueryable`1 used to generate the where clause for the delete statement.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row deleted.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        public int Delete<T>(IQueryable<T> source) where T : class
        {
            return 0;
        }

        //
        // Summary:
        //     Executes a delete statement using an expression to filter the rows to be deleted.
        //
        // Parameters:
        //   source:
        //     The source used to determine the table to delete from.
        //
        //   filterExpression:
        //     The filter expression used to generate the where clause for the delete statement.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row deleted.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        [Obsolete("The API was refactored to no longer need this extension method. Use query.Where(expression).Delete() syntax instead.")]
        public int Delete<T>(IQueryable<T> source, Expression<Func<T, bool>> filterExpression) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes a delete statement asynchronously using the query to filter the rows
        //     to be deleted.
        //
        // Parameters:
        //   source:
        //     The IQueryable`1 used to generate the where clause for the delete statement.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row deleted.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        public Task<int> DeleteAsync<T>(IQueryable<T> source) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes a delete statement asynchronously using an expression to filter the
        //     rows to be deleted.
        //
        // Parameters:
        //   source:
        //     The source used to determine the table to delete from.
        //
        //   filterExpression:
        //     The filter expression used to generate the where clause for the delete statement.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row deleted.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        [Obsolete("The API was refactored to no longer need this extension method. Use query.Where(expression).DeleteAsync() syntax instead.")]
        public Task<int> DeleteAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> filterExpression) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes an update statement using the query to filter the rows to be updated.
        //
        // Parameters:
        //   source:
        //     The query used to generate the where clause.
        //
        //   updateExpression:
        //     The System.Linq.Expressions.MemberInitExpression used to indicate what is updated.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row updated.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        public int Update<T>(IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class
        {
            return 0;
        }
        //Update<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class;
        //public int Update(IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class
        //{
        //    return EntityFramework.Extensions.BatchExtensions.Update(source, updateExpression);
        //}
        //
        // Summary:
        //     The API was refactored to no longer need this extension method. Use query.Where(expression).Update(updateExpression)
        //     syntax instead.
        //
        // Parameters:
        //   source:
        //     The source used to determine the table to update.
        //
        //   query:
        //     The query used to generate the where clause.
        //
        //   updateExpression:
        //     The MemberInitExpression used to indicate what is updated.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row updated.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        public int Update<T>(IQueryable<T> source, IQueryable<T> query, Expression<Func<T, T>> updateExpression) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes an update statement using an expression to filter the rows that are
        //     updated.
        //
        // Parameters:
        //   source:
        //     The source used to determine the table to update.
        //
        //   filterExpression:
        //     The filter expression used to generate the where clause.
        //
        //   updateExpression:
        //     The System.Linq.Expressions.MemberInitExpression used to indicate what is updated.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row updated.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        public int Update<T>(IQueryable<T> source, Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes an update statement asynchronously using the query to filter the rows
        //     to be updated.
        //
        // Parameters:
        //   source:
        //     The query used to generate the where clause.
        //
        //   updateExpression:
        //     The System.Linq.Expressions.MemberInitExpression used to indicate what is updated.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row updated.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        public Task<int> UpdateAsync<T>(IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class { throw new NotImplementedException(); }
        //
        // Summary:
        //     Executes an update statement asynchronously using an expression to filter the
        //     rows that are updated.
        //
        // Parameters:
        //   source:
        //     The source used to determine the table to update.
        //
        //   filterExpression:
        //     The filter expression used to generate the where clause.
        //
        //   updateExpression:
        //     The System.Linq.Expressions.MemberInitExpression used to indicate what is updated.
        //
        // Type parameters:
        //   T:
        //     The type of the entity.
        //
        // Returns:
        //     The number of row updated.
        //
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        [Obsolete("The API was refactored to no longer need this extension method. Use query.Where(expression).UpdateAsync(updateExpression) syntax instead.")]
        public Task<int> UpdateAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) where T : class { throw new NotImplementedException(); }
    }
}
