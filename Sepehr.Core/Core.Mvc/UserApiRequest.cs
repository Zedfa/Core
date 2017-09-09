using Core.Cmn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Core.Mvc
{
    public class UserApiRequest : IUserRequest
    {
        public UserApiRequest(HttpRequestMessage httpRequest)
        {
            HttpRequest = httpRequest;
        }
        public HttpRequestMessage HttpRequest { get; set; }

        private HttpContextBase Context
        {
            get
            {
                return ((HttpContextBase)HttpRequest.Properties["MS_HttpContext"]);
            }
        }
        private string _data;
        public string Data
        {
            get
            {
                if (string.IsNullOrEmpty(_data))

                {
                    //  _data = HttpRequest.Content.ReadAsStringAsync().Result;
                    using (var stream = new MemoryStream())
                    {
                        Context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                        Context.Request.InputStream.CopyTo(stream);
                        _data = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                    }
                }
                return _data;
            }

            set
            {
                _data = value;           
            }
        }


        private string _ip;
        public string IP
        {
            get
            {
                if (string.IsNullOrEmpty(_ip))
                    _ip = Context.Request.UserHostAddress;
                return _ip;
            }

            set
            {
                _ip = value;
            }
        }
        private string _method;
        public string Method
        {
            get
            {
                if (string.IsNullOrEmpty(_method))
                    _method = HttpRequest.Method.Method;
                return _method;
            }

            set
            {
                _method = value;
            }
        }
        private string _url;
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                    _url = HttpRequest.RequestUri.AbsoluteUri;
                return _url;
            }

            set
            {
                _url = value;
            }
        }
    }
}