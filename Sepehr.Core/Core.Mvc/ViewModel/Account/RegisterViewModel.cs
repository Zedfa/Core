using System.ComponentModel.DataAnnotations;
using Core.Mvc.Extensions;
using Core.Mvc.ViewModel;
using Core.Entity;


namespace Core.Mvc.ViewModel.Account
{
    public class RegisterViewModel : ViewModelBase<User>
    {

     
        [Required(ErrorMessage = "نام را وارد کنید")]
        [Display(Name = "نام")]
        public string FName
        {
            get { return Model.FName; }
            set { Model.FName = value; }
        }

        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(50, ErrorMessage = "حداکثر طول رشته 50 حرف می باشد")]

        public string LName
        {
            get { return Model.LName; }
            set { Model.LName = value; }
        }
     
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [StringLength(20, ErrorMessage = "حداکثر طول رشته 20 حرف می باشد")]

        string _userName;
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    if (Model.UserProfile == null) return "";
                    return Model.UserProfile.UserName;
                }
                else
                    return _userName;
            }
            set
            {
                if (Model.UserProfile != null)
                    Model.UserProfile.UserName = value;
                else
                    _userName = value;
            }
        }

   
        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [Display(Name = "رمز عبور")]
        [StringLength(100, ErrorMessage = "حداقل طول رشته 6 حرف می باشد", MinimumLength = 6)]

        [DataType(DataType.Password)]
        string _password;
        public string Password
        {
            get { return ""; }
            set { _password = value; }
        }


        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "رمز جدید با تکرار آن مطابقت ندارد")]
        public string ConfirmPassword { get; set; }

    }
}