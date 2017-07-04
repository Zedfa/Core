using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Core.Cmn;
using Core.Mvc.Controller;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using Core.Mvc.ViewModel;
using Core.Entity;

namespace Core.Mvc.Controllers
{
    public class CompanyChartController : ControllerBaseCr
    {
          private ICompanyChartService  _CompanyChartService;

          public CompanyChartController(ICompanyChartService  CompanyIdChartService)
        {
            _CompanyChartService = CompanyIdChartService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public override ContentResult CreateHelpView(string viewModelName)
        {
            return base.CreateHelpView("CompanyChart");
        }


        public JsonResult Read(int? id)
        {
            var CompanyIdCharts = _CompanyChartService.GetCompanyChart(id);

            var ochart = from e in CompanyIdCharts
                         where  e.ParentId == id 
                         select new
                         {
                             id = e.Id,
                             Title = e.Title,
                             hasChildren = e.ChildCompanyChart.Any(),
                             Level = e.Level
                         };
            return Json(ochart, JsonRequestBehavior.AllowGet);

        }



        public JsonResult Create(string selectedNodeId, string title)
        {
            
            var insertedCompany = _CompanyChartService.Create(new CompanyChart()
            {
                Title = title,
                ParentId = string.IsNullOrEmpty(selectedNodeId) ? (int?)null : int.Parse(selectedNodeId)

            });
           return Json(new { Title = title, SelectedNodeId = selectedNodeId, id = insertedCompany.Id });
    


        }


        public JsonResult Delete(int selectedNodeId)
        {
            _CompanyChartService.Delete(selectedNodeId , HttpContext.User.Identity.Name);
            return Json(new { id = selectedNodeId });


        }
        
        public JsonResult GetCompanyChart()
        {
            var vSelectedCompanyChart = new List<SelectedCompanyChartViewModel>();
            var CompanyCharts = _CompanyChartService.All().ToList();
            foreach (var CompanyIdChart in CompanyCharts)
            {
                vSelectedCompanyChart.Add(new SelectedCompanyChartViewModel()
                {
                    Title = CompanyIdChart.Title,
                    Id = CompanyIdChart.Id
                });
            }

            return Json(vSelectedCompanyChart, JsonRequestBehavior.AllowGet);

        }

    }
}
