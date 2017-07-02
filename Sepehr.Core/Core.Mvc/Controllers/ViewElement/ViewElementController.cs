using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Core.Mvc.Controller;
using Core.Mvc.Extensions;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using Core.Mvc.ViewModel;
using Core.Entity;

namespace Core.Mvc.Controllers
{
    public class ViewElementController : ControllerBaseCr
    {
        private IViewElementService _viewElementService;

        public ViewElementController(IViewElementService viewElementService)
        {
            _viewElementService = viewElementService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEntities([DataSourceRequest] DataSourceRequest request, int? id)
        {
            var nodes = new List<object>();
            var viewElements = new List<ViewElement>();
            if (id == null)
            {
                viewElements = _viewElementService.GetRootViewElements().ToList();
            }
            else
            {
                viewElements = _viewElementService.GetChildViewElementByParentId(id).ToList();

            }

            //var viewElements = _viewElementService.GetAllViewElement(id).ToList();
            viewElements.ForEach(viewElement =>
            {
                var node = new
                {
                    SortOrder=viewElement.SortOrder,
                    id = viewElement.Id,
                    Title = viewElement.Title,
                    ElementType = viewElement.ElementType,
                    Invisible = viewElement.InVisible,
                    IsHidden = viewElement.IsHidden,
                    UniqueName = viewElement.UniqueName,
                    XMLViewData = viewElement.XMLViewData,
                    hasChildren = viewElement.ChildrenViewElement.Any()

                };
                nodes.Add(node);
            });

            return Json(nodes, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult PostEntity(ViewElementViewModel viewElementViewModel)
        {
            //var viewElementByUniqueName = _viewElementService.GetViewElementByUniqueName(viewElementViewModel.UniqueName);

            //if (viewElementByUniqueName != null)
            //{
            //    if (viewElementByUniqueName.Id != viewElementViewModel.Id)
            //    {
            //        return Json(new
            //        {
            //            success = false,
            //            errors = "نام یونیک تکراری می باشد"
            //        });
            //    }

            //}

            var viewElement = _viewElementService.Create(viewElementViewModel.Model);
            viewElementViewModel.SetModel(viewElement);
            return Json(new
            {
                success = true,
                viewElementViewModel
            });


        }


        public JsonResult PutEntity(ViewElementViewModel viewElementViewModel)
        {
            var viewElementBYUniqueName = _viewElementService.GetViewElementByUniqueName(viewElementViewModel.UniqueName);

            //if (viewElementBYUniqueName != null)
            //{
            //    if (viewElementBYUniqueName.Id != viewElementViewModel.Id)
            //    {
            //        return Json(new
            //        {
            //            success = false,
            //            errors = "نام یونیک تکراری می باشد"
            //        });
            //    }

            //}

            var selectedViewElement = _viewElementService.Find(viewElementViewModel.Id);
            selectedViewElement.ParentId = viewElementViewModel.ViewElementParentId;
            selectedViewElement.Title = viewElementViewModel.Title;
            selectedViewElement.InVisible = viewElementViewModel.Invisible;
            selectedViewElement.IsHidden = viewElementViewModel.IsHidden;
            selectedViewElement.UniqueName = viewElementViewModel.UniqueName;
            selectedViewElement.XMLViewData = viewElementViewModel.XMLViewData;
            selectedViewElement.ElementType = viewElementViewModel.ElementType;
            selectedViewElement.SortOrder = viewElementViewModel.SortOrder;
            var updatedViewElement = _viewElementService.Update(selectedViewElement);
            return Json(new
            {
                success = true,
                viewElementViewModel
            });


        }
        public JsonResult LoadSelectedViewElementMenu(string selectedViewElementMenuId)
        {
            var viewElement = _viewElementService.Find(Convert.ToInt32(selectedViewElementMenuId));

            var viewElementViewModel = new ViewElementViewModel(viewElement);
            return Json(viewElementViewModel);

        }
        public int GetNewViewElementId()
        {
            return _viewElementService.GetNewViewElementId();
        }

        public JsonResult DeleteEntity(int selectedViewElementMenuId)
        {
            _viewElementService.Delete(selectedViewElementMenuId);
            return Json(new { id = selectedViewElementMenuId }, JsonRequestBehavior.AllowGet);

        }
    }
}
