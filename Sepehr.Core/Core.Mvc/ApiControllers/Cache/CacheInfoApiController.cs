using Core.Cmn.Cache;
using Kendo.Mvc.UI;
using System.Net;
using System.Net.Http;

using Core.Mvc.Controller;
using Core.Cmn.FarsiUtils;
namespace Core.Mvc.ApiControllers
{
    public class CacheInfoApiController : ApiControllerBase
    {
        public HttpResponseMessage GetEntities([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK,
                                                   new { Data = CacheConfig.CacheInfoDic.Values, Total = CacheConfig.CacheInfoDic.Count });
            return response;
        }

    }
}
