using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Entity;
using Core.Rep.DTO;
using Core.Mvc.Attribute.Validation;

namespace Core.Mvc.ViewModel.UserVM
{
    public class UserViewModel : ViewModelBase<User>
    {
        private static GridInfo _viewInfo;
        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dataSource = new DataSourceInfo();
                    dataSource.CrudCr.Read.Url = "api/Core/UserApi/GetEntities";
                    dataSource.CrudCr.Insert.Url = "api/Core/UserApi/PostEntity";
                    dataSource.CrudCr.Update.Url = "api/Core/UserApi/PutEntity";
                    dataSource.CrudCr.Remove.Url = "api/Core/UserApi/DeleteEntity";
                    dataSource.ModelCr.ModelIdName = "Id";



                    //------------------------


                    var features = new Features();
                    features.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/UserViewModelTemplate.cshtml";
                    var requiredValidation = new List<ValidationRuleInfo>();
                    requiredValidation.Add(new ValidationRuleInfo("RequiredValidator"));
                    // fConfig.Paging = false;
                    features.Selectability = Selectable.Row;
                    features.Insertable = true;

                    //---------------------

                    var ColumnsInfo = new List<Column> {
                                        new Column { Title="نام" , Field="FName"  ,  Visible=true , Width="50px",ValidationRules=requiredValidation },
                                        new Column { Title="نام خانوادگی" , Field="LName"  , Visible=true , Width="50px",ValidationRules=requiredValidation },
                                        new Column { Title="نام کاربری" , Field="UserName"  ,  Visible=true , Width="50px" ,ValidationRules=requiredValidation, Sortable = false , Filterable = false},
                                        new Column { Title="کلمه عبور" , Field="Password"  ,  Visible=false , Width="50px",ValidationRules=requiredValidation , Sortable = false , Filterable = false}


                              };


                    _viewInfo = new GridInfo();
                    _viewInfo.DataSource = dataSource;
                    _viewInfo.ColumnsInfo = ColumnsInfo;
                    _viewInfo.Features = features;
                    _viewInfo.DtoModelType = typeof(UserDTO);

                }
                return _viewInfo;
            }
            set
            {
                _viewInfo = value;
            }
        }
    }
}