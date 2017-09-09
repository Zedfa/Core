using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn.Extensions;
using Core.Cmn.Attributes;
using Core.Cmn;

namespace Core.Rep
{

    public class ViewElementRoleRepository : RepositoryBase<ViewElementRole>
    {
        private IDbContextBase _dbContext;
        ViewElementRepository viewElementRepository;
        public ViewElementRoleRepository(IDbContextBase dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
            viewElementRepository = new ViewElementRepository(dbContext);

        }


        [Cacheable(EnableSaveCacheOnHDD = true, EnableUseCacheServer = false, ExpireCacheSecondTime = 15)]
        public static IQueryable<ViewElementRole> AllViewElementsCache(IQueryable<ViewElementRole> query)
        {

            return query.Include(element => element.Role)
                .Include(element => element.ViewElement)
                .Include(element => element.ViewElement.ChildrenViewElement).Include(element => element.ViewElement.ParentViewElement);
        }
        public IQueryable<ViewElementRole> AllCache(bool canUseCacheIfPossible = true)
        {
            return Cache(AllViewElementsCache, canUseCacheIfPossible);
        }

        public override IQueryable<ViewElementRole> Filter(System.Linq.Expressions.Expression<System.Func<ViewElementRole, bool>> predicate, bool allowFilterDeleted = true)
        {
            return AllCache().Where(predicate).AsQueryable();

        }
        public List<int> GetViewElementsIdByRoleId(int? roleId)
        {
            return _dbContext.Set<ViewElementRole>().Where(a => a.RoleId == roleId).Select(viewElementRole => viewElementRole.ViewElementId).ToList();
        }
        public IQueryable<ViewElement> GetViewElementsByRoleId(int? roleId)
        {
            return AllCache().Include(viewElement => viewElement.ViewElement).Where(a => a.RoleId == roleId).Select(viewElementRole => viewElementRole.ViewElement);
        }
        List<int?> parentslist = new List<int?>();
        private List<int?> GetParents(ViewElement viewElement)
        {
            if (viewElement.ParentId != null)
            {
                parentslist.Add(viewElement.ParentId);
                GetParents(viewElement.ParentViewElement);
            }

            return parentslist;


        }
        public int UpdateByRoleId(
            int roleId,
            List<int> insertedViewElementList,
            List<int> deletedviewElementList,
            bool allowSaveChange = true
            )
        {

            List<ViewElementRole> existanceViewElementRole = this
                .All()
                .Include(viewElement => viewElement.ViewElement.ParentViewElement)
                .AsNoTracking()
                .Where(vl => vl.RoleId == roleId)
                .ToList();

            #region Add ViewElemntRole to DB

            foreach (int viewElementId in insertedViewElementList)
            {
                if (existanceViewElementRole.Any(vle => vle.ViewElementId == viewElementId) == false)
                {

                    Create(new ViewElementRole
                    {
                        ViewElementId = viewElementId,
                        RoleId = roleId
                    }, false);
                }
            }


            #endregion

            #region Delete removed ViewElementRole

            foreach (int viewElementId in deletedviewElementList)
            {
                if (existanceViewElementRole.Any(viewElement => viewElement.ViewElementId == viewElementId))
                {
                    var deletedViewElement = existanceViewElementRole
                        .First(viewElement => viewElement.ViewElementId == viewElementId);

                    Delete(vle => vle.ViewElementId == viewElementId && vle.RoleId == roleId, false);


                    var similarToDeletedViewElement = existanceViewElementRole
                        .Where(viewElement => viewElement.ViewElement.UniqueName.Trim() == deletedViewElement.ViewElement.UniqueName.Trim()
                                               &&
                                               viewElement.ViewElementId != viewElementId);

                    foreach (var item in similarToDeletedViewElement)
                    {

                        Delete(vle => vle.ViewElementId == item.ViewElementId && vle.RoleId == roleId, false);
                        var parents = GetParents(item.ViewElement);
                        if (parents != null)
                        {
                            foreach (var parent in parents)
                            {

                                Delete(vle => vle.ViewElementId == parent && vle.RoleId == roleId, false);
                            }
                        }
                    }


                }
            }

            #endregion

            if (allowSaveChange)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        public int Create(List<int> insertedViewElement, List<int> deletedviewElementRole, int roleId,
            bool allowSaveChange = true)
        {
            var addedViewElemetId = new List<int>();
            var allViewElements = new List<ViewElement>();
            var allParentViewElements = new List<ViewElement>();

            var findedRole = ContextBase.Set<ViewElementRole>().Where(x => x.RoleId == roleId);

            var existedVElement = findedRole.Select(a => a.ViewElement.Id).ToList();

            //در دیتابیس هستند  در لیست حذف شده هم هست
            var deleteElement = from x in existedVElement
                                where deletedviewElementRole.Contains(x)
                                select x;

            //لیست آیتم هایی که در دیتابیس هست و قرار نیست حذف شود
            var fff =
                findedRole.Where(a => a.ViewElement.IsHidden != true)
                    .Select(a => a.ViewElement.Id)
                    .ToList();
            var existedItemForAdd = fff.Where(item => !deleteElement.Contains(item)).ToList();


            foreach (int item in existedItemForAdd)
            {
                if (!insertedViewElement.Contains(item))
                    insertedViewElement.Add(item);
            }




            foreach (var vId in existedVElement)
            {
                var findedVElement = ContextBase.Set<ViewElementRole>().FirstOrDefault(x => x.RoleId == roleId && x.ViewElementId == vId);
                if (findedVElement != null)
                {
                    ContextBase.Set<ViewElementRole>().Remove(findedVElement);
                }

            }


            foreach (var velementId in insertedViewElement)
            {
                //var foundVelement = ContextBase.Set<ViewElement>().Find(velementId);
                var founded = viewElementRepository.GetViewElementAndChildsById(velementId);
                if (founded.ChildrenViewElement != null)
                {
                    foreach (var childElement in founded.ChildrenViewElement)
                    {

                        var parentElement = viewElementRepository.GetViewElementAndChildsById(childElement.ParentId ?? 0);
                        List<ViewElement> resultElements;
                        resultElements = GetHiddenEelements(parentElement);
                        if (resultElements != null)
                        {
                            foreach (var resultElement in resultElements)
                            {
                                var hElement = ContextBase.Set<ViewElement>().Find(resultElement.Id);
                                if (!addedViewElemetId.Any(a => a.Equals(hElement.Id)))
                                    Create(new ViewElementRole()
                                    {

                                        RoleId = roleId,
                                        ViewElementId = hElement.Id

                                    }, false);
                                //...
                                addedViewElemetId.Add(hElement.Id);
                                allViewElements.Add(founded);
                            }

                        }

                    }

                }


                //foundVelement.Roles.Add(findedRole);
                if (!addedViewElemetId.Any(a => a.Equals(founded.Id)))
                    Create(new ViewElementRole()
                    {

                        RoleId = roleId,
                        ViewElementId = founded.Id

                    }, false);
                //...
                addedViewElemetId.Add(founded.Id);
                allViewElements.Add(founded);
            }


            //...

            foreach (var item in allViewElements)
            {
                if (!allViewElements.Any(r => r.Id == item.ParentId))
                {
                    var foundParent = ContextBase.Set<ViewElement>().Find(item.ParentId); //Find(item.ParentId);

                    if (foundParent != null)
                    {
                        if (!allParentViewElements.Any(a => a.Id == foundParent.Id))
                            allParentViewElements.Add(foundParent);
                    }



                }

            }

            foreach (var parent in allParentViewElements)
            {
                if (parent.ChildrenViewElement != null)
                {
                    var hiddenElements = parent.ChildrenViewElement.Where(a => a.IsHidden).ToList();
                    foreach (var hItem in hiddenElements)
                    {
                        // hItem.Roles.Add(findedRole);
                        if (!addedViewElemetId.Any(a => a.Equals(hItem.Id)))
                            Create(new ViewElementRole()
                            {

                                RoleId = roleId,
                                ViewElementId = hItem.Id

                            }, false);
                        addedViewElemetId.Add(hItem.Id);
                        allViewElements.Add(hItem);
                    }
                }
            }
            var roles = findedRole.Select(x => x.Role).FirstOrDefault();
            if (roles != null)
            {
                ContextBase.Set<Role>().Attach(roles);
            }

            if (allowSaveChange)
                // return ContextBase.SaveChanges();
                return SaveChanges();
            return 0;
        }

        private List<ViewElement> GetHiddenEelements(ViewElement parentElement)
        {
            List<ViewElement> resultElements = new List<ViewElement>();
            var hiddenEl = parentElement.ChildrenViewElement.FirstOrDefault(element => element.IsHidden);
            if (hiddenEl != null) resultElements.Add(hiddenEl);

            if (parentElement.ChildrenViewElement != null)
                foreach (var childEl in parentElement.ChildrenViewElement)
                {
                    var hElement = GetHiddenEelements(childEl);
                    resultElements.AddRange(hElement);
                }
            return resultElements;
        }
        public IQueryable<ViewElementRole> All_ViewElement_Role()
        {
            return AllCache().Include(item => item.Role).Include(item => item.ViewElement);
        }
        public IQueryable<ViewElement> GetRootViewElementsBasedOnCompany(int? id, int companyId)
        {

            var allViewElementsBasedOnCompany = All_ViewElement_Role()
                .Where(viewElementRole => viewElementRole.Role.CurrentCompanyId == companyId &&
                    viewElementRole.ViewElement.ParentId == id &&
                    viewElementRole.ViewElement.IsHidden != true &&
                    viewElementRole.ViewElement.InVisible == false)
                    .Select(viewElement => viewElement.ViewElement);


            return allViewElementsBasedOnCompany;
        }
    }
}
