using Core.Entity;
using Core.Service;
using System.Collections.Generic;

namespace Core.Mvc.ViewModel
{
    public class TopMenuViewModel
    {
        private MenuItem _dbMenuItems;
       // private IUserProfileService _userProfileService;

        public List<MenuItemViewModel> MakeMenuItems()
        {
            var subMenuItems = new List<MenuItemViewModel>();
            var _userProfileService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();

            var menuItem = SetDbMenuItems();
            foreach (MenuItem menu in menuItem.Childeren)
            {
                subMenuItems.Add(new MenuItemViewModel(menu));
            }
            var ve = new ViewElement();
            //ve.UniqueName = "#/Utilities";
            //ve.Title = "امکانات";
            //ve.XMLViewData = "Areas/Core/Content/images/tools.png";
            //var utilMenu = new MenuItem
            //{
            //    ViewElement = ve,
            //    Childeren = new List<MenuItem> 
            //{ 
            //  new MenuItem { ViewElement = new ViewElement { UniqueName = "#/CloseAll", Title = "بستن تمام تب ها" } },
            //  new MenuItem() { ViewElement = new ViewElement { UniqueName = "#/AboutUs", Title = "درباره ما" } } }
            //};
            //subMenuItems.Add(new MenuItemViewModel(utilMenu));

            return subMenuItems;

        }

        private static string GetImageUrl(string menuItemName)
        {
            //TODO: Return the Image Url of the specified menu item(based on menu item title)
            return menuItemName;
        }

        private MenuItem SetDbMenuItems()
        {
            var viewElementService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IViewElementService>();
            int userId = Core.Mvc.Infrastructure.CustomMembershipProvider.GetUserIdCookie() ?? 0;
            if (userId > 0)
            {
                return _dbMenuItems = viewElementService.AccessibleViewElements(userId);
            }
            else
                return null;

        }

        //[Dependency]
        //private IViewElementService ViewElementService { get; set; }
    }
}