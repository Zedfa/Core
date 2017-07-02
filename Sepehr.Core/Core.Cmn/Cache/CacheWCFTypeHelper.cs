using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Cache
{
    public static class CacheWCFTypeHelper
    {
        public static List<Type> typeList { get; set; }

        static CacheWCFTypeHelper()
        {
            typeList = new List<Type>();
            typeList.Add(typeof(ICacheDataProvider));
          //  typeList.Add(typeof(IQueryable));
            
        }
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {

            //  Assembly assembly = Assembly.GetAssembly(typeof(MyBaseClass));
            //   List<Type> types = assembly.GetExportedTypes().Where(e => e.BaseType != null && e.BaseType.BaseType == typeof(MyBaseClass)).ToList();
            
           // var lst = new List<Type>();
            // lst.Add(typeof(object));
            // lst.Add(typeof(ICacheBase));
            // lst.Add(typeof(baseClass));
            // lst.Add(typeof(ComplexClass));
            //  lst.Add(typeof(ReturnStruct<IQueryable<ComplexClass>>));
            
            return typeList;

        }

    }
}
