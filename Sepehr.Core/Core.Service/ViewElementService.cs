using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Entity.Enum;
using Core.Service.Models;
using Core.Rep.DTO;
using Core.Cmn.Attributes;



namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IViewElementService), DomainName = "core")]
    public class ViewElementService : ServiceBase<ViewElement>, IViewElementService
    {
        private IUserService UserService { get; set; }

        private IUserProfileService UserProfileService { get; set; }

        private ICompanyChartRoleService CompanyChartRoleService { get; set; }


        private IViewElementRoleService ViewElementRoleService { get; set; }


        public ViewElementService(IDbContextBase dbContextBase, IUserService userService,
            IUserProfileService userProfileService, ICompanyChartRoleService companyChartRoleService, IViewElementRoleService viewElementRoleService)
            : base(dbContextBase)
        {

            _repositoryBase = new ViewElementRepository(ContextBase);

            UserService = userService;
            UserProfileService = userProfileService;
            CompanyChartRoleService = companyChartRoleService;
            ViewElementRoleService = viewElementRoleService;

        }
        public ViewElementService(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {

        }
        public ViewElement GetViewElementByUniqueName(string uniqueName)
        {
            return (_repositoryBase as ViewElementRepository).GetViewElementByUniqueName(uniqueName);
        }
        public IQueryable<ViewElement> GetRootViewElements()
        {

            return (_repositoryBase as ViewElementRepository).GetRootViewElements();
        }
        public IQueryable<ViewElement> GetChildViewElementByParentId(int? parentId)
        {
            return (_repositoryBase as ViewElementRepository).GetChildViewElementByParentId(parentId);
        }
        public ViewElement GetViewElementAndViewElementChildByID(int viewElemntId)
        {
            return (_repositoryBase as ViewElementRepository).GetViewElementAndViewElementChildByID(viewElemntId);
        }
        public IList<ViewElement> GetViewElement(int? id)
        {


            return (_repositoryBase as ViewElementRepository).GetViewElement(id).ToList();
        }
        public ViewElement GetViewElementAndChildsById(int id)
        {
            return (_repositoryBase as ViewElementRepository).GetViewElementAndChildsById(id);
        }
        //public IList<ViewElement> GetAllViewElement(int? id)
        //{

        //    return (_repositoryBase as ViewElementRepository).GetChildViewElementByParentId(id).ToList();
        //}


        //if (appBase.ViewElementsGrantedToAnonymousUser == null)
        //    {
        //        var userProfile = userProfileService.Filter(entity => entity.UserName.ToLower().Equals("anonymous")).FirstOrDefault();
        //        var viewElements = viewElementRoleService.GetViewElementGrantedToUser(userProfile);

        //        appBase.ViewElementsGrantedToAnonymousUser = new UserViewElement { UserName = "anonymous", ViewElements = viewElements };
        //    }



        //public bool RoleHasAccess(string userName,string uniqueName, string urlParam = "")
        //{
        //    //var urlString = !string.IsNullOrEmpty(area) ? string.Format("{0}/{1}/{2}", area, controller, action) : string.Format("{0}/{1}", controller, action);

        //    var currentUser = appBase.ViewElementsGrantedToUser.FirstOrDefault(a => a.UserName.ToLower() == userName.ToLower());

        //    if (currentUser == null)
        //    {
        //        var userProfile = UserProfileService.Filter(entity => entity.UserName.ToLower().Equals(userName)).FirstOrDefault();
        //        var viewElements = ViewElementRoleService.GetViewElementGrantedToUser(userProfile);
        //        currentUser = new UserViewElement { UserName = userName, ViewElements = viewElements };
        //        appBase.ViewElementsGrantedToUser.Add(currentUser);
        //    }

        //    var accessVElement =
        //        currentUser.ViewElements;



        //    foreach (var item in accessVElement)
        //    {
        //        //if (item.UniqueName.Contains("?"))
        //        //{
        //        //    if (urlParam != string.Empty)
        //        //    {
        //        //        urlString += "?" + urlParam;
        //        //    }
        //        //}
        //        //if (item.UniqueName == urlString)
        //        //    return true;
        //        if (!string.IsNullOrEmpty(uniqueName))
        //        {
        //            if (HasRequestedUrlAccessInViewElement(item, uniqueName))
        //                return true;

        //        }

        //    }
        //    return false;
        //    // return true;

        //}
        public int GetNewViewElementId()
        {
            return (_repositoryBase as ViewElementRepository).GetNewViewElementId();
        }
        public bool RoleHasAccess(int userId, string uniqueName, string urlParam = "")
        {

            UserViewElement currentUser = null;


            var viewElements = ViewElementRoleService.GetViewElementGrantedToUserByUserId(userId);
            currentUser = new UserViewElement { UserId = userId, ViewElements = viewElements };
            appBase.ViewElementsGrantedToUser.TryAdd(userId, currentUser);


            var accessVElement = currentUser.ViewElements;

            foreach (var item in accessVElement)
            {

                if (!string.IsNullOrEmpty(uniqueName))
                {
                    if (HasRequestedUrlAccessInViewElement(item, uniqueName))
                        return true;

                }

            }
            return false;


        }
        private bool HasRequestedUrlAccessInViewElement(ViewElementInfo item, string requestedUrl)
        {


            if (item.ConceptualName.Contains("?"))
            {
                if (requestedUrl.IndexOf("&_") > 0)
                {
                    requestedUrl = requestedUrl.Substring(0, requestedUrl.IndexOf("&_"));
                }

                if (item.ConceptualName.ToLower() == requestedUrl.ToLower())
                    return true;
            }
            else
            {
                var compareableUrl = requestedUrl.Contains("?") ? requestedUrl.Substring(0, requestedUrl.IndexOf('?')) : requestedUrl;

                if (item.ConceptualName.ToLower().Trim() == compareableUrl.ToLower())
                    return true;
            }
            return false;

        }

        public bool HasAnonymousAccess(string requestedUrl)
        {

            //if (appBase.ViewElementsGrantedToAnonymousUser == null)
            //{
            //var userProfileService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();
            // var viewElementRoleService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IViewElementRoleService>();

            //  var userProfile = UserProfileService.Filter(entity => entity.Id.Equals(2)).FirstOrDefault();
            // 2 => Anonymous userProfile
            var viewElements = ViewElementRoleService.GetViewElementGrantedToUserByUserId(2);

            appBase.ViewElementsGrantedToAnonymousUser = new UserViewElement { UserId = 2, ViewElements = viewElements };
            //}

            var elementWithAccess = appBase.ViewElementsGrantedToAnonymousUser.ViewElements.FirstOrDefault(element => HasRequestedUrlAccessInViewElement(element, requestedUrl));
            if (elementWithAccess == null)
                return false;
            else return true;
        }
        public List<ViewElementDTO> GetAccessibleViewElements(int userId)
        {
            return Cache(GetViewElementByUserId, userId);

        }

        [Cacheable(ExpireCacheSecondTime = 10)]
        public static List<ViewElementDTO> GetViewElementByUserId(int userId)
        {
            var roleList = new List<Role>();
            var ViewElementDTOList = new List<ViewElementDTO>();


            var roles = DependencyInjectionFactory.CreateInjectionInstance<IUserService>().FindUserRoles(userId);
            if (roles.Any())
                roleList = roles.ToList();
            foreach (var role in roleList)
            {


                var viewElements = role.ViewElementRoles
                    .Select(viewElementRole => new ViewElementDTO
                    {
                        UniqueName = viewElementRole.ViewElement.UniqueName,
                        Title = viewElementRole.ViewElement.Title
                    });
                ViewElementDTOList.AddRange(viewElements);


            }
            return ViewElementDTOList;
        }

        public MenuItem AccessibleViewElements(int userId)
        {

            var allNodes = new List<MenuItem>();
            MenuItem rootMenuItem = new MenuItem() { Name = "RootMenu", ViewElement = new ViewElement() };

            allNodes.Add(rootMenuItem);
            var roleList = new List<Role>();

            //var userService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IUserService>();

            var user = UserService.Find(userId);

            var roles = UserService.FindUserRoles(userId);
            if (roles.Any())
                roleList = roles.ToList(); //نقش های کاربر
            else
            {
                var companyId = (int)(user.CompanyChartId != null ? user.CompanyChartId : null);
                //var companyChartRoleService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<ICompanyChartRoleService>();

                var userPosition = CompanyChartRoleService.Filter(a => a.CompanyChartId == companyId).Select(a => a.Role).ToList();
                if (userPosition != null)
                    roleList = userPosition;

            }


            foreach (var role in roleList)
            {


                var viewElements =
                    role
                    .ViewElementRoles
                    .Where(a => !a.ViewElement.InVisible)
                    .Select(a => a.ViewElement)
                     .ToList();

                viewElements.ForEach(c =>
                {
                    GetParentMenuFromViewElement(c, ref allNodes);
                });


            }
            OrderMenuItems(rootMenuItem);
            return rootMenuItem;
        }

        private void OrderMenuItems(MenuItem menuItem)
        {
            menuItem.Childeren = menuItem.Childeren.OrderBy(child => child.SortOrder).ToList();

            foreach (var child in menuItem.Childeren)
            {
                if (child.Childeren.Count() > 0)
                    OrderMenuItems(child);

            }

        }


        public bool IsDuplicateUniqueName(string uniqueName, int viewElementId)
        {
            return (_repositoryBase as ViewElementRepository).IsDuplicateUniqueName(uniqueName, viewElementId);
        }

        public int Delete(int id, string userName)
        {

            return (_repositoryBase as ViewElementRepository).Delete(id, userName);
        }
        //public override void Create(List<ViewElement> objectList, bool allowSaveChange = true)
        //{
        //     (_repositoryBase as ViewElementRepository).Create(objectList, allowSaveChange);
        //}

        private MenuItem GetParentMenuFromViewElement(ViewElement viewElement, ref List<MenuItem> allNodes)
        {
            if (viewElement.ElementType == ElementType.Menu)
            {
                var parent = allNodes.FirstOrDefault(n => n.ViewElement.Id == viewElement.Id /*&& !n.ViewElement.InVisible*/);
                if (parent == null)
                {
                    parent = new MenuItem
                    {
                        ViewElementId = viewElement.Id,
                        Title = viewElement.Title,
                        ViewElement = viewElement,
                        SortOrder = viewElement.SortOrder

                    };
                    allNodes.Add(parent);
                    parent.Childeren = GetChildren(parent, ref allNodes);
                    parent.Parent = GetParent(parent, ref allNodes);

                }

                return parent;
            }
            else if (viewElement.ElementType == ElementType.Page)
            {
                var parent = allNodes.FirstOrDefault(n => n.ViewElement.Id == viewElement.Id /*&& !n.ViewElement.InVisible*/);
                if (parent == null)
                {
                    parent = new MenuItem
                    {
                        ViewElementId = viewElement.Id,
                        Title = viewElement.Title,
                        ViewElement = viewElement,
                    };
                    allNodes.Add(parent);
                }
                return parent;
            }
            else
            {

                if (viewElement.ParentId != null)
                    return GetParentMenuFromViewElement(Find(viewElement.ParentId), ref allNodes);
                else
                    return null;
            }
        }

        private MenuItem GetParent(MenuItem menuItem, ref List<MenuItem> allNodes)
        {
            var parent = allNodes.FirstOrDefault(n => n.ViewElement.Id == menuItem.ViewElement.ParentId && !n.ViewElement.InVisible);
            if (parent == null)
            {
                if (menuItem.ViewElement.ParentId != null && !menuItem.ViewElement.InVisible)
                {
                    var viewElement = Find(a => a.Id == menuItem.ViewElement.ParentId.Value);
                    parent = new MenuItem
                    {
                        ViewElementId = menuItem.ViewElement.ParentId.Value,
                        Title = viewElement.Title,
                        ViewElement = viewElement,
                    };
                    allNodes.Add(parent);
                    parent.Parent = GetParent(parent, ref allNodes);
                    parent.Childeren.Add(menuItem);

                }
                else
                {
                    parent = allNodes.Single(n => n.Name == "RootMenu");

                }
            }
            if (!parent.Childeren.Any(m => m.ViewElement.Id == menuItem.ViewElement.Id && !menuItem.ViewElement.InVisible))
            {
                parent.Childeren.Add(menuItem);
                menuItem.Parent = parent;
            }
            return parent;
        }

        public List<MenuItem> GetChildren(MenuItem menuItem, ref List<MenuItem> allNodes)
        {
            List<MenuItem> resultMenuItems = new List<MenuItem>();
            List<MenuItem> nodes = allNodes;
            if (menuItem.ViewElement.ChildrenViewElement != null)
            {
                menuItem.ViewElement.ChildrenViewElement.Where(el => el.ElementType == 0 && !el.InVisible).ToList().ForEach(c =>
                {
                    var child = nodes.FirstOrDefault(n => n.ViewElement.Id == c.Id && !n.ViewElement.InVisible);

                    if (child == null)
                    {
                        child = new MenuItem
                        {
                            ViewElementId = c.Id,
                            Title = c.Title,
                            ViewElement = c,
                            Parent = menuItem,
                            SortOrder = c.SortOrder

                        };

                        child.Childeren = GetChildren(child, ref nodes);
                        nodes.Add(child);
                    }
                    resultMenuItems.Add(child);
                });
            }
            return resultMenuItems;
        }

        private bool HasAccessViewElement(ViewElement vElement, string urlString)
        {

            if (vElement.UniqueName.Split('#')[1] == urlString)
            {
                return true;
            }


            if (vElement.ChildrenViewElement != null)
                foreach (var childElement in vElement.ChildrenViewElement)
                {

                    if (HasAccessViewElement(childElement, urlString)) return true;


                }

            return false;

        }


        //public override IQueryable<ViewElement> Filter(System.Linq.Expressions.Expression<Func<ViewElement, bool>> predicate, bool allowFilterDeleted = true)
        //{
        //    return _viewElementRepository.Filter(predicate, allowFilterDeleted).Include("ViewElementRoles").Include("ViewElementRoles.Role");
        //}


        public override ViewElement Find(System.Linq.Expressions.Expression<Func<ViewElement, bool>> predicate, bool allowFilterDeleted = true)
        {
            return base.Find(predicate, allowFilterDeleted);
        }




        public int? parentMenuId { get; set; }
    }
}
