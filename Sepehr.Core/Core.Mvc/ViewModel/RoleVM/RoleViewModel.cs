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
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/RoleApi/GetEntities";
                    dsConfig.CrudCr.Insert.Url = "api/RoleApi/PostEntity";
                    dsConfig.CrudCr.Update.Url = "api/RoleApi/PutEntity";
                    dsConfig.CrudCr.Remove.Url = "api/RoleApi/DeleteEntity";
                    dsConfig.ModelCr.ModelIdName = "Id";



                    //------------------------

                    var requiredValidation = new List<ValidationRuleInfo>();
                    requiredValidation.Add(new ValidationRuleInfo("RequiredValidator"));
                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url =
                        "~/Areas/Core/Views/Shared/EditorTemplates/RoleViewModelTemplates.cshtml";
                    fConfig.Selectability = Selectable.Row;
                    fConfig.Insertable = true;


                    //---------------------

                    List<Column> colsInfo = new List<Column> {
                          new Column { Title="نقش" , Field="Name" ,ValidationRules=requiredValidation }  ,
                          new Column { Field="Id" , Visible=false ,Searchable=false, Hidden=true},

                    };


                    _viewInfo = new GridInfo();
                    _viewInfo.DataSource = dsConfig;
                    _viewInfo.ColumnsInfo = colsInfo;
                    _viewInfo.Features = fConfig;
                    _viewInfo.DtoModelType = typeof(RoleDTO);

                }
                return _viewInfo;
            }
        }
    }
}