using System.ComponentModel.DataAnnotations;
using Core.Mvc.ViewModel;
using System.Runtime.Serialization;

using Core.Mvc.Extensions;

namespace Core.Mvc.ViewModel.Account
{
    [DataContract]
    public class ChangePasswordViewModel 
    {
        [DataMember]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "رمز عبور فعلی را وارد کنید")]
        [Display( Name = "رمز عبور فعلی")]
        public string OldPassword { get; set; }
        [DataMember]
        [DataType(DataType.Password)]
        [StrengthChecker]
        [Required(ErrorMessage = "رمز عبور جدید را وارد کنید")]
        [Display(Name = "رمز عبور جدید")]
       
        public string NewPassword
        {
            get;
            set;
        }
        [DataMember]
        [DataType(DataType.Password)]
      
        [Required(ErrorMessage = "رمز عبور جدید را دوباره وارد کنید")]
        [Display(Name = " تکرار رمز عبور جدید")]
       
        [Compare("NewPassword", ErrorMessage = "رمز جدید با تکرار آن مطابقت ندارد")]
        public string ConfirmPassword
        {
            get;
            set;
        }
    }
}