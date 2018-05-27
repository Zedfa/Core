using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kendo.Mvc.UI;
using Core.Service;
using Core.Ef;
using Kendo.Mvc.Extensions;
using System.Web.Mvc;
using Core.Mvc.Extensions.FilterRelated;
using Core.Rep.DTO;
using Core.Mvc.ViewModel;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExceptionLogApiController : Core.Mvc.Controller.ApiControllerBase
    {

        //private ICompanyChartService _companyChartService;


        //public ExceptionLogApiController(ICompanyChartService CompanyService)
        //    //: base(CompanyService)
        //{
        //    _companyChartService = CompanyService;
        //}


        public HttpResponseMessage GetAllLogs([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))]DataSourceRequest request)
        {
            var exceptionLogService = new LogService();
            var qdtos = exceptionLogService.GetAllLogDTOs();
             DataSourceResult result = qdtos.ToDataSourceResult(request,(dto) => EventLogViewModel.GetViewModel<EventLogViewModel>(dto));
            return Request.CreateResponse<DataSourceResult>(HttpStatusCode.OK, result);
        }


    }
}

 
