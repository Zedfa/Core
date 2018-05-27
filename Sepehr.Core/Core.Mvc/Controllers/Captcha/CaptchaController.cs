using Core.Cmn;
using Core.Mvc.Controller;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Core.Mvc.Controllers
{
    public class CaptchaController : ControllerBaseCr
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult GetCaptchaImage(string guid)
        {
            Bitmap image = Core.Mvc.Helpers.Captcha.CaptchaControl.CreateControl(true, Core.Mvc.Helpers.Captcha.CaptchaControl.CharSets.Numbers, 5, 145, 40, guid).GetImage();

            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            image.Dispose();

            return View("_CaptchaImage");
        }


        public bool ValidateCaptcha(string value, string guid)
        {
            return Core.Mvc.Helpers.Captcha.CaptchaControl.Validate(value, guid);
        }


        public ActionResult Validate(string value, string guid)
        {
           var _constantService = AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
            var msg = string.Empty;
            _constantService.TryGetValue<string>("IncorrectSecurityCode", out msg);
            if (!Core.Mvc.Helpers.Captcha.CaptchaControl.Validate(value, guid))
                return ShowException(new MvcExceptionInfo(msg/*ExceptionMessage.IncorrectSecurityCode*/));
            else
                return null;

        }
    }
}