using Core.Entity;
using Core.Service;
using System.Collections.Generic;

namespace Core.Mvc.ViewModel
{
    public class TopMenuViewModel
    {
        private MenuItem _dbMenuItems;
     

        public List<MenuItemViewModel> MakeMenuItems()
        {
            var subMenuItems = new List<MenuItemViewModel>();

            var menuItem = SetDbMenuItems();
            foreach (MenuItem menu in menuItem.Childeren)
            {
                subMenuItems.Add(new MenuItemViewModel(menu));
            }

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
              
    }
}