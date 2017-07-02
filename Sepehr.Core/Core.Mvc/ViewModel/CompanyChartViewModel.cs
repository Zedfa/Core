using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using Core.Mvc.ViewModels;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers;
using Core.Entity;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "CompanyChartViewModel")]
    public class CompanyChartViewModel : ViewModelBase<CompanyChart>
    {

        public CompanyChartViewModel()
            : base()
        {

        }

        public CompanyChartViewModel(CompanyChart CompanyChart)
            : base(CompanyChart)
        {

        }



        [DataMember]
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; }

        }


        [DataMember]
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title
        {
            get { return Model.Title; }
            set { Model.Title = value; }
        }


        public string Level
        {
            get;
            set;
        }

        [DataMember]
        public int? ParentId
        {
            get { return Model.ParentId; }
            set { Model.ParentId = value; }

        }


        [DataMember(Name = "hasChildren")]
        public bool HasChildren { get; set; }

        private static IViewInfo _viewInfo;



        public static IViewInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/CompanyChartApi";
                    // dsConfig.ModelRP.ModelIdName = "Id";
                    //dsConfig.ModelRP.ModelType = typeof(CompanyChartViewModel);
                    //------------------------
                    var fConfig = new Features();
                    fConfig.ReadOnly = true;
                    fConfig.Selectability = Selectable.Cell;
                    //---------------------
                    List<Column> colsInfo = new List<Column> { 
                                new Column { Title="جایگاه سازمانی" , Field="Title"  , Encoded=true , Visible=true  }  , 
                                new Column { Field="Id" , Visible=false , Hidden=true} 
                        };
                    _viewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        DataSource = dsConfig,
                        Features = fConfig,
                        ColumnsInfo = colsInfo
                    };

                }

                return _viewInfo;
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
                    _treeInfo.DataSource.ModelCr.ModelIdName = "Id";
                    _treeInfo.DataTextField = "Title";
                    _treeInfo.Operation.Insertable = true;
                    _treeInfo.AutoBind = true;
                    _treeInfo.DataSource.CrudCr.Read.Url = "api/Core/CompanyChartApi"; //"CompanyChart/Read";
                   // _treeInfo.DataSource.DataSourceEvents.Add( Core.Mvc.Helpers.CustomWrapper.DataSource.DataSourceEvent.OnRequestEnd, "req");
                   _treeInfo.TemplateInfo.Width = 300;
                   _treeInfo.TemplateInfo.Height = 100;

        
                }
                return _treeInfo;
            }
        }



       
    }
}