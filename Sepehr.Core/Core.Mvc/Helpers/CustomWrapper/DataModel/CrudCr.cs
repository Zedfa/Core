using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mvc.Helpers.CustomWrapper.DataModel;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class CrudCr
    {
        public CrudInfo Read { get; private set; }
        public CrudInfoEdit Update { get; private set; }
        public CrudInfoEdit Remove { get; private set; }
        public CrudInfo Insert { get; private set; }
        public CrudCr()
        {
            Read = new CrudInfo();
            Update = new CrudInfoEdit();
            Remove = new CrudInfoEdit();
            Insert = new CrudInfo();
        }
    }
}
