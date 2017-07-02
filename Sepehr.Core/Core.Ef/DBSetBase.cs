using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Cmn;

namespace Core.Ef
{
    public class DBSetBase<T> : IDbSetBase<T> where T : EntityBase<T>
    {
        private DbSet<T> _dbSet;
        public DBSetBase(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public ObservableCollection<T> Local
        {
            get { return _dbSet.Local; }
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public T Attach(T entity)
        {
            return _dbSet.Attach(entity);
        }

        public T Create()
        {
            return _dbSet.Create();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : T, new()
        {
            return _dbSet.Create<TDerivedEntity>();
        }

        public T Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public T Remove(T entity)
        {
            return _dbSet.Remove(entity);
        }
        public IEnumerable<T> RemoveRange(IEnumerable<T> entities)
        {
            return _dbSet.RemoveRange(entities);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return (_dbSet as IQueryable<T>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_dbSet as IQueryable).GetEnumerator();
        }

        public Expression Expression
        {
            get { return (_dbSet as IQueryable).Expression; }
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get { return (_dbSet as IQueryable).Provider; }
        }

        public IQueryable<T> AsNoTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public int Delete(IQueryable<T> source)
        {
            return EntityFramework.Extensions.BatchExtensions.Delete(source);
        }

        public int Update(IQueryable<T> source, Expression<Func<T, T>> updateExpression)
        {
            return EntityFramework.Extensions.BatchExtensions.Update<T>(source, updateExpression);
        }

        public IEnumerable<T> AddRange(IEnumerable<T> source)
        {
            return _dbSet.AddRange(source);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
