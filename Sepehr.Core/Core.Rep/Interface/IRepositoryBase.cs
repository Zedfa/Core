using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entity;
using Core.Cmn.EntityBase;
using Core.Cmn;

namespace Core.Repository.Interface
{
    public class CheckRelationBeforeDeleteResult : CoreEntity<CheckRelationBeforeDeleteResult>
    {
        public string inUsedTbName { get; set; }
    }
   public interface IRepositoryBase<T>: IDisposable  where T : class
   {
       List<CheckRelationBeforeDeleteResult> CheckRelationBeforeDelete(string tableName, string keyName, string keyValue);

       IQueryable<T> All(bool canUseCacheIfPossible = true);
      

       IQueryable<T> Filter(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true);

       IQueryable<T> Filter(string expression, bool allowFilterDeleted = true, params object[] value);

       IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

       IEnumerable<T> Filter(ExpressionInfo expressionInfo,out int total, bool allowFilterDeleted = true);

       bool Contains(Expression<Func<T, bool>> predicate);

       T Find(params object[] keys);

       T Find(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true);

       T Create(T t,bool allowSaveChange=true);

        List<T> Create(List<T> objectList, bool allowSaveChange = true);

       int Delete(T t, bool allowSaveChange=true);

       int Delete(Expression<Func<T, bool>> predicate, bool allowSaveChange = true);

       int Delete(IQueryable<T> itemsForDeletion, bool allowSaveChange = true);

       int Delete(List<T> itemsForDeletion, bool allowSaveChange = true);

       int Update(T t, bool allowSaveChange=true);

       int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updatepredicate, bool allowSaveChange = true);

       int Update(IQueryable<T> source,Expression<Func<T, T>> predicate);

       int Count { get; }

       int SaveChanges();


       //IQueryable<IDTO> GetDtoQueryable(Expression<Func<T, bool>> predicate, bool allowFilterDeleted = true);

       IQueryable<IDto> GetDtoQueryable(IQueryable<T> queryable);
       
    
   }
}
