using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Core.Mvc.ViewModel;
using Core.Entity;


namespace Core.Mvc.ViewModel
{
    public class ViewElementRoleViewModel : ViewModelBase<ViewElement>
    {

        public ViewElementRoleViewModel()
            : base()
        {

        }
        public ViewElementRoleViewModel(ViewElement model)
            : base(model)
        {

        }


        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; }

        }

        public string Name
        {
            get { return Model.Title; }

            set { Model.Title = value; }
        }


        public int SelectedRoleIdViewElement { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool HasAccess { get; set; }

        public int SelectedCompanyChartId { get; set; }
        [DataMember(IsRequired = true)]
        //[Display(Name = "Role", ResourceType = typeof(Titles))]
        //[Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string SelectedRoleNameViewElement { get; set; }

        public string[] ChekedItem { get; set; }
    }
}