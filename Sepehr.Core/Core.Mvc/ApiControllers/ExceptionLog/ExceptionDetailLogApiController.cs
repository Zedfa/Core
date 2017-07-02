using Core.Ef;
using Core.Rep.DTO;
using Core.Service;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Mvc.Extensions.FilterRelated;

namespace Core.Mvc.ApiControllers.ExceptionLog
{
    public class ExceptionDetailLogApiController : Core.Mvc.Controller.ApiControllerBase
    {
        public HttpResponseMessage GetAllExceptionDetailLogs([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))]DataSourceRequest request, Guid? ID)
        {

            if (ID.HasValue)
            {
                return GetExceptionLogOfCorrespondentExceptionLog(ID.Value);
            }
            else if (request.Filters != null)
            {
                return GetExceptionLogOfCorrespondentLog(request.Filters.ToList());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "داده ای یافت نشد");
            }
        }


        private HttpResponseMessage GetExceptionLogOfCorrespondentExceptionLog(Guid ID)
        {

            var exceptionLogService = new LogService();

            var exceptionLog = exceptionLogService.GetExceptionLogOfCorrespondentExceptionLog(ID);

            return Request.CreateResponse(HttpStatusCode.OK, exceptionLog);
        }

        private HttpResponseMessage GetExceptionLogOfCorrespondentLog(List<Kendo.Mvc.IFilterDescriptor> filters)
        {
            if (filters.Count > 0)
            {
                var filterVals = filters.GetFilterValues();
                if (filterVals["ID"] != null)
                {
                    var exceptionLogService = new LogService();
                    Guid logId;
                    if (Guid.TryParse(filterVals["ID"].ToString(), out logId))
                    {
                        var exceptionLog = exceptionLogService.GetExceptionLogOfCorrespondentLog(logId);
                        return Request.CreateResponse(HttpStatusCode.OK, exceptionLog);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "داده ای یافت نشد");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "داده ای یافت نشد");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "داده ای یافت نشد");
            }

        }
    }
}
