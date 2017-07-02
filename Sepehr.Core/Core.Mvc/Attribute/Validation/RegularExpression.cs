using System.ComponentModel.DataAnnotations;

namespace Core.Mvc.Attribute.Validation
{
   
   public class RegularExpression : RegularExpressionAttribute
    {
       public RegularExpression(string pattern) : base(pattern)
       {
       }
    }
}
