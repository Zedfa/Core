using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class ModelCr
    {
        public Type ModelType { get; set; }
        public string ModelIdName { get; set; }
    }
}
