using System.Collections.Generic;

namespace Core.Entity
{
    public class ExpressionInfo
    {
        public KeyValuePair<string, string>? Expression { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
