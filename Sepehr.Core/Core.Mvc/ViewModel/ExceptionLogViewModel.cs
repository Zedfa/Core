using Core.Entity;
using Core.Mvc.Attribute.Validation;
using Core.Mvc.Helpers;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.ViewModel;
using Core.Mvc.ViewModels;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "ExceptionLogViewModel")]
    public class ExceptionLogViewModel : ViewModelBase<ExceptionLogDTO>
    {
        public ExceptionLogViewModel()
            : base()
        {

        }

        public ExceptionLogViewModel(ExceptionLogDTO excLogDto)
            : base(excLogDto)
        {

        }

        [DataMember]
        public int Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
            }
        }

        [DataMember]
        public string ExceptionType
        
        {
            get
            {
                return Model.ExceptionType;
            }
            set
            {
                Model.ExceptionType = value;
            }
        
        }

        [DataMember]
        public string Message 
        {
            get
            {
                return Model.Message;
            }
            set
            {
                Model.Message = value;
            }
        }

        [DataMember]
        public String StackTrace 
        {
            get
            {
                return Model.StackTrace;
            }
            set
            {
                Model.StackTrace = value;
            }
        }

        [DataMember]
        public string Source 
        {
            get
            {
                return Model.Source;
            }
            set
            {
                Model.Source = value;
            }
        }

        [DataMember(Name = "hasChildren")]
        public bool HasChildren
        {
            get
            {
                return Model.HasChildren;
            }
            set
            {
                Model.HasChildren = value;
            }
        }

        [DataMember]
        public int? ParentId 
        {
            get
            {
                return Model.ParentId;
            }
            set
            {
                Model.ParentId = value;
            }
        }


        private static TreeInfo _treeInfo;
        public static TreeInfo TreeInfo
        {
            get
            {

                if (_treeInfo == null)
                {
                     _treeInfo = new TreeInfo();
                        _treeInfo.DataSource.ModelCr.ModelIdName = "ID";
                        _treeInfo.DataTextField = "Message";
                        _treeInfo.AutoBind = false;
                        _treeInfo.DataSource.CrudCr.Read.Url = "api/ControlPanel/ExceptionDetailLogApi/GetAllExceptionDetailLogs"; 
                        _treeInfo.TemplateInfo.Width = 300;
                        _treeInfo.TemplateInfo.Height = 100;
                }
                return _treeInfo;
            }
        }
            

             
      }
}


