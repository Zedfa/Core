using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class CrudInfo
    {
        public CrudInfo()
        {
        }
        public string Type { get; set; }
        public string DataType { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public string Url { get; set; }
        public Dictionary<string , object> QueryStringItems { get; set; }
        public string ParamsFunction { get; set; }

        public string ReadFilterObject { get; set; }
    }
}
