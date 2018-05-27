using Core.Cmn.Cache;
using Kendo.Mvc.UI;
using System.Net;
using System.Net.Http;

using Core.Mvc.Controller;
using Core.Cmn.FarsiUtils;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
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
