using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Reflection
{
    public  class ReflectionHelpers
    {
        public static string GetFullyQualifiedName(Type type)
        {
            return type.AssemblyQualifiedName.ToString();
        }

        public static Type GetType(string fullName)
        {
            var typ = Type.GetType(fullName) ??
                              AppDomain.CurrentDomain.GetAssemblies()
                                       .Select(a => a.GetType(fullName))
                                       .FirstOrDefault(t => t != null);
            return typ;
        }
    }
}
