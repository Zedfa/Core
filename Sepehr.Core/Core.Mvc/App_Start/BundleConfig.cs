using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Core.Mvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles, string relativePath = "~/")
        {
            //bundles.Add(new StyleBundle("~/core/basicExtensions").Include(
            //       relativePath + "Scripts/basicExtensions/stringEx.js"
            //    ));

            bundles.Add(new StyleBundle("~/Content/core/css/kendo").Include(
            relativePath + "Content/kendo/kendo.common.css",
            relativePath + "Content/kendo/kendo.default.css",
            relativePath + "Content/kendo/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/core/css/kendo/fa").Include(
                relativePath + "Content/kendo/kendo.common.css",
                relativePath + "Content/kendo/kendo.default.css",
                relativePath + "Content/kendo/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/core/css/kendo/en").Include(
                relativePath + "Content/kendo/kendo.common.css",
                relativePath + "Content/kendo/kendo.default.css"));

            bundles.Add(new StyleBundle("~/Content/core/css/kendo/ar").Include(
                relativePath + "Content/kendo/kendo.common.css",
                relativePath + "Content/kendo/kendo.default.css",
                relativePath + "Content/kendo/kendo.rtl.css"));

            bundles.Add(new StyleBundle("~/bundles/core/jquery-ui/css").Include(
                relativePath + "Content/jquery-ui/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/bundles/core/css").Include(
                relativePath + "Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundles/core/responsiveBoilerplate/fa")
                .Include(relativePath + "Content/bootstrap/bootstrap.css")
                .Include(relativePath + "Content/bootstrap/bootstrap-rtl.css"));

            bundles.Add(new StyleBundle("~/bundles/core/responsiveBoilerplate/ar")
                .Include(relativePath + "Content/bootstrap/bootstrap.css")
                .Include(relativePath + "Content/bootstrap/bootstrap-rtl.css"));

            bundles.Add(new StyleBundle("~/bundles/core/responsiveBoilerplate/en")
                .Include(relativePath + "Content/bootstrap/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/core/tools")
                .Include(relativePath + "Scripts/tools/DialogBox.js")
                .Include(relativePath + "Scripts/tools/Captcha.js")
                .Include(relativePath + "Scripts/tools/LookUp.js")
                .Include(relativePath + "Scripts/tools/Template.js")
                .Include(relativePath + "Scripts/tools/UserTabCookie.js")
                .Include(relativePath + "Scripts/tools/Tree.js")
                .Include(relativePath + "Scripts/lib/underscore.js")
                .Include(relativePath + "Scripts/tools/Grid.js")
                .Include(relativePath + "Scripts/tools/GridSearch.js")
                .Include(relativePath + "Scripts/tools/ngGridSearch.js")
                .Include(relativePath + "Scripts/tools/site-check-version.js")
                .Include(relativePath + "Scripts/tools/FilterDataSource.js")
                .Include(relativePath + "Scripts/tools/GeneralTools.js")
                .Include(relativePath + "Scripts/tools/sepehrappmodule.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/basicExtensions").Include(
                               relativePath + "Scripts/basicExtensions/stringEx.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/core/general-tools")
                .Include(relativePath + "Scripts/tools/site-check-version.js")
                .Include(relativePath + "Scripts/tools/GeneralTools.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/sepehrappmodule")
               .Include(relativePath + "Scripts/tools/sepehrappmodule.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/jquery")
                .Include(relativePath + "Scripts/lib/jquery-1.9.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/bootstrapjs")
                .Include(relativePath + "Scripts/lib/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/angular")
                .Include(relativePath + "Scripts/lib/angular.js")
                .Include(relativePath + "Scripts/lib/angular-cookies.js")
                .Include(relativePath + "Scripts/lib/angular-resource.js")
                .Include(relativePath + "Scripts/lib/angular-sanitize.js")
                .Include(relativePath + "Scripts/lib/angular-route.js")
               );

            bundles.Add(new ScriptBundle("~/bundles/core/zip")
                 .Include(relativePath + "Scripts/lib/jszip.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/kendo")
                .Include(relativePath + "Scripts/lib/kendo.all.js")
                .Include(relativePath + "Scripts/lib/kendo.aspnetmvc.js")
                .Include(relativePath + "Scripts/lib/kendo.culture.fa-IR.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/core/app/scripts/controllers")

                .Include(relativePath + "app/scripts/controllers/userRoleController.js")

                );

            bundles.Add(new ScriptBundle("~/bundles/core/directives")

                 .Include(relativePath + "Scripts/tools/app/directives/menuDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/stiReportViewerDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/kendoGridAngularAdapter.js")
                 .Include(relativePath + "Scripts/tools/app/directives/lookupDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/dropDownListDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/menuContainerDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/jqueryDateDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/dropDownListDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/captchaDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/priceFormatDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/notificationDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/validationDirective.js")
                 .Include(relativePath + "Scripts/tools/app/directives/sepehrViewDirective.js")
                 );

            bundles.Add(new ScriptBundle("~/bundles/core/controllers")
                .Include(relativePath + "Views/Account/_LogOn.js"));
            //.Include(relativePath + "Scripts/tools/app/controllers/roleController.js"));


            bundles.Add(new ScriptBundle("~/bundles/core/services")

                .Include(relativePath + "Scripts/tools/app/services/viewElementService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreMainService.js")
                .Include(relativePath + "Scripts/tools/app/services/gridSearchService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreAuthorizeService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreLoginService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreNotificationService.js")
                .Include(relativePath + "Scripts/tools/app/services/httpInterceptorService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreValidationService.js")
                .Include(relativePath + "Scripts/tools/app/services/dateInfoService.js")
                .Include(relativePath + "Scripts/tools/app/services/managePagesSevice.js")
                .Include(relativePath + "Scripts/tools/app/services/coreShortcutkeysService.js")
                .Include(relativePath + "Scripts/tools/app/services/coreAppInfoService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/core/generalServices")
                .Include(relativePath + "Scripts/tools/app/services/dateInfoService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/core/app/controllers")
                .Include(relativePath + "app/scripts/controllers/ChangeUserPasswordController.js")
                .Include(relativePath + "app/scripts/controllers/roleController.js")
                .Include(relativePath + "app/scripts/controllers/userRoleController.js")

                );

            bundles.Add(new ScriptBundle("~/bundles/core/app/services")
                 .Include(relativePath + "app/scripts/services/ChangeUserPasswordService.js"));

            bundles.Add(new ScriptBundle("~/bundles/core/app/directives")
                 .Include(relativePath + "app/scripts/directives/ConfirmPassCheckDirective.js"));


            bundles.Add(new ScriptBundle("~/bundles/core/app")
                .Include(relativePath + "Scripts/tools/app/coreApp.js"));


            bundles.Add(new ScriptBundle("~/bundles/core/startup")
                .Include(relativePath + "Scripts/tools/TopMainMenu.js",
                         relativePath + "Views/Shared/_Layout.js",
                         relativePath + "Views/Account/_LogOn.js"));


            //"ui.core.min.js"
            //"ui.datepicker-cc.min.js"
            //"calendar.min.js"
            //"ui.datepicker-cc-fa.js"

            bundles.Add(new ScriptBundle("~/bundles/core/datepicker/jquery").Include(
                relativePath + "Scripts/lib/DatePicker/jquery.ui.core.js",
                relativePath + "Scripts/lib/DatePicker/jquery.ui.datepicker-cc.all.min.js",
                relativePath + "Scripts/lib/DatePicker/calendar.js",
                relativePath + "Scripts/lib/DatePicker/jquery.ui.datepicker-cc-fa.js"));



            bundles.Add(new ScriptBundle("~/bundles/core/customTypes")
                .Include(relativePath + "Scripts/custom-types/browsers.js",
                relativePath + "Scripts/custom-types/coreViewElementInfo.js",
                relativePath + "Scripts/custom-types/notifications.js",
                relativePath + "Scripts/custom-types/validators.js",
                relativePath + "Scripts/custom-types/changeUserPassword.js"
                ));

            bundles.Add(GetExternalScriptBundle());


            bundles.IgnoreList.Ignore("*.min.js.map", OptimizationMode.Always);
            bundles.IgnoreList.Ignore("*.js.map", OptimizationMode.Always);
            bundles.IgnoreList.Ignore("*.ts", OptimizationMode.Always);




        }

        private static ScriptBundle GetExternalScriptBundle()
        {
            ScriptBundle result = new ScriptBundle("~/ExternalScriptBundle");
            try
            {
                //var dllName = Core.Cmn.ConfigHelper.GetConfigValue<string>("StartupProjectForWebClient");
                var assembly = AppBase.StartupProject; //Assembly.LoadFile(String.Format("{0}\\bin\\{1}.dll", AppDomain.CurrentDomain.BaseDirectory, dllName));
                Type method = assembly.GetType(String.Format("{0}.BundleConfig", AppBase.StartupProject.GetName().Name), false, true);
                if (method != null)
                {
                    if (method.GetMethod("GetScriptUrlsForCore") != null)
                    {
                        List<string> response = (List<string>)method.GetMethod("GetScriptUrlsForCore").Invoke(assembly, null);
                        if (response != null)
                        {
                            response.ForEach(r =>
                            {
                                result.Include(r);
                            });
                        }
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
