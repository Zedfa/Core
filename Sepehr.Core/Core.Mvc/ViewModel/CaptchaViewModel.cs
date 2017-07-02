using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel
{
    [DataContract]
    public class CaptchaViewModel
    {
       [DataMember]
        public string Base64imgage { get; set; }
        [DataMember]
        public string EncryptedKey { get; set; }
    }
}