using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class IndexablePropertyAttribute : Attribute
    {
        public IndexablePropertyAttribute()
        {
            IndexOrder = -1;
        }
        public string GroupName { get; set; }
        public int IndexOrder { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class IndexableNavigationPropertyAttribute : Attribute
    {
        public IndexableNavigationPropertyAttribute()
        {
            IndexOrder = -1;
        }
        public string GroupName { get; set; }
        public int IndexOrder { get; set; }
        public string NavigationPath { get; set; }
    }
}
