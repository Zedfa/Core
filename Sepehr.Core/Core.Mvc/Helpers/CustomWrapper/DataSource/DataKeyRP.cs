using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    [Serializable()]
    public class DataKeyCr : IDataKey
    {
        public DataKeyCr(string IdName)
        {
            Name = IdName;
        }
        public object GetValue(object dataItem)
        {
            throw new NotImplementedException();
           
        }

        public string Name
        {
            get;
            private set;
        }

        public string RouteKey
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
