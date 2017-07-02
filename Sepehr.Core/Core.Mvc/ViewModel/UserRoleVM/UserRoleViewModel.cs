using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Rep.DTO.UserRoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.ViewModel.UserRoleVM
{
    public class UserRoleViewModel
    {
        private static GridInfo _viewInfo;
        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/UserRoleApi/GetEntities";
                    dsConfig.CrudCr.Insert.Url = "api/UserRoleApi/PostEntity";
                    dsConfig.CrudCr.Remove.Url = "api/UserRoleApi/DeleteEntity";
                    dsConfig.ModelCr.ModelIdName = "RoleId";



                    //------------------------


                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/UserRoleViewModelTemplate.cshtml";

                    // fConfig.Paging = false;
                    fConfig.AutoBind = false;
                    fConfig.Selectability = Selectable.Row;
                    fConfig.Insertable = true;
                    fConfig.Updatable = false;


                    //---------------------

                    var ColumnsInfo = new List<Column> { 
                                        new Column { Field="RoleId",Searchable=false  ,Hidden=true,  Visible=false , Width="50px" }, 
                                        new Column { Title="عنوان نقش" , Field="RoleName" ,Searchable=true , Visible=true , Width="50px" }, 

                              };


                    _viewInfo = new GridInfo();
                    _viewInfo.DataSource = dsConfig;
                    _viewInfo.ColumnsInfo = ColumnsInfo;
                    _viewInfo.Features = fConfig;
                    _viewInfo.DtoModelType = typeof(UserRoleDTO);
                    _viewInfo.GridToolbar.Commands.Find(cmd => cmd.Name == GCommandCr.Create).Text = "اختصاص نقش";
                    _viewInfo.GridToolbar.Commands.Find(cmd => cmd.Name == GCommandCr.Edit).Text = "ویرایش نقش";
                    _viewInfo.GridToolbar.Commands.Find(cmd => cmd.Name == GCommandCr.Delete).Text = "حذف نقش";
                }
                return _viewInfo;
            }
        }
    }
}