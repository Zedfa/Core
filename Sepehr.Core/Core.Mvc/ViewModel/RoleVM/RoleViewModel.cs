using Core.Entity;
using Core.Mvc.Attribute.Validation;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel.RoleVM
{
    [DataContract(Name = "RoleViewModel")]
    public class RoleViewModel : ViewModelBase<Role>
    {

        private static GridInfo _viewInfo;
        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dataSource = new DataSourceInfo();
                    dataSource.CrudCr.Read.Url = "api/core/RoleApi/GetEntities";
                    dataSource.CrudCr.Insert.Url = "api/core/RoleApi/PostEntity";
                    dataSource.CrudCr.Update.Url = "api/core/RoleApi/PutEntity";
                    dataSource.CrudCr.Remove.Url = "api/core/RoleApi/DeleteEntity";
                   // dataSource.ModelCr.ModelIdName = "Id";



                    //------------------------

                    var requiredValidation = new List<ValidationRuleInfo>();
                    requiredValidation.Add(new ValidationRuleInfo("RequiredValidator"));
                    var features = new Features();
                    features.EditableConfig.CustomConfig.Template.Url =
                        "~/Areas/Core/Views/Shared/EditorTemplates/RoleViewModelTemplates.cshtml";
                    features.Selectability = Selectable.Row;
                    features.Insertable = true;

                    //---------------------

                    List<Column> colsInfo = new List<Column> {
                          new Column { Title="نقش" , Field="Name" ,ValidationRules=requiredValidation }  ,
                          new Column { Field="Id" , Visible=false ,Searchable=false, Hidden=true},

                    };


                    _viewInfo = new GridInfo();
                    _viewInfo.DataSource = dataSource;
                    _viewInfo.ColumnsInfo = colsInfo;
                    _viewInfo.Features = features;
                    _viewInfo.DtoModelType = typeof(RoleDTO);

                }
                return _viewInfo;
            }
        }
    }
}