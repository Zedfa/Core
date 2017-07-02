using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Cmn.Extensions;

namespace Core.Rep
{
    public class CompanyViewElementRepository : RepositoryBase<CompanyViewElement>
    {
        public CompanyViewElementRepository(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {


        }


        public int Create(List<int> inseretedCompanyViewElement, List<int> deletedCompanyViewElement, int CompanyId, bool allowSaveChange = true)
        {
            var addedViewElemetId = new List<int>();
            var allViewElements = new List<ViewElement>();
            var allParentViewElements = new List<ViewElement>();

            var foundedCompany = ContextBase.Set<Company>().Find(CompanyId);

            var existedVElement = foundedCompany.CompanyViewElements.Select(a => a.ViewElement.Id).ToList();

            //در دیتابیس هستند  در لیست حذف شده هم هست
            var deleteElement = from x in existedVElement
                                where deletedCompanyViewElement.Contains(x)
                                select x;

            //لیست آیتم هایی که در دیتابیس هست و قرار نیست حذف شود
            var neverDeleteItem =
                foundedCompany.CompanyViewElements.Where(a => a.ViewElement.IsHidden != true)
                    .Select(a => a.ViewElement.Id)
                    .ToList();
            var existedItemForAdd = neverDeleteItem.Where(item => !deleteElement.Contains(item)).ToList();


            foreach (int item in existedItemForAdd)
            {
                if (!inseretedCompanyViewElement.Contains(item))
                    inseretedCompanyViewElement.Add(item);
            }




            foreach (var vId in existedVElement)
            {
                var foundedViewElement = ContextBase.Set<CompanyViewElement>().Find(vId,CompanyId);
                ContextBase.Set<CompanyViewElement>().Remove(foundedViewElement);
            }


            foreach (var velementId in inseretedCompanyViewElement)
            {
                var foundVelement = ContextBase.Set<ViewElement>().Find(velementId);


                foreach (var childElement in foundVelement.ChildrenViewElement)
                {

                    var parentElement = ContextBase.Set<ViewElement>().Find(childElement.ParentId);
                    List<ViewElement> resultElements;
                    resultElements = GetHiddenEelements(parentElement);
                    if (resultElements != null)
                    {
                        foreach (var resultElement in resultElements)
                        {
                            var hElement = ContextBase.Set<ViewElement>().Find(resultElement.Id);
                            if (!addedViewElemetId.Any(a => a.Equals(hElement.Id)))
                                Create(new CompanyViewElement()
                                {
                                  
                                    CompanyId = CompanyId,
                                    ViewElementId = hElement.Id

                                }, false);






                            //...
                            addedViewElemetId.Add(hElement.Id);
                            allViewElements.Add(foundVelement);
                        }

                    }

                }


                if (!addedViewElemetId.Any(a => a.Equals(foundVelement.Id)))
                    Create(new CompanyViewElement()
                    {
                       
                        CompanyId = CompanyId,
                        ViewElementId = foundVelement.Id

                    }, false);
                //...
                addedViewElemetId.Add(foundVelement.Id);
                allViewElements.Add(foundVelement);
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
                        if (!addedViewElemetId.Any(a => a.Equals(hItem.Id)))
                            Create(new CompanyViewElement()
                            {
                              
                                CompanyId = CompanyId,
                                ViewElementId = hItem.Id

                            }, false);
                        addedViewElemetId.Add(hItem.Id);
                        allViewElements.Add(hItem);
                    }
                }
            }

            ContextBase.Set<Company>().Attach(foundedCompany);
            if (allowSaveChange)
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

         public IQueryable<ViewElement> GetViewElement(int? id)
        {
            var currentCompany = UserLog.GetCompanyId();
            //if (id.HasValue)
            //{
            //    return DbSet.Where(a => a.CompanyId == currentCompany).Select(a => a.ViewElement).Where(a => a.ParentId == id && a.IsHidden != true && (!a.InVisible));
            //}
            return DbSet.Where(a => a.CompanyId == currentCompany).Select(a => a.ViewElement).Where(a => a.ParentId == id && a.IsHidden != true && (!a.InVisible)).Include("ChildViewElement");
        }

         public IList<ViewElement> GetViewElementList(int? id)
         {
             var currentCompany = UserLog.GetCompanyId();
             return DbSet.Where(a => a.CompanyId == currentCompany).Select(a => a.ViewElement).Where(a => a.ParentId == id && a.IsHidden != true && (!a.InVisible)).Include("ChildViewElement").ToList();
         }

         
    }
}
