using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Mvc.Controller;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Service;
using Core.Entity;
using Core.Cmn;



namespace Core.Mvc.Controllers
{
    public class CompanyRoleController : ControllerBaseCr
    {
        private ICompanyChartRoleService _CompanyChartRoleService;

        private IServiceBase<Role> _roleService;

        public CompanyRoleController(ICompanyChartRoleService CompanyChartRoleService, IServiceBase<Role> roleServiceBase)
        {
            _CompanyChartRoleService = CompanyChartRoleService;
            _roleService = roleServiceBase;
        }

        public ActionResult Index()
        {

            return View(new CompanyRoleViewModel());
        }


        public JsonResult GetRoles([DataSourceRequest] DataSourceRequest request, int? CompanyChartId)
        {
            if (CompanyChartId != null)
            {
                var CompanyRoles = _CompanyChartRoleService.GetRoles((int)CompanyChartId);
                var roles = _roleService.Filter(a => a.Name != GeneralConstant.AdminRoleName).ToList();

                DataSourceResult result = roles.ToDataSourceResult(request, ochart => new CompanyChartRoleViewModel(ochart)
                {
                    HasAccess = CompanyRoles.Any(a => a.ID == ochart.ID),
                    SelectedCompanyChartId = (int)CompanyChartId

                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            return null;
        }


        public ActionResult PostEntity([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] List<CompanyChartRoleViewModel> CompanyRoleViewModels)
        {
            int selectedCompanyChart = 0;
            var insertedRole = new List<int>();
            var deletedRole = new List<int>();
            if (ModelState.IsValid)
            {
                foreach (var oRole in CompanyRoleViewModels)
                {
                    selectedCompanyChart = oRole.SelectedCompanyChartId;
                    if (oRole.HasAccess)
                        insertedRole.Add(oRole.Id);
                    else
                        deletedRole.Add(oRole.Id);

                }


                var addedRole = _CompanyChartRoleService.Create(insertedRole, deletedRole, selectedCompanyChart);

            }


            return RedirectToAction("GetRoles");


        }


    }
}
