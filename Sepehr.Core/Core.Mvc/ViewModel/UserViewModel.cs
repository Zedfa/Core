using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Core.Mvc.Attribute.Validation;
using Core.Mvc.Extensions;
using System.Collections.Generic;
using Core.Mvc.ViewModels;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Entity;


namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "UserViewModel")]
    public class UserViewModel : ViewModelBase<User>
    {

        public UserViewModel()
            : base()
        {

        }

        public UserViewModel(User user)
            : base(user)
        {
        }

        [Required(ErrorMessage = "نقش را انتخاب نمائید")]
        [DataMember(IsRequired = true)]
        public int SelectedRoleId { get; set; }

        [DataMember]
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; }

        }



        [DataMember]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام را وارد نمائید")]
        public string FName
        {
            get { return Model.FName; }

            set { Model.FName = value; }
        }
        [DataMember]
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "نام خانوادگی را وارد نمائید")]
        public string LName
        {
            get { return Model.LName; }
            set { Model.LName = value; }

        }
        [DataMember]
        [Display(Name = "نام کامل")]
        public string UserFullName
        {
            get { return Model.FName + " " + Model.LName; }

        }

        [DataMember]
        private string _userName;
        [DataMember]
        [Display(Name = " نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string UserName
        {

            get
            {
                if (string.IsNullOrEmpty(_userName))
                    return Model.UserProfile != null ? Model.UserProfile.UserName : null;
                return _userName;
            }
            set
            {

                _userName = value;
            }
        }

        [DataMember]
        private string _password;
        [DataMember]
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [StrengthChecker]
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _password;
            }
            set
            {
                _password = value;
            }
        }


        [DataMember]
        private string _comparePassword;
        [DataMember]
        [Display(Name = "رمز عبور")]

        public string ComparePassword
        {
            get
            {
                if (string.IsNullOrEmpty(_comparePassword))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _comparePassword;
            }
            set
            {
                _comparePassword = value;
            }
        }



        [DataMember]
        private string _confirmPassword;
        [DataMember]
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "کلمه عبور را وراد نمائید")]
        public string ConfirmPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_confirmPassword))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
            }
        }

        [DataMember]
        [Display(Name = "سازمان")]
        public int? CompanyChartId
        {
            get { return Model.CompanyChartId; }

            set
            {

                Model.CompanyChartId = value;



            }
        }




        [DataMember]
        private string _companyName;
        [DataMember]
        [Display(Name = "نام سازمان")]
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(_companyName))
                    return Model.CompanyChart != null ? Model.CompanyChart.Title : null;
                return _companyName;
            }
            set
            {
                _companyName = value;
            }
        }





        [DataMember]
        [Display(Name = "نام سازمان")]
        public string CompanyOfHeadUser { get; set; }




        //[DataMember]
        //private int _CompanyIdOfHeadUser { get; set; }
        //[DataMember(IsRequired = true)]
        //public int CompanyIdOfHeadUser
        //{
        //    get
        //    {
        //        if (_CompanyIdOfHeadUser == 0)
        //        {
        //            var i = Model.CurrentCompanyId != 0 ? Model.CurrentCompanyId : 0;
        //            if (i != null)
        //                return (int)i;
        //        }
        //        return _CompanyIdOfHeadUser;
        //    }
        //    set
        //    {
        //        _CompanyIdOfHeadUser = value;
        //    }
        //}




        //[DataMember]
        //public string CompanyName { get; set; }



        [DataMember]
        [Display(Name = "فعال")]
        public bool Active
        {
            get
            {
                //if (Model.Id == 0)
                //    return true;
                return Model.Active;
            }

            set
            {
                Model.Active = value;
            }
        }


        [DataMember]
        [Display(Name = "وضعیت")]
        public string ActiveStatus
        {
            get { return Active == false ? "غیر فعال" : "فعال"; }

        }

        [DataMember]
        private string _roleChoosInHeadUser;

        [DataMember]
        [Display(Name = "نقش")]

        public string RoleChoosInHeadUser
        {
            get
            {
                if (string.IsNullOrEmpty(_roleChoosInHeadUser))
                {
                    return (Model.UserRoles != null) ? ((Model.UserRoles.Count > 0) ? (Model.UserRoles.Where(x => x.UserId == Model.Id).LastOrDefault().Role.Name) : "") : "";
                }
                return _roleChoosInHeadUser;
            }
            set
            {
                _roleChoosInHeadUser = value;
            }
        }


        [DataMember]
        public int _roleIdForHead;

        [DataMember]
        [Display(Name = "نقش")]

        public int RoleIdForHead
        {
            get
            {
                if (Model.UserRoles != null)
                {
                    //tdo:felan farz bar in hast ke user admin sazman faghat 1 role darad...
                    return (Model.UserRoles != null) ? ((Model.UserRoles.Count > 0) ? (Model.UserRoles.Where(x => x.UserId == Model.Id).Last().RoleID) : 0) : 0;
                }
                return _roleIdForHead;
            }
            set
            {
                _roleIdForHead = value;
            }
        }


        //[DataMember]
        //public string UserDomainName { get; set; }
        //[DataMember]
        //public string UserHeadDomainName { get; set; }


        private static GridInfo _viewInfo;

        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {

                    var dsConfig = new DataSourceInfo();
                    //dsConfig.CrudCr.Read.Url = "api/Core/UserApi";

                    dsConfig.CrudCr.Read.Url = "api/UserApi/GetEntities";
                    dsConfig.CrudCr.Insert.Url = "api/UserApi/PostEntity";
                    dsConfig.CrudCr.Update.Url = "api/UserApi/PutEntity";
                    dsConfig.CrudCr.Remove.Url = "api/UserApi/DeleteEntity";

                    /*---------------------------------- */
                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/UserViewModelTemplate.cshtml";
                    // fConfig.EditableConfig.CustomConfig.EditWindow.Height = 380;
                    /*---------------------------------- */

                    _viewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        ColumnsInfo = new List<Column> {
                                         new Column { Title="نام" , Field="FName"  ,  Visible=true , Width="50px" },
                                        new Column { Title="نام خانوادگی" , Field="LName"  , Visible=true , Width="50px" },
                                        new Column { Title="نام کاربری" , Field="UserName"  ,  Visible=true , Width="50px" , Sortable = false , Filterable = false}

                              },


                        DataSource = dsConfig,
                        Features = fConfig,
                    };



                }
                return _viewInfo;
            }
            set
            {
                _viewInfo = value;
            }
        }


        // private static GridInfo _viewInfoHeadUser;

        //public static GridInfo ViewInfoHeadUser
        //{
        //    get
        //    {
        //        if (_viewInfoHeadUser == null)
        //        {

        //            var dsConfig = new DataSourceInfo();
        //            dsConfig.CrudCr.Read.Url = "api/Core/HeadUserApi";

        //            /*---------------------------------- */
        //            var fConfig = new Features();
        //            fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/HeadUserViewModelTemplate.cshtml";
        //            // fConfig.EditableConfig.CustomConfig.EditWindow.Height = 380;
        //            /*---------------------------------- */
        //            fConfig.UserGuideIncluded = false;
        //            _viewInfoHeadUser = new GridInfo(fConfig.CRUDOperation)
        //            {
        //                ColumnsInfo = new List<Column> { 
        //                                new Column { Title="نام" , Field="FName"  , Encoded=true , Visible=true , Width="70px" }, 
        //                                new Column { Title="نام خانوادگی" , Field="LName"  , Encoded=true , Visible=true , Width="70px" }, 
        //                                new Column { Title="نام کاربری" , Field="UserName"  , Encoded=true , Visible=true , Width="70px" , Filterable = false , Sortable = false},
        //                                new Column { Field="RoleIdForHead" , Visible=false , Hidden=true} ,
        //                                 new Column {Title="سازمان", Field="CompanyOfHeadUser" , Visible=true, Width="100px" , Filterable = false , Sortable = false} ,
        //                      },
        //                DataSource = dsConfig,
        //                Features = fConfig,
        //            };
        //        }
        //        return _viewInfoHeadUser;
        //    }
        //}

        private static GridInfo _lookupAllUser;
        public static GridInfo LookupAllUser
        {
            get
            {
                if (_lookupAllUser == null)
                {
                    _lookupAllUser = ViewInfo.DeepCopy<GridInfo>();
                    _lookupAllUser.Features.ReadOnly = true;
                    _lookupAllUser.DataSource.CrudCr.Read.Url = "api/SuppliersUsersApi/GetEntities";

                }
                return _lookupAllUser;
            }
        }

    }
}



