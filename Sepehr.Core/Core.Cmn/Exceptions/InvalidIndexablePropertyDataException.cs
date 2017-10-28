using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Exceptions
{
    public class InvalidIndexablePropertyDataException : CoreExceptionBase
    {
        public InvalidIndexablePropertyDataException(string className, string propertyName)
        {
            ClassName = className;
            PropertyName = propertyName;
        }
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public override string Message
        {
            get
            {
                return $"For 'IndexablePropertyAttribute', IndexOrder can't be null in {ClassName} Type for property '{PropertyName}'. {System.Environment.NewLine}When you have a GroupName in Indexable Attribute, IndexOrder can't be null for ordering of multiple indexes in grouped indexes";
            }
        }
    }
}
