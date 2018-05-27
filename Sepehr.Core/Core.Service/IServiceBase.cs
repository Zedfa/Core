using System.Data;
using System.Linq.Expressions;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cmn;
using System.Globalization;

namespace Core.Service
{
    public interface IServiceBase<T> where T : ObjectBase , new()
    {
        IDbContextBase ContextBase { get; }

        CultureInfo CurrentCulture { get; set; }

        IQueryable<T> All(bool canUseCache = true); //== All method in repository

        T Create(T entity, bool allowSaveChange = true);
        List<T> Create(List<T> objectList, bool allowSaveChange = true);
        int Delete(T entity, bool allowSaveChange = true);
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Delete(Expression<Func<T, bool>> predicate);
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Delete(IQueryable<T> itemsForDeletion);
        int Delete(List<T> itemsForDeletion, bool allowSaveChange = true);
        int Update(T entity, bool allowSaveChange = true);
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Update(IQueryable<T> source, Expression<Func<T, T>> predicate);
        // Remarks:
        //     When executing this method, the statement is immediately executed on the database
        //     provider and is not part of the change tracking system. Also, changes will not
        //     be reflected on any entities that have already been materialized in the current
        //     context.
        int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updatepredicate);

        int Count { get; }
        T Find(params object[] keys);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true);
        IQueryable<T> Filter(string expression, bool allowFilterDeleted = true, params object[] value);
        // IQueryable<T> Filter(ExpressionInfo expressionInfo, out int total, bool allowFilterDeleted = true);

        // IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);
        // bool Contains(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true);
        AppBase AppBase { get; }

    }
}


