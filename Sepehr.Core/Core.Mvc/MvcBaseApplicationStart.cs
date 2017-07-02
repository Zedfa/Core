using Core.Cmn;
using Core.Cmn.Cache;
using Core.Rep;
using Core.Service;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;

[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Core.Mvc.MvcBaseApplicationStart), "End")]

namespace Core.Mvc
{
    public class MvcBaseApplicationStart : ApplicationStartBase
    {

        public override void OnApplicationStart()
        {
            SetDependencyInjectionConfig();
        }

        private static void SetDependencyInjectionConfig()
        {
            GlobalConfiguration.Configuration.DependencyResolver = Core.Cmn.AppBase.DependencyInjectionManager.GetDependencyResolverForWebApi() as System.Web.Http.Dependencies.IDependencyResolver;
            DependencyResolver.SetResolver(Core.Cmn.AppBase.DependencyInjectionManager.GetDependencyResolverForMvc() as System.Web.Mvc.IDependencyResolver);
        }

        public override Enum ExecutionPriorityOnApplicationStart
        {
            get
            {
                return Core.Cmn.ExecutionPriorityOnApplicationStart.CreateInstanceDIFactory;
            }
        }

        public static void End()
        {
            System.Web.ApplicationShutdownReason shutdownReason = System.Web.Hosting.HostingEnvironment.ShutdownReason;
            string shutdownDetail = "";

            //Evaluate which option caused the error
            switch (shutdownReason)
            {
                case ApplicationShutdownReason.BinDirChangeOrDirectoryRename:
                    shutdownDetail = "A change was made to the bin directory or the directory was renamed";
                    break;
                case ApplicationShutdownReason.BrowsersDirChangeOrDirectoryRename:
                    shutdownDetail = "A change was made to the App_browsers folder or the files contained in it";
                    break;
                case ApplicationShutdownReason.ChangeInGlobalAsax:
                    shutdownDetail = "A change was made in the global.asax file";
                    break;
                case ApplicationShutdownReason.ChangeInSecurityPolicyFile:
                    shutdownDetail = "A change was made in the code access security policy file";
                    break;
                case ApplicationShutdownReason.CodeDirChangeOrDirectoryRename:
                    shutdownDetail = "A change was made in the App_Code folder or the files contained in it";
                    break;
                case ApplicationShutdownReason.ConfigurationChange:
                    shutdownDetail = "A change was made to the application level configuration";
                    break;
                case ApplicationShutdownReason.HostingEnvironment:
                    shutdownDetail = "The hosting environment shut down the application";
                    break;
                case ApplicationShutdownReason.HttpRuntimeClose:
                    shutdownDetail = "A call to Close() was requested";
                    break;
                case ApplicationShutdownReason.IdleTimeout:
                    shutdownDetail = "The idle time limit was reached";
                    break;
                case ApplicationShutdownReason.InitializationError:
                    shutdownDetail = "An error in the initialization of the AppDomain";
                    break;
                case ApplicationShutdownReason.MaxRecompilationsReached:
                    shutdownDetail = "The maximum number of dynamic recompiles of a resource limit was reached";
                    break;
                case ApplicationShutdownReason.PhysicalApplicationPathChanged:
                    shutdownDetail = "A change was made to the physical path to the application";
                    break;
                case ApplicationShutdownReason.ResourcesDirChangeOrDirectoryRename:
                    shutdownDetail = "A change was made to the App_GlobalResources foldr or the files contained within it";
                    break;
                case ApplicationShutdownReason.UnloadAppDomainCalled:
                    shutdownDetail = "A call to UnloadAppDomain() was completed";
                    break;
                default:
                    shutdownDetail = "Unknown shutdown reason";
                    break;
            }

            var eLog = Core.Cmn.AppBase.LogService.GetEventLogObj();
            eLog.OccuredException = null;
            eLog.UserId = "Core";
            eLog.CustomMessage = "Application_End => Cause: " + shutdownDetail;
            Core.Cmn.AppBase.LogService.Handle(eLog);
        }
    }
}