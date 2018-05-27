using System.Net;
using System.Net.Http.Headers;

namespace Core.Cmn.WebApi.Base
{
    public class WebApiHelperResponse
    {
        public string Content { get; set; }

        public HttpResponseHeaders Headers { get; set; }

        public CookieCollection Cookies { get; set; }
    }
}