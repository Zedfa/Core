using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Entity;


namespace Core.Mvc.ViewModel
{
    public class CompanyRoleViewModel : ViewModelBase<CompanyRole>
    {

        public CompanyRoleViewModel()
            : base()
        {
        }

        public CompanyRoleViewModel(CompanyRole model)
            : base(model)
        {
            SetModel(model);
        }

        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "لیست شرکت ها را وارد کنید")]
        [Display(Name = "لیست شرکت ها")]
        public int CompanyId
        {
            get { return Model.CompanyId; }
            set { Model.CompanyId = value; }
        }
        [DataMember]
        public int OldCompanyId
        {
            get;
            set;
        }

        //[DataMember]
        private string _roleName;
        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "عنوان نقش را وارد کنید")]
        [Display(Name = "عنوان نقش")]
        public string RoleName
        {
            get
            {
                ////if (string.IsNullOrEmpty(_companyRoletitle))
                ////    return Model.Role != null ? Model.Role.Name : null;
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }




        [DataMember(IsRequired = true)]
        public string CompanyName { get; set; }





        //[DataMember]
        // private int _roleIdInCompanyRole;

        [DataMember]
        public int RoleIdInCompanyRole
        {
            get;
            set;
        }

        [DataMember]
        public int OldRoleId
        {
            get;
            set;
        }

        [DataMember]
        public int RoleId
        {
            get
            {

                return Model.RoleId;
            }
            set
            {
                Model.RoleId = value;
            }
        }

        // private static GridInfo _viewInfo;
        public static GridInfo ViewInfo
        {
            get
            {

                if (_companyListviewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    //dsConfig.CrudCr.Read.Url = "api/Core/CompanyRoleApi";

                    dsConfig.CrudCr.Read.Url = "api/CompanyRoleApi/GetEntities";
                    dsConfig.CrudCr.Insert.Url = "api/CompanyRoleApi/PostEntity";
                    dsConfig.CrudCr.Update.Url = "api/CompanyRoleApi/PutEntity";
                    dsConfig.CrudCr.Remove.Url = "api/CompanyRoleApi/DeleteEntity";


                    dsConfig.ModelCr.ModelIdName = "CompanyId";
                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/CompanyRoleViewModelTemplate.cshtml";
                    fConfig.Selectability = Selectable.Row;
                    fConfig.UserGuideIncluded = false;
                    List<Column> colsInfo = new List<Column>
                        {
                            new Column {Title = "شرکت" , Field = "CompanyId" , Encoded = true , Visible = false , Hidden = true},
                            new Column {Title = " نام شرکت" , Field = "CompanyName" , Encoded = true , Visible = true  , Filterable = false , Sortable = false},
                            new Column {Title = "عنوان نقش شرکتی" , Field = "RoleName" , Encoded = true , Visible = true , Filterable = false , Sortable = false},
                            new Column {Title = "شناسه نقش شرکتی" , Field = "RoleId" , Encoded = true , Visible = false , Hidden = true},
                            new Column {Title = "OldRoleId" , Field = "OldRoleId" , Encoded = true , Visible = false , Hidden = true},
                            new Column {Title = "OldCompanyId" , Field = "OldCompanyId" , Encoded = true , Visible = false , Hidden = true}


                        };
                    _companyListviewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        DataSource = dsConfig,
                        Features = fConfig,
                        ColumnsInfo = colsInfo

                    };
                }
                return _companyListviewInfo;
            }


        }
        // private static GridInfo _viewInfoComapanyRole;

        //public static GridInfo ViewInfoComapanyRole
        //{
        //    get
        //    {
        //        if (_viewInfoComapanyRole == null)
        //        {

        //            var dsConfig = new DataSourceInfo();
        //            dsConfig.CrudCr.Read.Url = "api/HeadUserApi";

        //            /*---------------------------------- */
        //            var fConfig = new Features();
        //            fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/HeadUserViewModelTemplate.cshtml";

        //            /*---------------------------------- */

        //            _viewInfoComapanyRole = new GridInfo(fConfig.CRUDOperation)
        //            {
        //                ColumnsInfo = new List<Column> { 
        //                                new Column { Title="نام" , Field="FName"  , Encoded=true , Visible=true , Width="50px" }, 
        //                                new Column { Title="نام خانوادگی" , Field="LName"  , Encoded=true , Visible=true , Width="50px" }, 
        //                                new Column { Title="نام کاربری" , Field="UserName"  , Encoded=true , Visible=true , Width="50px" },
        //                                new Column { Field="RoleIdForHead" , Visible=false , Hidden=true} ,
        //                      },
        //                DataSource = dsConfig,
        //                Features = fConfig,
        //            };
        //        }
        //        return _viewInfoComapanyRole;
        //    }
        //}

        private static GridInfo _companyListviewInfo;

        public static GridInfo CompanyListViewInfo
        {
            get
            {
                if (_companyListviewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();

                    dsConfig.CrudCr.Read.Url = "api/CompanyApi/GetEntities";
                    dsConfig.CrudCr.Insert.Url = "api/CompanyApi/PostEntity";
                    dsConfig.CrudCr.Update.Url = "api/CompanyApi/PutEntity";
                    dsConfig.CrudCr.Remove.Url = "api/CompanyApi/DeleteEntity";

                    dsConfig.ModelCr.ModelIdName = "CompanyId";
                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/CompanyViewModelTemplate.cshtml";

                    fConfig.Selectability = Selectable.Row;

                    List<Column> colsInfo = new List<Column>
                    {
                      new Column{Title="شناسه" , Field="CompanyId" ,Encoded=true ,Visible=false,Hidden = true},
                    new Column{Title="نام" , Field="Name" ,Encoded=true ,Visible=true,Width = "70px"},
                    new Column{Title="عنوان" , Field="Title" ,Encoded=true ,Visible=true,Width = "70px"},
                    new Column{Title="تلفن" , Field="CompanyPhone" ,Encoded=true ,Visible=true},
                    new Column{Title="آدرس" , Field="CompanyAddress" ,Encoded=true ,Visible=true},
                    new Column{Title="کد" , Field="Code" ,Encoded=true ,Visible=true},

                    new Column{Title="نام خانوادگی" , Field="CompanyFamily" ,Encoded=true ,Visible=true},
                    new Column{Title="نام پدر" , Field="CompanyFatherName" ,Encoded=true ,Visible=true},

                    new Column{Title="کد ملی" , Field="CompanyNationalCode" ,Encoded=true ,Visible=true},

                    };
                    _companyListviewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        DataSource = dsConfig,
                        Features = fConfig,
                        ColumnsInfo = colsInfo
                    };

                }
                return _companyListviewInfo;
            }

        }





    }
}