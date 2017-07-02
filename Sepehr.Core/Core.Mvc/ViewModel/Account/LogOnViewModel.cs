
using Core.Entity;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel.Account
{
    [DataContract]
    public class LogOnViewModel : ViewModelBase<UserProfile>
    {
        [DataMember]

        // [Required(ErrorMessageResourceName = "RequiredUserName", ErrorMessageResourceType = typeof(Resource.Resource))]
        //[Display( ResourceType = typeof(Resource.Resource), Name = "UserName")]

        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string UserName
        {
            get
            {
                return Model.UserName;
            }
            set { Model.UserName = value; }
        }
        [DataMember]
        //[Required(ErrorMessageResourceName = "RequiredPassword", ErrorMessageResourceType = typeof(Resource.Resource))]
        //[Display(Name = "Password", ResourceType = typeof(Resource.Resource))]
        [Required(ErrorMessage = "کلمه عبور را وارد کنید")]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return Model.Password; }
            set { Model.Password = value; }
        }

        //[Display(Name = "RememberMe", ResourceType = typeof(Resource.Resource))]
        [Display(Name = "مرا به خاطر بسپار")]
        [DataMember]
        public bool RememberMe { get; set; }

        [DataMember]
        public string HiddenId { get; set; }

        [DataMember]

        //[Required(ErrorMessageResourceName = "CaptchaCodeRequired", ErrorMessageResourceType = typeof(Resource.Resource))]
        [Required(ErrorMessage = "کد امنیتی را وارد کنید")]

        public string CaptchaCode { get; set; }

    }
}