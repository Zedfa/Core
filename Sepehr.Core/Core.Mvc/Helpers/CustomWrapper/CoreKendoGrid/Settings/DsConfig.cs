using System;
using Core.Mvc.Helpers.CustomWrapper.DataModel;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings
{
    [Serializable()]
    public class DsConfig
    {
        public DsConfig()
        {
            ModelCr = new ModelCr();
            //SchemaCr = schemaCr;
            CrudCr = new CrudCr();
            ServerRelated = new ServerRelInfo();
        }
        public ModelCr ModelCr { get; private set; }
        //public SchemaCr SchemaCr { get; private set; }
        public CrudCr CrudCr { get; private set; }
        public ServerRelInfo ServerRelated { get; private set; }
    } 
}
