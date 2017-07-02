using Core.Service;
using Core.Mvc.ViewModel.Account;
using Core.Mvc.Infrastructure;

namespace Core.Mvc.ApiControllers
{

    public class ChangeUserPasswordApiController : Core.Mvc.Controller.ApiControllerBase
    {
        //
        private IUserService _userService;
        private IConstantService _constantService;

        
        public ChangeUserPasswordApiController(IUserService userService,IConstantService constantService)
        {
            _userService = userService;

        }

        public void PutEntity(ChangeUserPasswordViewModel changeUserPasswordViewModel)
        {
            var message = new Message();
            var text = string.Empty;

            if (changeUserPasswordViewModel.NewPassword.Equals(changeUserPasswordViewModel.ConfirmPassword))
            {
                var userId = CustomMembershipProvider.GetUserIdCookie();
                if (userId != null)
                {
                    var user = _userService.GetUserAndUserProfileByUserId(userId ?? 0);
                    var password = _userService.GetMd5Hash(changeUserPasswordViewModel.Password);

                    if (password.Equals(user.UserProfile.Password))
                    {

                        var newPassword = _userService.GetMd5Hash(changeUserPasswordViewModel.NewPassword);
                        user.UserProfile.Password = newPassword;
                        var updatedUser = _userService.Update(user);
                        CustomMembershipProvider.SetPassCodeCookie(user.UserProfile.UserName, user.UserProfile.Password);

                        message.type = MessageType.success;
                        _constantService.TryGetValue<string>("ChangePasswordWasSuccessFull", out text);
                        message.text = text/*Core.Resources.Messages.ChangePasswordWasSuccessFull*/;


                    }
                    else
                    {
                        message.type = MessageType.error;
                        _constantService.TryGetValue<string>("IncorrectPassword", out text);
                        message.text = text/*Core.Resources.ExceptionMessage.IncorrectPassword*/;

                    }
                }

            }
            else
            {
                message.type = MessageType.error;
                _constantService.TryGetValue<string>("ConfirmPasswordWasNotMatched", out text);
                message.text = text/*Core.Resources.ExceptionMessage.ConfirmPasswordWasNotMatched*/;

            }

            MessageStrore.Add(message);



        }

    }
}



