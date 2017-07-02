using Core.Entity.Enum;

namespace Core.Service
{
   public class SearchInfoViewModel
    {
       public int Id { get; set; }
       public string ColumnName { get; set; }
       public ConditionalOprator ConditionalOprator { get; set; }
       public object Value { get; set; }
       public LogicalOprator? LogicalOprator { get; set; }

    }
}
