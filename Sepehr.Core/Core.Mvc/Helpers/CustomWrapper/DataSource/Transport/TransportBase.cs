using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Kendo.Mvc;
using Core.Cmn.Extensions;

namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    [Serializable()]
    public class TransportBase : JsonObject
    {
        public TransportBase()
        {
            Read = new OperationBase();

            Read.Type = "Get";
                  
            Update = new OperationBase();

            Update.Type = "Put";

            Destroy = new OperationBase();

            Destroy.Type = "Delete";

            Create = new OperationBase();

            Create.Type = "Post";
        }

        public new string Prefix { get; set; }

        
        protected override void Serialize(IDictionary<string, object> json)
        {
            var read = SerializeCRUDOperatonValues(this.Read);
            
            json["prefix"] = Prefix.HasValue() ? Prefix : string.Empty;            

            if (read.Keys.Any())
            {
                json["read"] = read;
            }

            var update = SerializeCRUDOperatonValues(this.Update);

            if (update.Keys.Any())
            {
                json["update"] = update;
            }

            var create = SerializeCRUDOperatonValues(this.Create);

            if (create.Keys.Any())
            {
                json["create"] = create;
            }

            var destroy = SerializeCRUDOperatonValues(this.Destroy);

            if (destroy.Keys.Any())
            {
                json["destroy"] = destroy;
            }

            json["cache"] = "inmemory";

            //json["parameterMap"] = new ClientHandlerDescriptor { TemplateDelegate = obj => "function (data, operation) { return JSON.stringify(data); }" };

            //if (!string.IsNullOrEmpty(this.Read.ReadFilterObject))
            //{
            //    json["parameterMap"] = new ClientHandlerDescriptor  //+ this.Read.ReadFilterObject +
            //    {
            //        TemplateDelegate = obj => " function(data, type) {  " +
            //                                             " if (type == 'read') { var value={ filters :kendo.stringify(" + this.Read.ReadFilterObject + ") }; return value;  " + ////return kendo.stringify(data);
            //                     "} }"

            //    };
            //}
        }

        public new OperationBase Read { get; private set; }

        public new OperationBase Update { get; private set; }

        public new OperationBase Create { get; private set; }

        public new OperationBase Destroy { get; private set; }

        public string EntityKeyName { get; set; }

        protected Dictionary<string, object> SerializeCRUDOperatonValues(OperationBase CrudOpt)
        {
            var json = new Dictionary<string, object>();

            if (CrudOpt.Url != null)
            {
                var urlStr = CrudOpt.Encode(CrudOpt.Url);
                if (CrudOpt.Type.HasValue())
                {
                   // if (CrudOpt.Type.ToLower().Equals("Get".ToLower()))
                   // {
                        //json["cache"] = true;
                   // }
                    

                    
                    if ((CrudOpt.Type.ToLower().Equals("Put".ToLower()) || CrudOpt.Type.ToLower().Equals("Delete".ToLower())) && !string.IsNullOrEmpty(EntityKeyName))
                    {
                        var retUrl = "function(entity) { return '" + urlStr + "/' + entity." + EntityKeyName + "; }";
                        json["url"] = new ClientHandlerDescriptor { TemplateDelegate = obj => retUrl };
                    }
                    if (!string.IsNullOrEmpty(CrudOpt.ReadFunction))
                    {
                        json["url"] = new ClientHandlerDescriptor { TemplateDelegate = obj=> CrudOpt.ReadFunction + "()" };
                    }
 
                    else
                    {
                        if (CrudOpt.Type.ToLower().Equals("Get".ToLower()) && CrudOpt.Params.Count > 0)
                        {
                            var qString = string.Empty;
                            foreach (var param in CrudOpt.Params)
                            {
                                qString += param.Key + "=" + param.Value + "&";
                            }

                            qString = qString.Remove(qString.Length - 1);
                            var retUrl = "function(entity) { return " + urlStr + "/?" + qString + "; }";
                            json["url"] = new ClientHandlerDescriptor { TemplateDelegate = obj => retUrl };

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(CrudOpt.ActionName))
                            {
                                //var retUrl = "eval('" + ActionName + "'); } ";
                                json["url"] = new ClientHandlerDescriptor { TemplateDelegate = obj => "eval('" + CrudOpt.ActionName + "();');" };
                            }
                            else
                            {
                                json["url"] = urlStr;
                            }
                        }
                    }
                }


                //else
                //{
                //    if (!string.IsNullOrEmpty(CrudOpt.ActionName))
                //    {
                //        json["url"] = new ClientHandlerDescriptor { TemplateDelegate = obj => "function() { return " + "" + CrudOpt.ActionName + "(); }" };
                //    }
                //}
                if (CrudOpt.DataType.HasValue())
                {
                    json["dataType"] = CrudOpt.DataType;
                }

                if (CrudOpt.Data.HasValue())
                {

                    json["data"] = CrudOpt.Data;
                }

                if (CrudOpt.Type.HasValue())
                {
                    json["type"] = CrudOpt.Type;
                }
            }
           
           
           
            return json;
        }

    }
}
