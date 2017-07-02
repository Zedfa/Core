using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Core.Mvc.Controller;
using Core.Service;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Entity;
using Core.Cmn;
using WebGrease.Css.Extensions;

namespace Core.Mvc.Controllers
{
    public class ViewElementRoleController : ControllerBaseCr
    {

        private IViewElementService _viewElementService;

        private IViewElementRoleService _viewElementRoleService;

        //private ICompanyRoleService _CompanyRoleService;

        public ViewElementRoleController(IViewElementService viewElementService, IViewElementRoleService viewElementRoleService)//, ICompanyRoleService CompanyRoleService)
        {
            _viewElementService = viewElementService;
            _viewElementRoleService = viewElementRoleService;
            //_CompanyRoleService = CompanyRoleService;

        }


        public ViewElementRoleController()
        {

        }

        public ActionResult Index()
        {
            return View(new ViewElementRoleViewModel()
            {

            });
        }



        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int? id, int? selectedRole)
        {
            var nodes = new List<object>();
            if (selectedRole == null)
                return Json(nodes, JsonRequestBehavior.AllowGet);


            List<ViewElement> viewElements = new List<ViewElement>();
            if (User.Identity.Name.ToLower() == GeneralConstant.AdminUserName.ToLower())
            {
                if (id == null)
                {
                    viewElements = _viewElementService.GetRootViewElements().ToList();
                }
                else
                {
                    viewElements = _viewElementService.GetChildViewElementByParentId(id).ToList();

                }
            }
            else
            {

                viewElements = _viewElementRoleService.GetRootViewElementsBasedOnCompany(id).ToList();
            }


            var chekedItem = _viewElementRoleService.GetViewElementsIdByRoleId(selectedRole);
            viewElements.ForEach(cs =>
            {
                var d = new
                {
                    id = cs.Id.ToString(),
                    Text = cs.Title,
                    Title = cs.Title,
                    hasChildren = (cs.ChildrenViewElement == null || cs.ChildrenViewElement.Count == 0) ? false : true,
                    //expanded = true,
                    @checked = chekedItem.Contains(cs.Id)
                };
                nodes.Add(d);
            });

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }


        //private List<int> GetAssigndedNode(int? roleId)
        //{
        //    var result = new List<int>();
        //    if (roleId != null)
        //    {
        //        var existedVElement = _viewElementRoleService.All().Where(a => a.RoleId == roleId).Select(viewEelementRole=>viewEelementRole.ViewElementId)

        //    }
        //    return result;

        //}



        public void PostEntity(string selectedNode, string unCheckedNode, int roleId)
        {
            var addedViewElement = new List<int>();
            var deletedViewElement = new List<int>();
            if (selectedNode != null)
            {
                selectedNode = selectedNode.Remove(selectedNode.Length - 1, 1);
                var selectedViewElement = selectedNode.Split(',');
                foreach (var vElement in selectedViewElement)
                {
                    addedViewElement.Add(Convert.ToInt32(vElement));
                }
            }


            if (!string.IsNullOrEmpty(unCheckedNode))
            {
                unCheckedNode = unCheckedNode.Remove(unCheckedNode.Length - 1, 1);
                var deletedVElement = unCheckedNode.Split(',');
                foreach (var vElement in deletedVElement)
                {
                    deletedViewElement.Add(Convert.ToInt32(vElement));
                }
            }
            //var addedRole = _viewElementRoleService.Create(addedViewElement, deletedViewElement, roleId);
            var addedRole = _viewElementRoleService.UpdateByRoleId(roleId, addedViewElement, deletedViewElement);


            //return RedirectToAction("Index");

        }
    }
}
