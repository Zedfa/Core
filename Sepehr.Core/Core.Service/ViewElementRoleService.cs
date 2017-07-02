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

        private IUserLog _userLog;

        //[Dependency]
        private IUserService UserService { get; set; }


        //[Dependency]
        private ICompanyChartRoleService CompanyChartRoleService { get; set; }

        // private ViewElementRoleRepository _viewElementRepository;
        //private IViewElementService _viewElementService { get; set; }

        public ViewElementRoleService(IDbContextBase dbContextBase, IUserLog userLog, IUserService userService, ICompanyChartRoleService companyChartRoleService)//,IViewElementService viewElementService)
            : base(dbContextBase, userLog)
        {

            _userLog = userLog;
            _repositoryBase = new ViewElementRoleRepository(ContextBase, userLog);
            UserService = userService;
            CompanyChartRoleService = companyChartRoleService;
            //_viewElementService = viewElementService;
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
            return (_repositoryBase as ViewElementRoleRepository).GetRootViewElementsBasedOnCompany(id);
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

        public void SetViewElementGrantedToUser(UserProfile userProf)
        {
            var viewElementGrantedToUser = GetViewElementGrantedToUser(userProf.Id);
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


        public IList<ViewElementInfo> GetViewElementGrantedToUser(int userId)
        {

            var grntedVElements = new List<ViewElement>();
            var _allGrantedViewElementToUserList = new List<ViewElementInfo>();
            var roleList = new List<Role>();
            var roles = UserService.FindUserRoles(userId);
            var user = UserService.Find(userId);
            if (roles.Any())
                roleList = roles.ToList(); //نقش های کاربر
            else
            {
                int companyId = (int)(user.CompanyChartId != null ? user.CompanyChartId : null);


                var userPosition = CompanyChartRoleService.Filter(a => a.CompanyChartId == companyId).Select(a => a.Role).ToList();

                if (userPosition != null)
                    roleList = userPosition;

            }



            foreach (var role in roleList)
            {


                var viewElements = (_repositoryBase as ViewElementRoleRepository).Filter(a => a.RoleId == role.ID).ToList();

                foreach (var viewElement in viewElements)
                {
                    if (!grntedVElements.Any(a => a.Id == viewElement.ViewElementId))// if (!_allGrantedViewElementToUserList.Any(a => a.Contains( viewElement.UniqueName)))/
                    {
                        //if ((!viewElement.ViewElement.InVisible))
                        //{
                            grntedVElements.Add(viewElement.ViewElement);
                            _allGrantedViewElementToUserList.Add(
                                new ViewElementInfo
                                {
                                    ConceptualName = viewElement.ViewElement.UniqueName.Split('#')[0],
                                    Url = viewElement.ViewElement.UniqueName.Split('#')[1],
                                    ElementType = (ElementType)viewElement.ViewElement.ElementType
                                });
                        //}

                    }

                    var rv = GetChildrenEelements(viewElement.ViewElement);
                    rv.AddRange(GetParentElements(viewElement.ViewElement));

                    foreach (var resultElement in rv.ToList())
                    {
                        if (!grntedVElements.Any(a => a.Id == resultElement.Id))
                        // if (!_allGrantedViewElementToUserList.Any(a => a.Contains( viewElement.UniqueName)))
                        {
                            grntedVElements.Add(resultElement);
                            _allGrantedViewElementToUserList.Add(
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
            return _allGrantedViewElementToUserList;


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
