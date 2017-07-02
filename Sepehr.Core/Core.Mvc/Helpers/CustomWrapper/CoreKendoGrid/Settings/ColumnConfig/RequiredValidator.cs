using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.Helpers.CustomWrapper.CoreKendoGrid.Settings.ColumnConfig
{
    [Serializable]
    public class RequiredValidator : JsonObjectBase , IValidationRule
    {
        public RequiredValidator(string msg)
        {
            Message = msg;
        }
        public RequiredValidator()
        {
            Message = "لطفا فیلد مورد نظر را پر کنید";
        }
        public string Message { get; private set; }


        protected override void Serialize(IDictionary<string, object> json)
        {
            json["message"] = Message;
        }
    }
}