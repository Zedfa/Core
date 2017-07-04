using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Service.Models;
using Core.Entity.Enum;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IViewElementRoleService), DomainName = "core")]
    public class ViewElementRoleService : ServiceBase<ViewElementRole>, IViewElementRoleService
    {

      
        private IUserService UserService { get; set; }
        private IUserRoleService UserRoleService { get; set; }                  
        private ICompanyChartRoleService CompanyChartRoleService { get; set; }
        

        public ViewElementRoleService(IDbContextBase dbContextBase, IUserRoleService userRoleService, IUserService userService, ICompanyChartRoleService companyChartRoleService)//,IViewElementService viewElementService)
            : base(dbContextBase)
        {

        
            _repositoryBase = new ViewElementRoleRepository(ContextBase);
            UserService = userService;
            UserRoleService = userRoleService;
            CompanyChartRoleService = companyChartRoleService;
            //_viewElementService = viewElementService;
        }

        public ViewElementRoleService(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {

        }
        public List<int> GetViewElementsIdByRoleId(int? roleId)
        {
            return (_repositoryBase as ViewElementRoleRepository).GetViewElementsIdByRoleId(roleId);
        }

        public IQueryable<ViewElement> GetViewElementsByRoleId(int? roleId)
        {
            return (_repositoryBase as ViewElementRoleRepository).GetViewElementsByRoleId(roleId);
        }
        public IQueryable<ViewElement> GetRootViewElementsBasedOnCompany(int? id)
        {
            return (_repositoryBase as ViewElementRoleRepository).GetRootViewElementsBasedOnCompany(id,appBase.CompanyId);
        }
        public int Create(List<int> addedViewElementRole, List<int> deletedviewElementRole, int roleId, bool allowSaveChange = true)
        {

            return (_repositoryBase as ViewElementRoleRepository).Create(addedViewElementRole, deletedviewElementRole, roleId, allowSaveChange);
        }

        public int UpdateByRoleId(
          int roleId,
          List<int> insertedViewElement,
          List<int> removedViewElement,
          bool allowSaveChange = true
          )
        {
            return (_repositoryBase as ViewElementRoleRepository).UpdateByRoleId(roleId, insertedViewElement, removedViewElement, allowSaveChange);
        }
        //public void SetViewElementGrantedToUser(UserProfile userProf)
        //{
        //    lock (AppBase.ViewElementsGrantedToUser)
        //    {
        //        var viewElementGrantedToUser = GetViewElementGrantedToUser(userProf);
        //        if (AppBase.ViewElementsGrantedToUser.FirstOrDefault(item => item.UserName.ToLower() !=userProf.UserName.ToLower()) == null)
        //            AppBase.ViewElementsGrantedToUser.Add(new UserViewElement
        //            {
        //                UserName = userProf.UserName.ToLower(),
        //                ViewElements = viewElementGrantedToUser
        //            });

        //    }
        //}
        public IList<ViewElementInfo> GetViewElementGrantedToUserByUserId(int userId)
        {
            return Cache(GetViewElementGrantedToUserCache, userId);

        }


        public void SetViewElementGrantedToUser(UserProfile userProf)
        {
            var viewElementGrantedToUser = GetViewElementGrantedToUserByUserId(userProf.Id);
            UserViewElement currentUser = null;
            if (!AppBase.ViewElementsGrantedToUser.TryGetValue(userProf.Id, out currentUser))
            {
                AppBase.ViewElementsGrantedToUser.TryAdd(userProf.Id, new UserViewElement
                {
                    UserId = userProf.Id,
                    ViewElements = viewElementGrantedToUser
                });
            }
        }

        [Cacheable(ExpireCacheSecondTime = 60)]
        public static IList<ViewElementInfo> GetViewElementGrantedToUserCache(int userId)
        {
            var viewElementRoleService = ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IViewElementRoleService>();
            return viewElementRoleService.GetUserViewElements(userId);
        }

        public IList<ViewElementInfo> GetUserViewElements(int userId)
        {
            var grantedViewElements = new List<ViewElement>();
            var allGrantedViewElementsToUser = new List<ViewElementInfo>();
            var userRoles = UserRoleService.GetRolesByUserId(userId);

            foreach (var role in userRoles)
            {
                var viewElementsByRole =
                    (_repositoryBase as ViewElementRoleRepository)
                    .Filter(viewElementRole => viewElementRole.RoleId == role.RoleID)
                    .ToList();

                foreach (var viewElement in viewElementsByRole)
                {
                    if (!grantedViewElements.Any(a => a.Id == viewElement.ViewElementId))
                    {
                        grantedViewElements.Add(viewElement.ViewElement);
                        allGrantedViewElementsToUser.Add(
                            new ViewElementInfo
                            {
                                ConceptualName = viewElement.ViewElement.UniqueName.Split('#')[0],
                                Url = viewElement.ViewElement.UniqueName.Split('#')[1],
                                ElementType = (ElementType)viewElement.ViewElement.ElementType
                            });
                    }

                    var rv = GetChildrenEelements(viewElement.ViewElement);
                    rv.AddRange(GetParentElements(viewElement.ViewElement));

                    foreach (var resultElement in rv.ToList())
                    {
                        if (!grantedViewElements.Any(a => a.Id == resultElement.Id))
                        {
                            grantedViewElements.Add(resultElement);
                            allGrantedViewElementsToUser.Add(
                                new ViewElementInfo
                                {
                                    ConceptualName = resultElement.UniqueName.Split('#')[0],
                                    Url = resultElement.UniqueName.Split('#')[1],
                                    ElementType = resultElement.ElementType
                                });
                        }
                    }
                }
            }

            return allGrantedViewElementsToUser;
        }

        private static List<ViewElement> GetParentElements(Entity.ViewElement viewElement)
        {
            var res = new List<ViewElement>();
            if (viewElement.ParentViewElement != null)
            {
                res.Add(viewElement.ParentViewElement);
                res.AddRange(GetParentElements(viewElement.ParentViewElement).ToArray());
            }
            return res;
        }



        private List<ViewElement> GetChildrenEelements(ViewElement element)
        {
            var resultElements = new List<ViewElement>();
            var viewElements = element.ChildrenViewElement;
            if (viewElements != null)
            {
                viewElements = viewElements.ToList();

                foreach (var viewElement in viewElements)
                {
                    // if(!resultElements.Any(a=>a.Id==element.Id))
                    resultElements.Add(viewElement);
                }


                if (element.ChildrenViewElement != null)
                    foreach (var childEl in element.ChildrenViewElement.Where(a => (!a.InVisible)).ToList())
                    {
                        var hElement = GetChildrenEelements(childEl);

                        resultElements.AddRange(hElement);
                    }
            }
            return resultElements;
        }








    }
}
