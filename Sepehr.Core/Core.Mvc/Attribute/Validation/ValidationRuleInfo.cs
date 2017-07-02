using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Attribute.Validation
{
     [Serializable]
    public class ValidationRuleInfo: JsonObjectBase
    {
       
         public ValidationRuleInfo(string validationName, params string[] prms)
        {
            this.Params = new Dictionary<int, string>();
            
            for (int i = 0; i < prms.Length; i++)
            {
                 this.Params.Add(i, prms[i]);
            }

            Name = validationName;
        }
        public string Message { get; set; }
        public string Name { get; private set; }
        public Dictionary<int, string> Params { get; set; }

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["message"] = Message;
            json["params"] = this.Params;
        }
    }
}