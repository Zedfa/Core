using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Attributes
{
    public class FillNavigationProperyByCacheAttribute : Attribute
    {
        public string ThisEntityRefrencePropertyName { get; set; }
        public string OtherEntityRefrencePropertyName { get; set; }
        public string CacheName { get; set; }
        public string SecondLevelDataSourceName { get; set; }
    }
}
