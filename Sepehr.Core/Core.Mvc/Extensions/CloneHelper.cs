using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.Mvc.Extensions
{
    public static class CloneHelper
    {
        public static T DeepCopy<T>(this T obj) where T : class , new()
        {
            object clonedObj = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                clonedObj = formatter.Deserialize(ms);
            }
            return (T)clonedObj;
        }
    }
}
