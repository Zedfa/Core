using System.ComponentModel.DataAnnotations;
using Core.Mvc.Extensions;
using Core.Mvc.ViewModel;
using System.Runtime.Serialization;
using Core.Entity;


namespace Core.Mvc.ViewModel
{
    public class CompanyViewElementViewModel : ViewModelBase<CompanyViewElement>
    {

          public CompanyViewElementViewModel()
            : base()
        {

        }
          public CompanyViewElementViewModel(CompanyViewElement model)
              : base(model)
          {

          }


          public int CompanyIdCompanyViewElement
          {
              get { return Model.CompanyId; }
              set { Model.CompanyId = value; }

          }

          public int ViewElementIdCompanyViewElement
          {
              get { return Model.ViewElementId; }

              set { Model.ViewElementId = value; }
          }


        

          public int UserId { get; set; }

          public string UserName { get; set; }

          public bool HasAccess { get; set; }

          public int SelectedCompanyIdViewElement { get; set; }
          [DataMember(IsRequired = true)]
          [Display(Name = "نام شرکت")]
          [Required(ErrorMessage = "نقش را وارد کنید")]
          public string SelectedCompanyNameViewElement { get; set; }

          public string[] ChekedItem { get; set; }


    }
}