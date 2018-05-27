using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using Core.Service;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{

    ///تعریف ساختار سازمانی در این بخش انجام می شود 
    ///Created on Date :

    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CompanyChartApiController :Core.Mvc.Controller.ApiControllerBase //CrudApiControllerBase<CompanyChart, CompanyChartViewModel, ICompanyChartService>
    {
        private ICompanyChartService _companyChartService;


        public CompanyChartApiController(ICompanyChartService CompanyService)
            //: base(CompanyService)
        {
            _companyChartService = CompanyService;
        }



        ///تعریف ساختار سازمانی در این بخش انجام می شود 
        /// لیست ساختار سازمانی 
    
        public HttpResponseMessage GetEntities()
        {
            var nodes = new List<CompanyChartViewModel>();
            var CompanyCharts = _companyChartService.GetCompanyChart(null).ToList();
            foreach (var CompanyChart in CompanyCharts)
            {
                nodes.Add(new CompanyChartViewModel()
                {
                    Id = CompanyChart.Id,
                    Title = CompanyChart.Title,
                    HasChildren = CompanyChart.ChildCompanyChart.Any(),
                    ParentId = CompanyChart.ParentId,
                });
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, nodes);

        }
        public HttpResponseMessage GetEntities(int Id)
        {
            var nodes = new List<CompanyChartViewModel>();
            var CompanyCharts = _companyChartService.GetCompanyChart(Id).ToList();
            foreach (var CompanyChart in CompanyCharts)
            {
                nodes.Add(new CompanyChartViewModel()
                {
                    Id = CompanyChart.Id,
                    Title = CompanyChart.Title,
                    HasChildren = CompanyChart.ChildCompanyChart.Any(),
                    ParentId = CompanyChart.ParentId,
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, nodes);

            //  var dataSourceResult = nodes.ToDataSourceResult(request);
            // return Request.CreateResponse(HttpStatusCode.OK, dataSourceResult.Data);
        }




        public HttpResponseMessage PostEntitiy(CompanyChartViewModel CompanyChart)
        {

            if (ModelState.IsValid)
            {
              
                var account = _companyChartService.Create(CompanyChart.Model);
                CompanyChart.SetModel(account);
                return Request.CreateResponse(HttpStatusCode.OK, CompanyChart);
            }


            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }


        public HttpResponseMessage PutEntitiy(CompanyChartViewModel CompanyChart)
        {
            if (ModelState.IsValid)
            {
                SetViewModel(CompanyChart);

                var updatedAccount = _companyChartService.Update(CompanyChart.GetModel());

                return Request.CreateResponse(HttpStatusCode.OK, CompanyChart);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage DeleteEntitiy(CompanyChartViewModel CompanyChart)
        {
            if (ModelState.IsValid)
            {
                SetViewModel(CompanyChart);
                var deleteAccount = _companyChartService.Delete(CompanyChart.GetModel());
                return Request.CreateResponse(HttpStatusCode.OK, CompanyChart);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        private void SetViewModel(CompanyChartViewModel viewModel)
        {
            var foundEntity = _companyChartService.Find(viewModel.Id);

            //Id = CompanyChart.Id,
            //        Title = CompanyChart.Title,
            //        HasChildren = CompanyChart.ChildCompanyChart.Any(),
            //        ParentId = CompanyChart.ParentId,

            foundEntity.Id = viewModel.Id;
            foundEntity.Title = viewModel.Title;
            foundEntity.ParentId = viewModel.ParentId;

            viewModel.SetModel(foundEntity);

        }
        
    }
}



