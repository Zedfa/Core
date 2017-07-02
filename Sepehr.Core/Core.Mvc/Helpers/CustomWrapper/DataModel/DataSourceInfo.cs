using Core.Mvc.Helpers.CustomWrapper.DataSource;
using System;
using System.Collections.Generic;

namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    [Serializable()]
    public class DataSourceInfo
    {
        public DataSourceInfo()
        {
            ModelCr = new ModelCr();
            CrudCr = new CrudCr();
            ServerRelated = new ServerRelInfo();
            ServerRelated.DSType = DataSourceType.Ajax;
            DataSourceEvents = new Dictionary<DataSourceEvent, object>();
        }
        public IFilterItem InitialFilter { get; set; }
        public ModelCr ModelCr { get; private set; }

        public CrudCr CrudCr { get; private set; }

        public ServerRelInfo ServerRelated { get; private set; }

        public Dictionary<DataSourceEvent, object> DataSourceEvents { get; private set; }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
