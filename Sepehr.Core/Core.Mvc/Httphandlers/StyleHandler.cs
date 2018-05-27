using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Core.Mvc.HttpHandlers
{
    public class StyleHandler : IHttpHandler
    {
        public bool IsReusable => false;
        private readonly List<string> languages = new List<string> { "fa", "en", "ar" };
        public void ProcessRequest(HttpContext context)
        {
            //check request from the same domain
            if (context.Request.UrlReferrer?.Authority == context.Request.Url.Authority)
            {
                var cssFilePath = context.Server.MapPath(context.Request.Url.AbsolutePath);
              
                if (!File.Exists(cssFilePath))
                {
                    string currentLang = context.Request.Url.AbsolutePath.Replace(context.Request.Url.Authority, "").Split('/')[0];

                    if (string.IsNullOrEmpty(currentLang) || !languages.Exists(item => item.Equals(currentLang)))
                        currentLang = languages.First();

                    var pathByLang = context.Request.Url.AbsolutePath.Replace(".css", $".{currentLang}.css");
                    cssFilePath = context.Server.MapPath(pathByLang);                  
                   
                }
                    WriteCssFileInResponse(context.Response, cssFilePath);                          


            }
        }

        private void WriteCssFileInResponse(HttpResponse response, string cssRelativePath)
        {
            if (File.Exists(cssRelativePath))
            {
                string cssFile = File.ReadAllText(cssRelativePath);

                byte[] cssFileBytes = new System.Text.UTF8Encoding().GetBytes(cssFile);
                response.OutputStream.Write(cssFileBytes, 0, cssFileBytes.Length);
                response.ContentType = "text/css";
                response.OutputStream.Flush();
            }
        }
    }
}