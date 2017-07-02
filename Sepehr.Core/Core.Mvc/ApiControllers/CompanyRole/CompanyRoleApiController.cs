using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ModelBinding;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Core.Mvc.ViewModel;
using Core.Service;
using Core.Mvc.Controller;
using Core.Entity;



namespace Core.Mvc.ApiControllers
{
    public class CompanyRoleApiController : CrudApiControllerBase<CompanyRole, CompanyRoleViewModel, ICompanyRoleService>
    {
        private IRoleService _roleService;
        private ICompanyRoleService _companyRoleService;
        private ICompanyService _companyService;
        public CompanyRoleApiController(ICompanyRoleService companyRoleService, ICompanyService companyService, IRoleService roleService)
            : base(companyRoleService)
        {
            _companyRoleService = companyRoleService;
            _companyService = companyService;
            _roleService = roleService;
        }

        public override DataSourceResult GetEntities([ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var companyRoles = _companyRoleService.All_Role_Company();
            List<CompanyRoleViewModel> companyRolesViewModelList = new List<CompanyRoleViewModel>();
            CompanyRoleViewModel companyRole;
            foreach (var item in companyRoles)
            {
                companyRole = new CompanyRoleViewModel();
                companyRole.CompanyId = item.CompanyId;
                companyRole.RoleId = item.RoleId;
                companyRole.OldCompanyId = item.CompanyId;
                companyRole.OldRoleId = item.RoleId;
                companyRole.RoleName = item.Role.Name;
                companyRole.CompanyName = item.Company.Name;
                companyRolesViewModelList.Add(companyRole);
            }

            var dataSourceResult = companyRolesViewModelList.ToDataSourceResult(request);

            //var companyRoleViewModels = CompanyRoleViewModel.GetViewModels<CompanyRoleViewModel>(dataSourceResult.Data.Cast<CompanyRole>());

            ////var companyRoles = companyRoleViewModels as List<CompanyRoleViewModel> ?? companyRoleViewModels.ToList();
            //foreach (var companyRole in companyRoles)
            //{
            //    //var foundedCompany = _companyService.Find(companyRole.CompanyId);
            //    companyRole.CompanyName = companyRole.company.Name;
            //    //IdRole
            //    var foundedRole = _roleService.Find(companyRole.RoleId);

            //    companyRole.CompanyTitle = foundedRole.Name;
            //    companyRole.OldRoleId = companyRole.RoleId;
            //    companyRole.OldCompanyId = companyRole.CompanyId;
            //}


            //dataSourceResult.Data = companyRoles;
            dataSourceResult.Data = companyRolesViewModelList;
            return dataSourceResult;
        }


        public override System.Net.Http.HttpResponseMessage PostEntity([ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, CompanyRoleViewModel companyRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var model = companyRoleViewModel.GetModel();
                var companyRoleById = _companyRoleService.Find(model.CompanyId, model.RoleId);
                if (companyRoleById != null)
                {
                    ModelState.AddModelError("CompanyTitle", "نقش انتخاب شده قبلا برای این شرکت ثبت شده است");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);

                }

                var companyRole = _companyRoleService.Create(model);
                companyRoleViewModel.SetModel(companyRole);
                return Request.CreateResponse(HttpStatusCode.Created, new { Data = new[] { companyRoleViewModel }, Total = 1 });
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public override HttpResponseMessage PutEntity([ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, CompanyRoleViewModel companyRoleViewModel)
        {

            if (ModelState.IsValid)
            {
                var model = companyRoleViewModel.GetModel();

                var companyRoleById = _companyRoleService.Find(model.CompanyId, model.RoleId);
                var oldCompanyRoleById = _companyRoleService.Find(companyRoleViewModel.OldCompanyId, companyRoleViewModel.OldRoleId);


                if (companyRoleById != null)
                {
                    ModelState.AddModelError("CompanyTitle", "نقش انتخاب شده قبلا برای این شرکت ثبت شده است");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);

                }
                _companyRoleService.Delete(oldCompanyRoleById);
                _companyRoleService.Create(model);


                return Request.CreateResponse(HttpStatusCode.OK, new { Data = new[] { companyRoleViewModel }, Total = 1 });
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public override HttpResponseMessage DeleteEntity(CompanyRoleViewModel companyRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedCompanyRole = _companyRoleService.Find(companyRoleViewModel.Model.CompanyId, companyRoleViewModel.Model.RoleId);
                var deleted = _companyRoleService.Delete(selectedCompanyRole);
                return Request.CreateResponse(HttpStatusCode.OK, companyRoleViewModel);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}