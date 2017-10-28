using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    public class InfoForFillingNavigationProperty
    {
        public Type ParentEntityType { get; set; }
        public Type NavigationPropertyType { get; set; }
        public string ThisEntityRefrencePropertyName { get; set; }
        public string OtherEntityRefrencePropertyName { get; set; }
        public string CacheName { get; set; }
        public System.Reflection.PropertyInfo PropertyInfo { get; set; }

        private bool? _isEnumerable;

        public bool IsEnumerable
        {
            get
            {
                if (_isEnumerable == null)
                    _isEnumerable = (typeof(IEnumerable).IsAssignableFrom(PropertyInfo.PropertyType));
                return _isEnumerable.Value;
            }
        }

        internal bool IsInitForFillingNavigationProperty { get; set; }

        public string SecondLevelDataSourceName { get; set; }
    }
}
