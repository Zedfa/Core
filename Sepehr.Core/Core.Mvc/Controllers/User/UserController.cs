using System.Linq;
using System.Web.Mvc;
using Core.Mvc.Controller;
using Core.Service;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Core.Mvc.ViewModel;
using Core.Ef.Exceptions;

namespace Core.Mvc.Controllers
{
    public class UserController : ControllerBaseCr
    {
        private IUserService _userService;

        public UserController(IUserService  userService)
        {
            _userService = userService;
        }

       
        public ActionResult Index()
        {
            return View();
           
        }





        public JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            
            var users = _userService.All().ToList();

            DataSourceResult result = users.ToDataSourceResult(request, rUser => new UserViewModel(rUser )
            {
               

            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var addedUser = _userService.Create(userViewModel.GetModel());
                    userViewModel.SetModel(addedUser );
                 

                }
                catch (DbEntityValidationExceptionBase ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

          
            return Json(new[] { userViewModel }.ToDataSourceResult(request, ModelState));
        }

        

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var updatedUser = _userService.Update(userViewModel.GetModel());
            }
            // Return the updated product. Also return any validation errors.
            return Json(new[] { userViewModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, UserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
               
                var deleteUser = _userService.Delete(userViewModel.GetModel());

            }
            // Return the updated product. Also return any validation errors.
            return Json(new[] { userViewModel }.ToDataSourceResult(request, ModelState));
        }



    }
}
