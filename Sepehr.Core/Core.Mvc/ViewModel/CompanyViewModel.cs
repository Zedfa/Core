using Core.Entity;
using Core.Mvc.Extensions;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.ViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "CompanyViewModel")]
    public class CompanyViewModel : ViewModelBase<Company>
    {

        public CompanyViewModel()
            : base()
        {
        }

        public CompanyViewModel(Company model)
            : base(model)
        {
            SetModel(model);
        }



        [DataMember()]
        public int CompanyId
        {
            get { return Model.Id; }
            set { Model.Id = value; }
        }

        [DataMember]
        [Required(ErrorMessage = "نام را وارد کنید")]
        [Display(Name = "نام")]
        public string CompanyName
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        [DataMember]
        [Required(ErrorMessage = "عنوان نمایشی را وارد کنید")]
        [Display(Name = "عنوان نمایشی")]
        public string CompanyTitle
        {
            get { return Model.Title; }
            set { Model.Title = value; }
        }


        [DataMember]
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "تلفن را وارد کنید")]
        [RegularExpression(@"(^\d{8,12})", ErrorMessage = "فرمت را صحیح وارد کنید")]

        public string CompanyPhone
        {
            get { return Model.Phone; }
            set { Model.Phone = value; }
        }

        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "کد را وارد کنید")]
        [Display(Name = "کد")]
        public string CompanyCode
        {
            get { return Model.Code; }
            set { Model.Code = value; }
        }




        [DataMember]
        [Display(Name = "نام پدر")]
        public string CompanyFatherName
        {
            get { return Model.FatherName; }
            set { Model.FatherName = value; }
        }

        [DataMember]
        [Display(Name = "نام خانوادگی")]
        public string CompanyFamily
        {
            get { return Model.Family; }
            set { Model.Family = value; }
        }

        [DataMember]
        [Display(Name = "کدملی")]
        public string CompanyNationalCode
        {
            get { return Model.NationalId; }
            set { Model.NationalId = value; }
        }

        [DataMember]
        public bool CompanyIsValidNationalCode { get; set; }


        [DataMember]
        [Required(ErrorMessage = "نشانی را وارد کنید")]
        [Display(Name = "نشانی")]
        public string CompanyAddress
        {
            get { return Model.Address; }
            set { Model.Address = value; }
        }

        [DataMember]
        [Display(Name = "فعال")]
        public bool CompanyActive
        {
            get
            {

                return Model.Active;
            }

            set
            {
                Model.Active = value;
            }
        }



        private static GridInfo _viewInfo;

        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/CompanyApi";
                    dsConfig.ModelCr.ModelIdName = "CompanyId";
                    var fConfig = new Features();
                    fConfig.EditableConfig.CustomConfig.Template.Url = "~/Areas/Core/Views/Shared/EditorTemplates/CompanyViewModelTemplate.cshtml";


                    fConfig.Selectability = Selectable.Row;
                    fConfig.UserGuideIncluded = false;
                    List<Column> colsInfo = new List<Column>
                        {
                            new Column{Title = "شناسه",Field = "CompanyId",Encoded = true,Visible = false,Hidden = true , Filterable = false , Sortable = false},
                            new Column{Title = "نام",Field = "CompanyName",Encoded = true,Visible = true,Width = "70px"  , Filterable = false , Sortable = false},
                            new Column{Title = "عنوان",Field = "CompanyTitle",Encoded = true,Visible = true,Width = "70px" , Filterable = false , Sortable = false},
                            new Column {Title = "تلفن", Field = "CompanyPhone", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                            new Column {Title = "نشانی", Field = "CompanyAddress", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                            new Column {Title = "کد", Field = "CompanyCode", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                            new Column {Title = "نوع شرکت", Field = "CompanyType", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                            new Column{Title = "نام خانوادگی", Field = "CompanyFamily", Encoded = true, Visible = true  , Filterable = false , Sortable = false},
                            new Column{Title = "نام پدر", Field = "CompanyFatherName", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                            new Column{Title = "کد ملی", Field = "CompanyNationalCode", Encoded = true, Visible = true , Filterable = false , Sortable = false},
                           
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
                    _lookupInfo = ViewInfo.DeepCopy();
                    _lookupInfo.Features.ReadOnly = true;
                    _lookupInfo.ColumnsInfo.Remove(
                        _lookupInfo.ColumnsInfo.FirstOrDefault(column => column.Field == "CompanyPhone"));
                    //_lookupInfo.ColumnsInfo.Remove(
                    //    _lookupInfo.ColumnsInfo.FirstOrDefault(column => column.Field == "CompanyId"));
                    _lookupInfo.ColumnsInfo.Remove(
                        _lookupInfo.ColumnsInfo.FirstOrDefault(column => column.Field == "CompanyAddress"));
                    _lookupInfo.ColumnsInfo.Remove(
                        _lookupInfo.ColumnsInfo.FirstOrDefault(column => column.Field == "CompanyFatherName"));

                    _lookupInfo.ColumnsInfo.Remove(
                        _lookupInfo.ColumnsInfo.FirstOrDefault(column => column.Field == "CompanyNationalCode"));
                }
                return _lookupInfo;
            }
        }

    }
}