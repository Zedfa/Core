
using System.Net;
using System.Net.Http;
using Core.Mvc.Extensions;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Entity;
using Core.Rep.DTO.UserRoleDTO;
using System;



namespace Core.Mvc.ApiControllers
{
    public class UserRoleApiController : Controller.CrudDTOApiControllerBase<UserRole, UserRoleDTO, IUserRoleService>
    {

        private IUserRoleService _userRoleService;


        public UserRoleApiController(IUserRoleService userRoleService)
            : base(userRoleService)
        {
            _userRoleService = userRoleService;
        }


        public override DataSourceResult GetEntities(
           [System.Web.Http.ModelBinding.ModelBinder(typeof(ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)//int UserID
        {
            return _userRoleService.GetAllUserRolesDto().ToDataSourceResult(request);
        }

        private string ConvertToFilterQuery(Kendo.Mvc.FilterOperator filterOperator)
        {
            return "==";
        }





        public override HttpResponseMessage PostEntity(UserRoleDTO userRoleDTO)
        {
            var hasDuplicateRole =
                _userRoleService
                .HasDuplicateRoleAssigne(userRoleDTO.RoleId, userRoleDTO.UserId);

            if (hasDuplicateRole)
            {
                ModelState.AddModelHandledError("RoleId", "نقش انتخاب شده تکراری است");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }


            var model = userRoleDTO.GetModel();
            var userRole = _userRoleService.Create(model);
            userRoleDTO.SetModel(userRole);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created,
                                                                   new { Data = new[] { userRoleDTO } });
            return response;
        }



        public override HttpResponseMessage DeleteEntity(UserRoleDTO userRoleDTO)
        {
            if (ModelState.IsValid)
            {


                try
                {
                    var selectedUserRole = userRoleDTO.Model;
                    var userRole = _userRoleService.Find(selectedUserRole.UserId, selectedUserRole.RoleID);
                    var deleteUserRole = _userRoleService.Delete(userRole);


                }
                catch (Exception ex)
                {
                    return Request.CreateResponse("این رکورد مورد استفاده است.");
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { userRoleDTO } });


            }


            return Request.CreateResponse("ValidationError.");



            //if (ModelState.IsValid)
            //{
            //    var selectedUserRole = userRoleDTO.Model;
            //    var userRole = _userRoleService.Find(selectedUserRole.UserId, selectedUserRole.RoleID);
            //    var deleteUserRole = _userRoleService.Delete(userRole);
            //    return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { userRoleDTO } });


            //}
            //return Request.CreateResponse(HttpStatusCode.NotFound);
        }


    }
}
