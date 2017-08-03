using Core.Entity;
using Core.Mvc.Controller;
using Core.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kendo.Mvc.Extensions;
using Core.Cmn;
using Core.Service;
using Core.Mvc.Infrastructure;
using System.Data.SqlClient;
using System.Data;
using Core.Rep.DTO;
namespace Core.Mvc.ApiControllers
{
    public class RoleApiController : Core.Mvc.Controller.CrudDTOApiControllerBase<Role, RoleDTO, IRoleService>
    {
        private IRoleService _roleService;
        public RoleApiController(IRoleService roleService)
            : base(roleService)
        {
            _roleService = roleService;
        }


        ///لیست نقش های سیستم 

        public override DataSourceResult GetEntities([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var roles = _roleService.GetAllRoleDTO();
            var dataSourceResult = roles.ToDataSourceResult(request);
            return dataSourceResult;
        }


        ///ایجاد نقش  
        public override HttpResponseMessage PostEntity(RoleDTO roleDTO)
        {
            var isRoleNameDuplicate = 
                _roleService
                .Filter(a => a.Name == roleDTO.Name)
                .Any();

            if (isRoleNameDuplicate)
            {
                ModelState.AddModelHandledError("Name", "عنوان نقش تکراری می باشد");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            var insertedRole = _roleService.Create(roleDTO.Model);
            roleDTO.SetModel(insertedRole);
            var response = Request.CreateResponse(HttpStatusCode.Created, new { Data = new[] { roleDTO } });
            return response;

        }

        public override HttpResponseMessage PutEntity(RoleDTO roleDTO)
        {
            var role = _roleService.Find(a => a.ID != roleDTO.Id);

            var isRoleNameDuplicate= 
                _roleService
                .Filter(a => a.Name == roleDTO.Name && a.ID != role.ID)
                .Any();

            if (isRoleNameDuplicate)
            {
                ModelState.AddModelHandledError("Name", "عنوان نقش تکراری می باشد");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            base.PutEntity(roleDTO);
            var response = Request.CreateResponse(HttpStatusCode.Created,
                                                         new { Data = new[] { roleDTO } });
            return response;
        }

        public override HttpResponseMessage DeleteEntity(RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {

                var role = _roleService.Find(roleDTO.Id);
                try
                {
                    _roleService.Delete(role);


                }
                catch 
                {
                    //MessageStrore.Add(new Message { text = "dd", type = MessageType.error });
                    //return Request.CreateErrorResponse(HttpStatusCode.NotFound,ex);
                    ModelState.AddModelHandledError("Name", "این رکورد مورد استفاده است");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { roleDTO } });


            }

            return Request.CreateResponse("ValidationError.");


        }

    }
}
