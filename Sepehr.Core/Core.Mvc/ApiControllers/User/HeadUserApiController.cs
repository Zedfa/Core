using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using Core.Ef;
using Core.Mvc.Extensions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using Core.Mvc.ViewModel;

using Core.Entity;
using Core.Service;
using Core.Mvc.Controller;


namespace Core.Mvc.ApiControllers
{
    public class HeadUserApiController : CrudApiControllerBase<User, UserViewModel, IUserService>
    {
        private IUserService _userService;
        private ICompanyService _companyService;
        private ICompanyRoleService _companyRoleService;

        public HeadUserApiController(IUserService userService, ICompanyService companyService, ICompanyRoleService companyRoleService)
            : base(userService)
        {
            _userService = userService;
            _companyService = companyService;
            _companyRoleService = companyRoleService;
        }

        public override DataSourceResult GetEntities(
            [System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var users = _userService.GetAllHeadUsers();
            DataSourceResult dataSourceResult = null;
            if (users != null)
            {
                dataSourceResult = users.ToDataSourceResult(request);
                var userViewModels = UserViewModel.GetViewModels<UserViewModel>(dataSourceResult.Data.Cast<User>());
                var viewModels = userViewModels as List<UserViewModel> ?? userViewModels.ToList();

                foreach (var userViewModel in viewModels)
                {
                    var roleid = userViewModel.RoleIdForHead;
                    var foundedCompanyRole =
                        _companyRoleService.Filter(a => a.RoleId == roleid).FirstOrDefault();
                    if (foundedCompanyRole != null)
                    {
                        var foundedCompany = _companyService.Find(foundedCompanyRole.CompanyId);
                        userViewModel.CompanyOfHeadUser = foundedCompany.Name;
                    }

                }
                dataSourceResult.Data = viewModels;
            }
            else
            {
                users = Enumerable.Empty<User>().AsQueryable();
                dataSourceResult = users.ToDataSourceResult(request);

            }

            return dataSourceResult;
        }


        public override HttpResponseMessage PostEntity(
            [System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, UserViewModel userViewModel)
        {

            var isDuplicateUserName = _userService.IsDuplicateUserName(userViewModel.UserName, userViewModel.Id);
            if (isDuplicateUserName)
            {
                ModelState.AddModelHandledError("UserName", "نام کاربری تکراری می باشد");
            }

            if (userViewModel.Password != userViewModel.ConfirmPassword)
            {
                ModelState.AddModelHandledError("ConfirmPassword", "کلمه عبور صحیح نمی باشد");
            }

            if (ModelState.IsValid)
            {

                var roleid = userViewModel.RoleIdForHead;
                var foundedCompanyRole = _companyRoleService.Filter(a => a.RoleId == roleid).FirstOrDefault();
                var foundedCompany = _companyService.Find(foundedCompanyRole.CompanyId);
                var user = userViewModel.GetModel();
                //چون کاربر ادمین هر سازمان توسط ما تعریف می شود پس باید سازمان آن را هم مشخص کنیم
                user.CurrentCompanyId = foundedCompany.Id;
                user.UserProfile = new UserProfile
                {
                    Password = _userService.GetMd5Hash(userViewModel.Password),
                    UserName = userViewModel.UserName,
                   // CurrentCompanyId = foundedCompany.Id
                };

                user.UserRoles = AddRoleToUser(userViewModel, foundedCompany.Id);

                var addedUser = _userService.Create(user);
                userViewModel.SetModel(addedUser);

                return Request.CreateResponse(HttpStatusCode.Created,
                    new { Data = new[] { userViewModel }, Total = 1 });
            }


            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }



        public override HttpResponseMessage PutEntity(
    [System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, UserViewModel userViewModel)
        {

            var isDuplicateUserName = _userService.IsDuplicateUserName(userViewModel.UserName, userViewModel.Id);
            if (isDuplicateUserName)
            {
                ModelState.AddModelHandledError("UserName", "نام کاربری تکراری می باشد");
            }

            if (userViewModel.Password != userViewModel.ConfirmPassword)
            {
                ModelState.AddModelHandledError("ConfirmPassword", "کلمه عبور صحیح نمی باشد");

            }

            if (ModelState.IsValid)
            {

                var roleid = userViewModel.RoleIdForHead;
                var foundedCompanyRole = _companyRoleService.Filter(a => a.RoleId == roleid).FirstOrDefault();
                var foundedCompany = _companyService.Find(foundedCompanyRole.CompanyId);

                var user = userViewModel.GetModel();
                //چون کاربر ادمین هر سازمان توسط ما تعریف می شود پس باید سازمان آن را هم مشخص کنیم
                user.CurrentCompanyId = foundedCompany.Id;
                user.UserProfile = new UserProfile
                {

                    Password = userViewModel.ComparePassword != userViewModel.Password ? _userService.GetMd5Hash(userViewModel.Password) : userViewModel.Password,
                    UserName = userViewModel.UserName,
                    //CurrentCompanyId = foundedCompany.Id

                };

                user.UserRoles = AddRoleToUser(userViewModel, foundedCompany.Id).ToList();
               // var updatedUser = _userService.UpdateHeaderUser(user, foundedCompany.Id);
                return Request.CreateResponse(HttpStatusCode.OK,
                      new { Data = new[] { userViewModel }, Total = 1 });


            }


            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);

        }

        public override HttpResponseMessage DeleteEntity(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var deleted = _userService.DeleteWithRoles(userViewModel.Model);
                return Request.CreateResponse(HttpStatusCode.OK, userViewModel);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }



        private List<UserRole> AddRoleToUser(UserViewModel userViewModel,int currentCompanyId)
        {
            var headUserRole = new List<UserRole>();


            if (userViewModel.RoleIdForHead > 0)
            {
                headUserRole.Add(new UserRole()
                {
                    UserId = userViewModel.Id,
                    RoleID = userViewModel.RoleIdForHead,
                    //CurrentCompanyId = currentCompanyId
                });
            }

            return headUserRole;
        }
    }
}
