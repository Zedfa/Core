using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Kendo.Mvc;
using System.Collections.Generic;

namespace Core.Mvc.Helpers
{
    public class TreeViewTransport : TransportBase
    {
        protected override void Serialize(IDictionary<string, object> json)
        {
            base.Serialize(json);
         
            json["parameterMap"] = new ClientHandlerDescriptor { TemplateDelegate = obj => "function (data, operation) {  return Template.setParams(data,operation); }" };

        }
       
    }
}
