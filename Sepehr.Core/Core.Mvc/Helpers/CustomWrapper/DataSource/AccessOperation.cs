using System;
using System.Collections.Generic;


namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    [Serializable()]
    public class AccessOperation
    {
        public AccessOperation()
        {
            ReadOnly = false;
            Insertable = true;
            Updatable = true;
            Removable = true;
            Refreshable = true;
            HasCustomAction = false;
            UserGuideIncluded = true;
            Search = true;
            InsertableOrUpdatable = true;
        }
        public bool ReadOnly { get; set; }
        public bool Insertable { get; set; }
        public bool Updatable { get; set; }
        public bool Removable { get; set; }
        public bool Refreshable { get; set; }
        public bool HasCustomAction {get;set;}
        public bool Search { get; set; }
        public bool UserGuideIncluded { get; set; }
        public bool InsertableOrUpdatable { get; set; }
       
    }
}
