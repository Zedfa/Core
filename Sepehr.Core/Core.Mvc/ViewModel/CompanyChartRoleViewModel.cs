using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Core.Mvc.ViewModel;
using Core.Entity;


namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "CompanyRoleViewModel")]
    public class CompanyChartRoleViewModel : ViewModelBase<Role>
    {

        public CompanyChartRoleViewModel()
            : base()
        {

        }
        public CompanyChartRoleViewModel(Role model)
            : base(model)
        {

        }


        public int Id
        {
            get { return Model.ID; }
            set { Model.ID = value; }

        }
        //[DataMember]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string Name
        {
            get { return Model.Name; }

            set { Model.Name = value; }
        }


        //[DataMember(IsRequired = true)]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        public int SelectedRoleId { get; set; }
       // [DataMember]
        public int UserId { get; set; }

       // [DataMember]
        public string UserName { get; set; }

        public bool HasAccess { get; set; }

        public int SelectedCompanyChartId { get; set; }
    }
}