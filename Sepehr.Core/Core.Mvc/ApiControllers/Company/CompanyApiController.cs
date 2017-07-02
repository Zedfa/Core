using System.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Core.Mvc.Extensions;
using System.Net.Http;
using Core.Mvc.ViewModel;

using Kendo.Mvc.UI;
using Core.Mvc.Controller;
using Core.Service;
using Core.Entity;

namespace Core.Mvc.ApiControllers
{
    /// <summary>


    public class CompanyApiController : CrudApiControllerBase<Company, CompanyViewModel, IServiceBase<Company>>
    {
        //
        // GET: /Company/
        // private IServiceBase<Model.Company> _company;
        private ICompanyService _companyService;


        public CompanyApiController(ICompanyService company)
            : base(company)
        {
            _companyService = company;
        }



        public override HttpResponseMessage PostEntity([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, CompanyViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.CompanyNationalCode))
            {
                if (!viewModel.CompanyIsValidNationalCode)
                {
                    ModelState.AddModelHandledError("CompanyNationalCode", "کد ملی معتبر راوارد نمائید");
                }
            }
            var isDuplicateName = _companyService.Filter(a => a.Name == viewModel.CompanyName).Any();
            if (isDuplicateName)
            {
                ModelState.AddModelHandledError("DomainName", "نام تکراری می باشد");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            var savedDomainName = _companyService.Create(viewModel.Model);
            viewModel.SetModel(savedDomainName);
            var response = Request.CreateResponse(HttpStatusCode.Created,
                                                                  new { Data = new[] { viewModel }, Total = 1 });
            return response;

        }


        public override HttpResponseMessage PutEntity([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request, CompanyViewModel viewModel)
        {

            if (!string.IsNullOrEmpty(viewModel.CompanyNationalCode))
            {
                if (!viewModel.CompanyIsValidNationalCode)
                    ModelState.AddModelHandledError("CompanyNationalCode", "کد ملی معتبر راوارد نمائید");
            }
            var isDuplicateName = _companyService.Filter(a => a.Name == viewModel.CompanyName).Any();
            if (isDuplicateName)
            {
                ModelState.AddModelHandledError("DomainName", "نام تکراری می باشد");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            var savedDomainName = _companyService.Update(viewModel.Model);
            var response = Request.CreateResponse(HttpStatusCode.Created,
                                                                  new { Data = new[] { viewModel }, Total = 1 });
            return response;

        }

        public override HttpResponseMessage DeleteEntity(CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                var deleted = _companyService.DeleteWithRoles(companyViewModel.Model);
                return Request.CreateResponse(HttpStatusCode.OK, companyViewModel);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
