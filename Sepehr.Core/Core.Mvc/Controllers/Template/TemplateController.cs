using Core.Cmn.Reflection;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Mvc.Helpers;
using Core.Mvc.Helpers.CoreKendoGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Mvc.Controllers
{
    public class TemplateController : Core.Mvc.Controller.ControllerBaseCr
    {
       
        // GET: ControlPanel/Template
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public string Get(string templateUrl, string viewModelFullName)
        {
           // viewModelFullName = viewModelFullName.Replace("_", ".");
            var type = ReflectionHelpers.GetType(viewModelFullName);

            //if (type == null)
            //    type = Type.GetType("Sepehr360.Rep.DTO." + viewModel + ",Sepehr360.Rep"); ;
            var partialViewStrEquivalent = this.PartialViewToString(templateUrl, Activator.CreateInstance(type)).Replace("\r\n", ""); ;
            

            var noneSerializedEditTemplateStr = partialViewStrEquivalent
                                                .Replace("\\D+\\d+|\\d+\\D+", "\\\\D+\\\\d+\\|\\\\d+\\\\D+")
                                                .Replace("\\d{", "\\\\d{")
                                                .Replace("^\\d", "^\\\\d")
                                                .Replace("</script>", @"<\/script>")
                                                .Replace("#", "\\\\\\#");

            return noneSerializedEditTemplateStr;
        }


        //render tree template
        public PartialViewResult Show(string templateUrl, string viewModel)
        {
            var vm = Activator.CreateInstance(Type.GetType("Sepehr360.Mvc.Areas.ControlPanel.ViewModel." + viewModel));
            return PartialView(templateUrl, vm);


        }

        public PartialViewResult GetGridViewTemplate(string gName, string viewModel, string onDelete)
        {
            Type viewInfoType = ReflectionHelpers.GetType(viewModel);

            System.Reflection.PropertyInfo viewInfoProp = viewInfoType.GetProperty("ViewInfo", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);

            var viewInfoMetaData = viewInfoProp.GetValue(null, null) as Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo;

            viewInfoMetaData.Features.PreDeletionCallback = onDelete;


            var gridModel = new GridInfoViewModel() { GridName = gName, GridInfo = viewInfoMetaData, ModelTypeFullName = viewModel };
            return PartialView("~/Views/PartialViews/Templates/GridViewTemplate.cshtml", gridModel);
        }

        public ContentResult GetGridTotalConfigurationOptions(string gName, string viewModel,string property, string onDelete, string lkpPropName, bool? isLookup)
        {
            //string json = string.Empty;

            Dictionary<string, object> config = null;

            

            //if (string.IsNullOrEmpty(lkpPropName))
            if (!isLookup.GetValueOrDefault())
            {
                config = CreateGridConfigByViewModel(viewModel, property, gName, onDelete);
                                
//                json = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                //json = JsonConvert.SerializeObject(config);

            }
            else
            {
                config = CreateGridConfigByViewModel(viewModel, lkpPropName, gName, onDelete);
               // json = JsonConvert.SerializeObject(config);
            }


           

            return Content(JsonConvert.SerializeObject(config), "application/json");
        }

        private static Dictionary<string, object> CreateGridConfigByViewModel(string Class,string propertyName,string gridName,string onDelete)
        {
            Type viewModelClass = ReflectionHelpers.GetType(Class);
            propertyName = propertyName ?? "ViewInfo";
            System.Reflection.PropertyInfo viewInfoProp = viewModelClass.GetProperty(propertyName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);

                var viewInfoMetaData = viewInfoProp.GetValue(null, null) as Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo;

                if (!string.IsNullOrEmpty(onDelete))
                {
                    viewInfoMetaData.Features.PreDeletionCallback = onDelete;
                }

                //viewInfoMetaData.DtoModelType = viewInfoType;

                var gridCr = new GridCr<object>(null, gridName, viewInfoMetaData, null, null, null, null);
                return gridCr.GetGridTotalConfig();
        }
        //public ContentResult GetGridTotalConfigurationOptions(string gName, string viewModel, string onDelete)
        //{
        //    Type viewInfoType = ReflectionHelpers.GetType(viewModel);

        //    System.Reflection.PropertyInfo viewInfoProp = viewInfoType.GetProperty("ViewInfo", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);

        //    var viewInfoMetaData = viewInfoProp.GetValue(null, null) as Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo;

        //    viewInfoMetaData.Features.PreDeletionCallback = onDelete;

        //    viewInfoMetaData.DtoModelType = viewInfoType;
            
        //    var gridCr = new GridCr<object>(null, gName, viewInfoMetaData, null, null, null, null); 
        //    var configDic = gridCr.GetGridTotalConfig();


        //    var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        //    var json = JsonConvert.SerializeObject(configDic, Formatting.Indented, jsonSerializerSettings);

        //    return Content(json, "application/json");

        //    //var retJson = Json(
        //    //   new { gridOptions = from item in configDic select new { item } }
                
        //    //    , JsonRequestBehavior.AllowGet);
        //    //retJson.MaxJsonLength = 10000;
        //    //return retJson;
        //    //var gridModel = new GridInfoViewModel() { GridName = gName, GridInfo = viewInfoMetaData, ModelTypeFullName = viewModel };
        //   // return PartialView("~/Views/PartialViews/Templates/GridViewTemplate.cshtml", gridModel);
        //}

        public PartialViewResult GetLookupTemplate(string areaName)
        {
            return PartialView("~/Areas/Core/Views/Lookup/LookupTemplate.cshtml");
        }

        public PartialViewResult UploadGridLookup(string templateUrl, string viewModel, string viewInfo, string lookup, bool isMultiSelect
             , string propertyNameForDisplay, string propertyNameForValue, string propertyNameForBinding)
        {

            
            //Type viewInfotype = Type.GetType(viewModel);

           // System.Reflection.PropertyInfo viewInfoProp = viewInfotype.GetProperty(viewInfo, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);

            //var viewInfoMetaData = viewInfoProp.GetValue(null, null) as Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo;
           Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo viewInfoMetaData ;
           if (LookUpHelper.AllGridLookups.TryGetValue(viewInfo, out viewInfoMetaData))
           {
               var gridViewModel = new Core.Mvc.Helpers.Lookup.Grid
               {

                   GridInfo = viewInfoMetaData,
                   //when call from createControl grid has no Id
                   //GridID = string.IsNullOrEmpty(viewInfoMetaData.GridID) ? string.Format("lookupGrid_{0}", lookup) : viewInfoMetaData.GridID,
                   GridID = viewInfoMetaData.GridID,

                   ClientDependentFeatures = viewInfoMetaData.ClientDependentFeatures,

                   LookupName = lookup,

                   ViewModel = viewModel,//t.AssemblyQualifiedName, //viewModel,

                   // ViewModelType = t ,

                   UseMultiSelect = isMultiSelect,

                   PropertyNameForDisplay = propertyNameForDisplay,

                   PropertyNameForValue = propertyNameForValue,

                   PropertyNameForBinding = propertyNameForBinding

               };

               return PartialView(templateUrl, gridViewModel);

           }
           else
           {
               throw new Exception("your lookupKey is not correct");
           }
        }


        public PartialViewResult UploadTreeLookup(string templateUrl, string viewModel, string ViewInfo, string lookup, bool isMultiSelect
            , string propertyNameForDisplay, string propertyNameForBinding)
        {

            //Type t = Type.GetType(viewModel);

            //System.Reflection.PropertyInfo p = t.GetProperty(ViewInfo, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy);

            //var info = p.GetValue(null, null) as Core.Mvc.Helpers.TreeInfo;

            TreeInfo viewInfoMetaData;

            if (LookUpHelper.AllTreeLookups.TryGetValue(ViewInfo, out viewInfoMetaData))
            {

                var treeViewModel = new Core.Mvc.Helpers.Lookup.Tree
                {

                    TreeInfo = viewInfoMetaData,

                    TreeID = viewInfoMetaData.Name,

                    LookupName = lookup,

                    UseMultiSelect = isMultiSelect,

                    PropertyNameForBinding = propertyNameForBinding,

                    PropertyNameForDisplay = viewInfoMetaData.DataTextField

                };

                return PartialView(templateUrl, treeViewModel);

            }
            else
            {
                throw new Exception("your lookupKey is not correct");
            }

        }


    }

     

}
public static class ControllerExtensionsHelper
{
    public static string PartialViewToString(this Controller controller)
    {
        return controller.PartialViewToString(null, null);
    }

    public static string RenderPartialViewToString(this Controller controller, string viewName)
    {
        return controller.PartialViewToString(viewName, null);
    }

    public static string RenderPartialViewToString(this Controller controller, object model)
    {
        return controller.PartialViewToString(null, model);
    }

    public static string PartialViewToString(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
        }

        controller.ViewData.Model = model;

        using (var stringWriter = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
            viewResult.View.Render(viewContext, stringWriter);
            return stringWriter.GetStringBuilder().ToString();
        }
    }
}