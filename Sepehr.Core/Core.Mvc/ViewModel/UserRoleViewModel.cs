using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Core.Cmn.Extensions;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.Attribute.Filter;
using Core.Mvc.ViewModel;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using Core.Entity;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "UserRoleViewModel")]
    public class UserRoleViewModel : ViewModelBase<Core.Entity.UserRole>
    {
        public UserRoleViewModel()
            : base()
        {

        }
        public UserRoleViewModel(Core.Entity.UserRole model)
            : base(model)
        {

        }

         [DataMember]
        public string Id
        {
            get
            {
                return UserId.ToString() + RoleId.ToString();
            } 
            
        }



        [DataMember(IsRequired = true)]
         [Required(ErrorMessage = "نقش را وارد کنید")]
         [Display(Name = "نقش")]
        public int RoleId
        {
           
            get { return Model.RoleID; }
            set { Model.RoleID = value; }
        }

       
        [DataMember]
        public int UserId
        {
            get { return Model.UserId; }
            set { Model.UserId = value; }
        }

        [DataMember]
        public string UserName { get; set; }
       
        [DataMember]
        private string _roleName;
        [DataMember]
        [Required(ErrorMessage = "نقش را وارد کنید")]
        [SearchRelatedType(CustomType = SearchRelatedTypes.Navigation, NavigationProperty = "Role.Name")]
        public string RoleName
        {

            get
            {
                if (string.IsNullOrEmpty(_roleName))
                    return Model.Role != null ? Model.Role.Name : null;
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }

        [DataMember(IsRequired = true)]
        public int OldSelectedRoleId
        {
            get; set;
        }

       

        private static GridInfo _viewInfo;

        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/UserRoleApi";
                    dsConfig.ModelCr.ModelIdName = "Id";

                    //------------------------
                    var fConfig = new Features();
                    // fConfig.ReadOnly = true;
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/UserRoleViewModelTemplate.cshtml";
                    fConfig.Selectability = Selectable.Cell;
                    //---------------------
                    List<Column> colsInfo = new List<Column> { 
                                new Column { Title="نقش" , Field="RoleName"  , Encoded=true , Visible=true  }  , 
                                new Column { Field="Id" , Visible=false , Hidden=true} 
            };
                    //GridToolbar = new Toolbar()
                    //{
                    //    Commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandRP.Create , Text = "جدید" } ,
                    //                                         new ColumnCommand { Name = GCommandRP.Delete , Text = "حذف" } ,                         
                    //                                         new ColumnCommand { Name = GCommandRP.Edit , Text = "ویرایش" }}
                    //}

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


        private static GridInfo _roleListViewInfo;

        public static GridInfo RoleListViewInfo
        {
            get
            {
                var dConfig = new Core.Mvc.Helpers.CustomWrapper.DataModel.DataSourceInfo();
                dConfig.CrudCr.Read.Url = "api/Core/UserRoleApi";

                var fCnfig = new Features();
                fCnfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/ListOfRoleViewModelTemplates.cshtml";

                fCnfig.AutoBind = false;
                fCnfig.UserGuideIncluded = false;

                _roleListViewInfo = new GridInfo(fCnfig.CRUDOperation)
               
                {
                    ColumnsInfo = new List<Column> { 
                                new Column { Title="نقش" ,  Field="RoleName"  , Encoded=true , Visible=true , Width="50px" , Sortable = false,Filterable = false},
                                new Column {Title = "شناسه", Field = "Id", Encoded = true, Hidden = true}
            },

                    DataSource = dConfig,
                    Features = fCnfig
                };
                return _roleListViewInfo;
            }
        }

        private static GridInfo _lookupInfo;
        public static GridInfo LookupInfo
        {
            get
            {
                if (_lookupInfo == null)
                {
                    _lookupInfo = ViewInfo.DeepCopy<GridInfo>();
                    _lookupInfo.Features.ReadOnly = true;
                }
                return _lookupInfo;
            }
        }

       

    }


}