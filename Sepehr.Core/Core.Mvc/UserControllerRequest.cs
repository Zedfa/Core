using Core.Cmn;
using Core.Cmn.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc
{
    public class UserControllerRequest : IUserRequest
    {
        public UserControllerRequest(HttpRequestBase httpRequestBase)
        {
            HttpRequest = httpRequestBase;
        }
      
        public HttpRequestBase HttpRequest { get; set; }
        private string _data;
        public string Data
        {
            get
            {
                if (string.IsNullOrEmpty(_data))
                    //todo : it's dangerous for those contents that are too large (like files )
                    _data = new System.IO.StreamReader(HttpRequest.InputStream).ReadToEnd();
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
                    _ip = HttpRequest.UserHostAddress;
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
                    _method = HttpRequest.HttpMethod;
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
                    _url = HttpRequest.Url.AbsoluteUri;
                return _url;
            }

            set
            {
                _url = value;
            }
        }
    }
}