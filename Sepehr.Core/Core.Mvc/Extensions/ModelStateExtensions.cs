using System.Web.Http.ModelBinding;

namespace Core.Mvc.Extensions
{
    public static class ModelStateExtensions
    {
        //remark: developers use for show exceptions in "api controller"
        public static void AddModelHandledError(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState.AddModelError(key, errorMessage);
            modelState.AddModelError("isHandled", "true");
        }
        

        //remark: developers use for show exceptions in "controller"
        public static void AddModelHandledError(this System.Web.Mvc.ModelStateDictionary modelState, string key, string errorMessage)
        {
            modelState.AddModelError(key, errorMessage);
            modelState.AddModelError("isHandled", "true");
        }

       
    }
}
