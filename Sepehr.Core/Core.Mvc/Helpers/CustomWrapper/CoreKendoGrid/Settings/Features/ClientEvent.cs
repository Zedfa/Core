using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class ClientEvent
    {
        //public ClientEvent()
        //{
        //    OnEdit = new ClientEventOnEdit();
        //}
        public string OnDataBound { get; set; }
        public string OnDataBinding { get; set; }
        public string OnEdit { get; set; }
        public string OnSave { get; set; }
        public string OnCancel { get; set; }
        public string OnChange { get; set; }
        public string OnDelete { get; set; }
        public string OnCreate { get; set; }
        /// <summary>
        /// This Property Is Set in Conditions where the read method need to be called by query string 
        /// </summary>
        public string OnLazyReadScript { get; set; }
        public string OnGettingPostUrl { get; set; }
        public string OnGettingPutUrl { get; set; }
        public string OnGettingDeleteUrl { get; set; }
        public string OnDoubleClick { get; set; }
        public string OnKeyDown { get; set; }
        public string OnInit { get; set; }
    }
}
