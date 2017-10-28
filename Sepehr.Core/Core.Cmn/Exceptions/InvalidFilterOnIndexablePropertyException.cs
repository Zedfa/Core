using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Exceptions
{
    public class InvalidFilterOnIndexablePropertyException : CoreExceptionBase
    {
        public InvalidFilterOnIndexablePropertyException(string className, string propertyName)
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
                return $@"You can't use DataSource Filter on properties that is not IndexableProperty. Your Entity name is {ClassName} and your property is {PropertyName}";
            }
        }
    }
}
