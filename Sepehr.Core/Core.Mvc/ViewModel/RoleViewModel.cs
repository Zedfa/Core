using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using System.Collections.Generic;
using Core.Cmn.Extensions;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Entity;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "RoleViewModel")]
    public class RoleViewModel : ViewModelBase<Role>
    {
        public RoleViewModel()
            : base()
        {

        }
        public RoleViewModel(Role model)
            : base(model)
        {

        }
        [DataMember]
        public int Id
        {
            get { return Model.ID; }
            set { Model.ID = value; }

        }
        [DataMember]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessage = "نقش را وارد کنید")]
        [Display(Name = "نقش")]
        public string Name
        {
            get { return Model.Name; }

            set { Model.Name = value; }
        }


        [DataMember(IsRequired = true)]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessage = "نقش را وارد کنید")]
        [Display(Name = "نقش")]

        public int SelectedRoleId { get; set; }


        [DataMember(IsRequired = true)]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required(ErrorMessage = "نقش را وارد کنید")]
        [Display(Name = "نقش")]
        public int SelectedRoleName { get; set; }


        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }
         [DataMember]
        public int SelectedUserId { get; set; }

         [DataMember]
        public bool HasAccess { get; set; }
         [DataMember]
        public int SelectedCompanyChartId { get; set; }

         private static GridInfo _viewInfo;

         public static GridInfo ViewInfo
         {
             get
             {
                 if (_viewInfo == null)
                 {
                     var dsConfig = new DataSourceInfo();
                     dsConfig.CrudCr.Read.Url = "api/Core/RoleApi";
                     dsConfig.ModelCr.ModelIdName = "Id";

                     //------------------------
                     var fConfig = new Features();
                     // fConfig.ReadOnly = true;
                     fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/RoleViewModelTemplates.cshtml";
                     fConfig.Selectability = Selectable.Cell;


                     //---------------------
                     List<Column> colsInfo = new List<Column> { 
                                new Column { Title="نقش" , Field="Name"  , Encoded=true , Visible=true  }  , 
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


         private static GridInfo _lookupInfoWithCompanyRole;
         public static GridInfo LookupInfoWithCompanyRole
         {
             get
             {
                 if (_lookupInfoWithCompanyRole == null)
                 {
                     _lookupInfoWithCompanyRole = ViewInfo.DeepCopy<GridInfo>();
                     _lookupInfoWithCompanyRole.Features.ReadOnly = true;
                     _lookupInfoWithCompanyRole.DataSource.CrudCr.Read.Url = "api/Core/RoleWithCompanyRoleApi";
                 }
                 return _lookupInfoWithCompanyRole;
             }
         }
    }


}

