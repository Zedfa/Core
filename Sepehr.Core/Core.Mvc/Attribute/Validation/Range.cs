using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Mvc.Attribute.Validation
{
   
    public class Range : RangeAttribute
    {

        public Range(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public Range(double minimum, double maximum) : base(minimum, maximum)
        {
        }

        public Range(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
        }
    }

    
}
