using Core.Entity;
using Core.Mvc.ApiControllers;
using Core.Mvc.Controller;
using Core.Mvc.Infrastructure;
using Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Core.UnitTesting.Mvc
{
    [TestClass()]
    public class AccountApiControllerTest: ApiControllerBase
    {

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Core.Cmn.AppBase.StartApplication();
        }
       
        [TestMethod()]
        public void PostEntity_False_Test()
        {
            // Arrange
            var authentication = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IAuthentication>();
            var viewElementRoleService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IViewElementRoleService>();
            var companyChartService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<ICompanyChartService>();
            var userService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IUserService>();
            var coreUserLogService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IServiceBase<CoreUserLog>>();
            var userProfileService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IUserProfileService>();
            var constantService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IConstantService>();
            var domainAuthenticationService = Core.Cmn.AppBase.DependencyInjectionManager.Resolve<IDomainAuthenticationService>();


            var controller = new AccountApiController(authentication, viewElementRoleService, companyChartService, userService, coreUserLogService, userProfileService, constantService, domainAuthenticationService);
            controller.Request = new HttpRequestMessage {
                RequestUri = new Uri("http://localhost:15660/api/AccountApi/PostEntity"),
                Method = new HttpMethod("POST"),
                Content = new StringContent("UserName\":\"admin\", \"Password\":\"98989\", \"RememberMe\":false, \"HiddenId\":\"b00efd89ff88cf37b6f350c563f649dc6d13593f\", \"CaptchaCode\":\"7894\", \"Domain\":\"\" "
                                , UTF8Encoding.UTF8, "application/json")
            };
        
            controller.Configuration = new HttpConfiguration();
            var token = new System.Threading.CancellationToken();
            controller.ExecuteAsync(controller.ControllerContext, token);

            // Act
            var response = controller.PostEntity(new Core.Mvc.ViewModel.Account.LogOnViewModel
            {
                UserName = "admin",
                Password = "111111",
                RememberMe = false,
                HiddenId = "44ef4a84913c86839c4def0e52cbcf3e9b336b1b",
                CaptchaCode = "656656",
                Domain = string.Empty
            });
            // Assert
            Assert.IsFalse(response.IsSuccessStatusCode);

        }
    }
}
