using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Mvc.Controller;
using Core.Mvc.ViewModel;
using System.Web;
using Core.Mvc.Infrastructure;
using Core.Service;
using Core.Rep.DTO;
using System.Web.Http.Description;

namespace Core.Mvc.ApiControllers
{
    // IgnoreApi baraye inke felan toye swagger nayad ta badan in controller ro barresi konim o age niaz bod toye document swagger biad
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ViewElementApiController : ApiControllerBase
    {
        private IViewElementService _viewElementService;
        public ViewElementApiController(IViewElementService viewElementService)
        {
            _viewElementService = viewElementService;
        }
        public List<MenuItemViewModel> GetMenuItems()
        {
           // if (HttpContext.Current.User.Identity.IsAuthenticated)
            //int? userId = CustomMembershipProvider.GetUserIdCookie();
            //bool isPassCodeValidate = CustomMembershipProvider.ValidatePassCode(CustomMembershipProvider.GetPassCodeCookie());
            //if (isPassCodeValidate && userId.HasValue)
            if (CustomMembershipProvider.IsCurrentUserAuthenticate())
            {
                var topMenuViewmodel = new TopMenuViewModel();

                return topMenuViewmodel.MakeMenuItems();
            }
            return null;
        }
        public List<ViewElementDTO> GetAccessibleViewElements()
        {
             return _viewElementService.GetAccessibleViewElements(CustomMembershipProvider.GetUserIdCookie().Value);
        }
    }
}
