using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Routing;


namespace Core.Mvc.Helpers.CustomWrapper.DataSource
{
    [Serializable()]
    public class OperationBase : INavigatable
    {
        private string routeName;
        private string controllerName;
        private string actionName;
        public  string IdName { get; set; }


        public OperationBase()
        {
            Params = new Dictionary<string, object>();
            Data = new ClientHandlerDescriptor();
            DataType = "JSON";
        }

        public string Encode(string value)
        {            
            value = Regex.Replace(value, "(%20)*%23%3D(%20)*", "#=", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, "(%20)*%23(%20)*", "#", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, "(%20)*%24%7B(%20)*", "${", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, "(%20)*%7D(%20)*", "}", RegexOptions.IgnoreCase);
            return value;
        }

       

        public string DataType { get; set; }               

        public string ActionName
        {
            get
            {
                return actionName;
            }
            set
            {

                actionName = value;
                routeName = null;
            }
        }

        public string ControllerName
        {
            get
            {
                return controllerName;
            }
            set
            {
                controllerName = value;
                routeName = null;
            }
        }

        public ClientHandlerDescriptor Data { get; set; }

        public RouteValueDictionary RouteValues
        {
            get;
            set;
        }

        public  Dictionary<string,object> Params
        {
            get;
            set;
        }

        public string ReadFilterObject { get; set; }

        public string ReadFunction { get; set; }

        public string RouteName
        {
            get
            {
                return routeName;
            }
            set
            {
                routeName = value;
                controllerName = actionName = null;
            }
        }

        public string Url
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}

