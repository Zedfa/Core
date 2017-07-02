using System.ComponentModel.DataAnnotations;

namespace Core.Mvc.Attribute.Validation
{
  
    public class StringLength : StringLengthAttribute
    {
        public StringLength(int maximumLength) : base(maximumLength)
        {
        }
    }
}
