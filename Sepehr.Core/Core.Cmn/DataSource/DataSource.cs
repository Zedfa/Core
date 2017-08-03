using Core.Cmn.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.DataSource
{
    public class DataSource : IQueryable
    {
        public Type ElementType
        {
            get
            {
                return Source.ElementType;
            }
        }

        public DataSourceInfo DataSourceInfo { get; set; }

        public IQueryable Source { get; set; }
        public Expression Expression
        {
            get
            {
                return Source.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return Source.Provider;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }
    public class DataSource<T> : DataSource, IQueryable<T>
    {
        public DataSource()
        {

        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)GetEnumerator();
        }
    }
}
