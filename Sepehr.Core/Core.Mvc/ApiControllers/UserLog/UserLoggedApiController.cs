using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Entity;
using Core.Service;
using Core.Mvc.Controller;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserLoggedApiController : CrudApiControllerBase<CoreUserLog, UserLoggedViewModel, IServiceBase<CoreUserLog>>
    {
        private IServiceBase<CoreUserLog> _userLogService;

        public UserLoggedApiController(IServiceBase<CoreUserLog> userLogService)
            : base(userLogService)
        {
            _userLogService = userLogService;            
        }

        public override DataSourceResult GetEntities([System.Web.Http.ModelBinding.ModelBinder(typeof(Core.Mvc.ModelBinders.DataSourceRequestModelBinder))] DataSourceRequest request)
        {
            var userlog = _userLogService.All();
            var dataSourceResult = userlog.ToDataSourceResult(request);
            dataSourceResult.Data = UserLoggedViewModel.GetViewModels<UserLoggedViewModel>(dataSourceResult.Data.Cast<CoreUserLog>());
            return dataSourceResult;
        }
    }
}
