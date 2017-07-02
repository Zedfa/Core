
///chegini 1391/04/24
namespace Core.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
   
    public class EntityInfo
    {
       public EntityInfo()
        {
            Properties = new Dictionary<string, PropertyInfo>();
            KeyColumns = new Dictionary<string, PropertyInfo>();
        }
        public Dictionary<string, PropertyInfo> Properties { get; internal set; }
        public Dictionary<string, PropertyInfo> KeyColumns { get; internal set; }
    }
}
