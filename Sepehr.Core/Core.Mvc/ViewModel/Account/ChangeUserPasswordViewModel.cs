using System.Runtime.Serialization;
using Core.Entity;



namespace Core.Mvc.ViewModel.Account
{
    [DataContract(Name = "ChangeUserPasswordViewModel")]
    public class ChangeUserPasswordViewModel : ViewModelBase<User>
    {

        public ChangeUserPasswordViewModel()
            : base()
        {

        }

        public ChangeUserPasswordViewModel(User user)
            : base(user)
        {
        }

        private string _userName;
        [DataMember]
        public string UserName
        {

            get
            {
                if (string.IsNullOrEmpty(_userName))
                    return Model.UserProfile != null ? Model.UserProfile.UserName : null;
                return _userName;
            }
            set
            {

                _userName = value;
            }
        }

        private string _password;

        [DataMember]
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                    return Model.UserProfile != null ? Model.UserProfile.Password : null;
                return _password;
            }
            set
            {
                _password = value;
            }
        }


        [DataMember]
        public string NewPassword
        {
            get;
            set;
        }

        [DataMember]
        public string ConfirmPassword
        {
            get;
            set;
        }

    }
}



