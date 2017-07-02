using System.Collections.Generic;
using Core.Entity.Enum;

namespace Core.Service
{
    public static class ConnvertToString
    {
       public static  KeyValuePair<string,string> ConnvertToSearchString(this List<SearchInfoViewModel>   searchInfoViewModel )
       {
           var result = "";
           var value = "";
           int i = 0;
           foreach (var item in searchInfoViewModel)
           {
               if (item.ConditionalOprator == ConditionalOprator.IsEqualTo)
                   result += item.ColumnName + "==" + "@"+i + (SetLogicalOprator(item.LogicalOprator));
               if (item.ConditionalOprator == ConditionalOprator.IsGreaterThan)
                   result += item.ColumnName + ">" + @i + (SetLogicalOprator(item.LogicalOprator));
               if (item.ConditionalOprator == ConditionalOprator.IsLessThan)
                   result += item.ColumnName + "<" + @i + (SetLogicalOprator(item.LogicalOprator));
               if (item.ConditionalOprator == ConditionalOprator.Contains)
                   result += item.ColumnName + ".Contains" + (@i) + (SetLogicalOprator(item.LogicalOprator));
                             
               value += item.Value + ",";
               i++;
           }

          var val= value.Remove(value.Length - 1, 1);
           return new KeyValuePair<string, string>(result,val);
       }

        private static string SetLogicalOprator(LogicalOprator? logicalOprator)
        {
            if (logicalOprator == LogicalOprator.And)
                return "&&";
              if (logicalOprator == LogicalOprator.Or)
                  return "||";
              return string.Empty;

        }
    }
}
