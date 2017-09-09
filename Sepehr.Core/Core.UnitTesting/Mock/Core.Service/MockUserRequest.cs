using Core.Cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitTesting.Mock
{
    class MockUserRequest : IUserRequest
    {
        public MockUserRequest()
        {
            
        }
       

        private string _data;
        public string Data
        {
            get
            {
                if (string.IsNullOrEmpty(_data))
                    _data = "{\"UserName\":\"admin\",\"Password\":\"123\",\"RememberMe\":false,\"HiddenId\":\"3a219d4b1be5e870c4b78a2eebd510a964370e5e\",\"CaptchaCode\":\"2321\",\"Domain\":\"\"}";
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
                    _ip = "::1";
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
                    _method = "POST";
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
                    _url = "http://localhost:15660/api/AccountApi/PostEntity";
                return _url;
            }

            set
            {
                _url = value;
            }
        }
    }
}