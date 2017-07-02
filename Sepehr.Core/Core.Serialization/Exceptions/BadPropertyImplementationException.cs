using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Exceptions
{
    public class BadPropertyImplementationException : Exception
    {
        public BadPropertyImplementationException(string propertyName, string typeName)
        {
            PropertyName = propertyName;
            TypeName = typeName;
        }
        public string PropertyName { get; private set; }
        public override string Message => $@"setter or getter or both for property '{PropertyName}' in '{TypeName}' type isn't implement correctly. All setter & getter in inherited tpye from SerializableEntity must implement like this sample:
'        public string Name
        {{
            get
            {{
                return GetValue<string>();
            }}
            set
            {{
                SetValue(value);
            }}
        }}'";

        public string TypeName { get; private set; }
    }
}
