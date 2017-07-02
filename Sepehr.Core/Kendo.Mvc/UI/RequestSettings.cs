namespace Kendo.Mvc.UI
{
    using System.Web.Routing;

    public class RequestSettings : INavigatable
    {
        private string routeName;
        private string controllerName;
        private string actionName;

        public RequestSettings()
        {
            RouteValues = new RouteValueDictionary();
        }

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

        public RouteValueDictionary RouteValues
        {
            get;
            set;
        }

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
    }
}
