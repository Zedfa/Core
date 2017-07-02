using Core.Mvc.Helpers.CustomWrapper.DataModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Mvc.Helpers
{
    [DataContract]
    [Serializable]
    public class TreeInfo
    {
        //[DataMember]
         public string Name { get; internal set; }
        // [DataMember]
        public string DataTextField { get; set; }

        private bool _autobind = true;
        public bool AutoBind
        {
            get
            {
                return _autobind;
            }
            set
            {
                _autobind = value;
            }
        }

        private Core.Mvc.Helpers.CustomWrapper.DataSource.AccessOperation _opeation;
        public Core.Mvc.Helpers.CustomWrapper.DataSource.AccessOperation Operation
        {
            get
            {
                if (_opeation == null)
                {
                    _opeation = new Core.Mvc.Helpers.CustomWrapper.DataSource.AccessOperation();
                }
                return _opeation;
            }
            set
            {
                _opeation = value;
            }
        }

        private DataSourceInfo _dataSource;
        public DataSourceInfo DataSource
        {
            get
            {
                if (_dataSource == null)
                    _dataSource = new DataSourceInfo();

                return _dataSource;
            }

            set
            {
                _dataSource = value;
            }
        }

        public string FunctionName { get; set; }

        //[DataMember]
        // public string Url { get; set; }

        //[DataMember]
        //public bool hasChildren { get; set; }

        //[DataMember]
        //public int id { get; set; }

        private Template _templateInfo { get; set; }
        public Template TemplateInfo
        {
            get
            {

                if (_templateInfo == null)
                    _templateInfo = new Template();

                return _templateInfo;
            }

            set
            {
                _templateInfo = value;
            }
        }

    }


}
