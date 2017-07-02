using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Mvc.Controller;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Entity;
using Core.Service;
using WebGrease.Css.Extensions;

namespace Core.Mvc.Controllers
{
    public class CompanyViewElementController : ControllerBaseCr
    {

        private IViewElementService _viewElementService;
        private ICompanyViewElementService _CompanyViewElementService;




        public CompanyViewElementController(IViewElementService viewElementService, ICompanyViewElementService CompanyViewElementService)
        {
            _viewElementService = viewElementService;
            _CompanyViewElementService = CompanyViewElementService;

        }


        public CompanyViewElementController()
        {

        }

        public ActionResult Index()
        {

            return View(new CompanyViewElementViewModel());
        }



        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int? id, int? selectedCompanyIdViewElement)
        {
            //..در این قسمت باید لیست کلیه منوها اورده شود
            var nodes = new List<object>();
            if (selectedCompanyIdViewElement == null)
                return Json(nodes, JsonRequestBehavior.AllowGet);

            IList<ViewElement> viewElements;
            if (id == null)
            {
                viewElements = _viewElementService.GetRootViewElements().ToList();
            }
            else
            {
                //viewElements = _viewElementService.GetChildMenusByParentMenuId(id).ToList();

            }



            var chekedItem = GetAssigndedNode(selectedCompanyIdViewElement);
            //viewElements.ForEach(cs =>
            //{
            //    var d = new
            //    {
            //        id = cs.Id.ToString(),
            //        Text = cs.Title,
            //        Title = cs.Title,
            //        hasChildren = cs.ChildViewElement.Any(),
            //        //expanded = true,
            //        @checked = chekedItem.Contains(cs.Id)
            //    };
            //    nodes.Add(d);
            //});

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }


        private List<int> GetAssigndedNode(int? selectedCompanyIdViewElement)
        {
            var result = new List<int>();
            if (selectedCompanyIdViewElement != null)
            {
                var existedVElement = _CompanyViewElementService.Filter(a => a.CompanyId == selectedCompanyIdViewElement).ToList();//findedRole.ViewElements.ToList();

                foreach (var item in existedVElement)
                {
                    result.Add(item.ViewElementId);
                }


            }
            return result;

        }



        public ActionResult PostEntity(string selectedNode, string unCheckedNode, int CompanyIdViewElement)
        {
            var addedViewElement = new List<int>();
            var deletedViewElement = new List<int>();
            if (selectedNode != null)
            {
                selectedNode = selectedNode.Remove(selectedNode.Length - 1, 1);
                var selectedViewElement = selectedNode.Split(',');

                addedViewElement = GetViewElementGrantedToCompany(selectedViewElement);
            }

            if (unCheckedNode != null)
            {
                unCheckedNode = unCheckedNode.Remove(unCheckedNode.Length - 1, 1);
                var deletedVElement = unCheckedNode.Split(',');
                //برای حذف همه بچه ها را هم باید به دست آورده و حذف کنیم...  
                deletedViewElement = GetViewElementGrantedToCompany(deletedVElement);

            }

            var addedRole = _CompanyViewElementService.Create(addedViewElement, deletedViewElement, CompanyIdViewElement, false);



            return RedirectToAction("Index");

        }


        private List<int> GetViewElementGrantedToCompany(string[] viewElements)
        {
            var grntedVElements = new List<ViewElement>();
            var getViewElementGrantedToCompany = new List<int>();

            foreach (var vElementId in viewElements)
            {
                var viewElement = _viewElementService.GetViewElementAndViewElementChildByID(int.Parse(vElementId));
                if (!grntedVElements.Any(a => a.Id == int.Parse(vElementId)))
                {

                    grntedVElements.Add(viewElement);
                    getViewElementGrantedToCompany.Add(viewElement.Id);
                }

                var rv = GetEelements(viewElement);
                foreach (var resultElement in rv.ToList())
                {
                    if (!grntedVElements.Any(a => a.Id == resultElement.Id))
                    {
                        grntedVElements.Add(resultElement);
                        getViewElementGrantedToCompany.Add(resultElement.Id);
                    }
                }
            }

            return getViewElementGrantedToCompany;
        }



        private List<ViewElement> GetEelements(ViewElement element)
        {
            var resultElements = new List<ViewElement>();
            var viewElements = element.ChildrenViewElement.ToList();

            foreach (var viewElement in viewElements)
            {
                resultElements.Add(viewElement);
            }


            if (element.ChildrenViewElement != null)
                foreach (var childEl in element.ChildrenViewElement.ToList())
                {
                    var hElement = GetEelements(childEl);
                    resultElements.AddRange(hElement);
                }
            return resultElements;
        }
















    }
}
