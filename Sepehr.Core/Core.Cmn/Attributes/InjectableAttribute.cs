
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class InjectableAttribute : Attribute
    {
        public Type InterfaceType { get; set; }
        public string DomainName { get; set; }
        public int Version { get; set; }
    }
}
