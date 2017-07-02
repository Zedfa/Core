
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.Features;
using System.Globalization;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Helpers.CustomWrapper.CoreKendoGrid.Settings.Features;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    [Serializable()]
    public class Features : JsonObjectBase
    {
        private CultureInfo _cultureInfo;

        public Features()
        {
            Scrollability = new Scrollable();
            Selectability = new Selectable();
            Sortability = new Sortable();
            EditableConfig = new EditConfig();
            Filter = new FilterConfig(_cultureInfo);
            Filter.FilterAffection = true;
            PagingInfo = new Pageability(_cultureInfo);
            Selectability = Selectable.Row;
            ReadOnly = false;
            Updatable = true;
            Removable = true;
            Insertable = true;
            Refreshable = true;
            Searchable = true;
            AutoBind = true;
            UserGuideIncluded = true;
            MakeCRUDOperationDefinition();
            Navigatable = true;
            Paging = true;
            //PageSize = 20;
            Grouping = false;
            Reorderable = true;
            Resizable = true;
            //FileOutput = new FileOutput();

            ColumnMenu = new ColumnMenu(_cultureInfo);
            GroupingInfo = new Grouping(_cultureInfo);
        }

        public AccessOperation CRUDOperation { get; set; }

        private void MakeCRUDOperationDefinition()
        {
            CRUDOperation = new AccessOperation();
            CRUDOperation.ReadOnly = ReadOnly;
            CRUDOperation.Insertable = Insertable;
            CRUDOperation.Updatable = Updatable;
            CRUDOperation.Removable = Removable;
            CRUDOperation.Refreshable = Refreshable;
            CRUDOperation.Search = Searchable;
        }

        public Scrollable Scrollability { get; private set; }
        public Selectable Selectability { get; set; }
        public Sortable Sortability { get; private set; }
        public EditConfig EditableConfig { get; private set; }
        public FilterConfig Filter { get; private set; }
        public Pageability PagingInfo { get; private set; }
        public Grouping GroupingInfo { get; private set; }
        public FileOutput FileOutput { get; set; }
        public string PreDeletionCallback { get; set; }
        public bool Navigatable { get; set; }
        public bool Paging { get; set; }
        public int? PageSize { get; set; }
        public bool Grouping { get; set; }
        public bool ReadOnly { get; set; }
        public bool Updatable { get; set; }
        public bool Removable { get; set; }
        public bool Insertable { get; set; }
        public bool Reorderable { get; set; }
        public bool Refreshable { get; set; }
        public bool Searchable { get; set; }
        public bool UserGuideIncluded { get; set; }
        public bool Resizable { get; set; }
        public bool AutoBind { get; set; }
        public ColumnMenu ColumnMenu { get; set; }
        public bool Aggregates { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {

        }
        internal void SetCulture(CultureInfo culture)
        {
            _cultureInfo = culture;
        }




    }
}
