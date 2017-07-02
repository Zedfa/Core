using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Extensions
{
    public interface IBatchExtensions
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Delete<T>( IQueryable<T> source) where T : class;
       
        
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        int Delete<T>( IQueryable<T> source, Expression<Func<T, bool>> filterExpression) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
         Task<int> DeleteAsync<T>( IQueryable<T> source) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        Task<int> DeleteAsync<T>( IQueryable<T> source, Expression<Func<T, bool>> filterExpression) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Update<T>( IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class;
        //
        // Summary:
        //     The API was refactored to no longer need  extension method. Use query.Where(expression).Update(updateExpression)
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        int Update<T>( IQueryable<T> source, IQueryable<T> query, Expression<Func<T, T>> updateExpression) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        int Update<T>( IQueryable<T> source, Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        Task<int> UpdateAsync<T>( IQueryable<T> source, Expression<Func<T, T>> updateExpression) where T : class;
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
        //     When executing  method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.

        Task<int> UpdateAsync<T>( IQueryable<T> source, Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) where T : class; 
    }
}
