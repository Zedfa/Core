using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;


namespace Core.Cmn.Extensions
{
    public static class OrderExtension
    {

        //public static List<T> CreateSortList<T>(IEnumerable<T> dataSource,
        //        string fieldName, SortDirection sortDirection)
        //{

        //    List<T> returnList = new List<T>();
        //    returnList.AddRange(dataSource);
        //    PropertyInfo propInfo = typeof(T).GetProperty(fieldName);
        //    Comparison<T> compare = delegate(T a, T b)
        //    {
        //        bool asc = sortDirection == SortDirection.Ascending;
        //        object valueA = asc ? propInfo.GetValue(a, null) : propInfo.GetValue(b, null);
        //        object valueB = asc ? propInfo.GetValue(b, null) : propInfo.GetValue(a, null);
                
        //        return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
        //    };
        //    returnList.Sort(compare);
        //    return returnList;
        //}
       
    }
}
