

using System.Linq;
using System.Net;
using System.Net.Http;
using Core.Mvc.Extensions;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using Core.Entity;

using Core.Rep.DTO;
using System;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserApiController : Core.Mvc.Controller.CrudDTOApiControllerBase<User, UserDTO, IUserService>
    {

        private IUserService _userService;

        public UserApiController(IUserService userService)
            : base(userService)
        {
            _userService = userService;
        }

        public override DataSourceResult GetEntities(
            [System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {


            var users = _userService.GetAllUsersExceptedAdmin();
            var dataSourceResult = users.ToDataSourceResult(request);
            return dataSourceResult;
        }

        public override HttpResponseMessage PostEntity(UserDTO userDTO)
        {

            var isDuplicateUserName = _userService.IsDuplicateUserName(userDTO.UserName, userDTO.Id);
            if (isDuplicateUserName)
            {
                ModelState.AddModelHandledError("UserName", "نام کاربری تکراری می باشد");
            }

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                ModelState.AddModelHandledError("ConfirmPassword", "کلمه عبور صحیح نمی باشد");

            }

            if (ModelState.IsValid)
            {


                var user = userDTO.Model;
                // user.CurrentCompanyId = _userService.AppBase.CompanyId;

                user.UserProfile = new UserProfile
                {
                    Password = _userService.GetMd5Hash(userDTO.Password),
                    UserName = userDTO.UserName,
                    //    CurrentCompanyId = _userService.AppBase.CompanyId
                };
                var addedUser = _userService.Create(user);
                userDTO.SetModel(addedUser);
                //userDTO.UserName = userDTO.UserName;

                return Request.CreateResponse(HttpStatusCode.Created,
                    new { Data = new[] { userDTO } });
            }


            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }



        public override HttpResponseMessage PutEntity(UserDTO userDTO)
        {

            var isDuplicateUserName = _userService.IsDuplicateUserName(userDTO.UserName, userDTO.Id);
            if (isDuplicateUserName)
            {
                ModelState.AddModelHandledError("UserName", "نام کاربری تکراری می باشد");
            }

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                ModelState.AddModelHandledError("ConfirmPassword", "کلمه عبور صحیح نمی باشد");

            }

            if (ModelState.IsValid)
            {

                var user = userDTO.GetModel();

                user.UserProfile = new UserProfile
                {

                    Password = userDTO.ComparePassword != userDTO.Password ? _userService.GetMd5Hash(userDTO.Password) : userDTO.Password,
                    UserName = userDTO.UserName
                };


                var updatedUserId = _userService.Update(user);


                return Request.CreateResponse(HttpStatusCode.OK,
                new { Data = new[] { userDTO }, Total = 1 });


            }


            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        public override HttpResponseMessage DeleteEntity(UserDTO userDTO)
        {



            if (ModelState.IsValid)
            {

                var user = _userService.All(false).First(x => x.Id == userDTO.Id);
                try
                {
                    var insertedUser = _userService.Delete(user);
                    //_userService.Delete(user);
                }

                catch 
                {
                    return Request.CreateResponse("این رکورد مورد استفاده است.");
                }

                //return Request.CreateResponse(HttpStatusCode.OK, roleDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { userDTO } });


            }

            return Request.CreateResponse("ValidationError.");



        }

    }
}



