using System.Collections.Generic;
using System.Globalization;
using Core.Mvc.ViewModels;
using System;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Helpers.CustomWrapper.DataModel;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings
{
    [Serializable()]
    public class GridInfo : IViewInfo
    {
        private CultureInfo _cultureInfo;
        public GridInfo()
            : this(null, null)
        { }
        public GridInfo(AccessOperation crudOperation = null, Toolbar gridToolbar = null, string cultureName = "fa-IR")
        {
            _cultureInfo = new CultureInfo(cultureName);
            //ID = Id;

            if (crudOperation == null)
            {
                CRUDOperation = new AccessOperation();
            }
            else
            {
                CRUDOperation = crudOperation;
            }
            //GridToolbar = gridToolbar;

            //if (gridToolbar == null)
            //{
            //    GridToolbar = new Toolbar();

            //}
            //else
            //{
            //    GridToolbar = gridToolbar;
            //    GridToolbar.CRUDOperation = CRUDOperation;
            //    if (GridToolbar != null && GridToolbar.Commands != null && GridToolbar.Commands.Count > 0)
            //    {
            //        GridToolbar.Commands = new Toolbar(GridToolbar.Commands).Commands;
            //    }
            //} 
            GridToolbar = gridToolbar ?? new Toolbar();
            GridToolbar.CRUDOperation = CRUDOperation;

            
        }

        public string Width { get; set; }
        public string Hieght { get; set; }
        public List<Column> ColumnsInfo { get; set; }

        public Type DtoModelType { get; set; }
        public DataSourceInfo DataSource { get; set; }
        public CoreKendoGrid.Features Features { get; set; }
        internal Toolbar GridToolbar { get; set; }

        public string GridID { get; internal set; }

        public CoreKendoGrid.ClientDependentFeature ClientDependentFeatures { get; internal set; }

        internal AccessOperation CRUDOperation { get; set; }

        internal CultureInfo GetCultureInfo()
        {
            return _cultureInfo;
        }
        //private static string _gridID;
        //public static string GridID { get { return _gridID; } set { lock (value) { _gridID = value; } } }
    }
}
