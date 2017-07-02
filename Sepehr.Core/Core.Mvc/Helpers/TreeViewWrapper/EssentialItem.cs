using Kendo.Mvc;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Core.Mvc.Helpers
{
    public class EssentialItem<T> :  LinkedObjectBase<T> where T:EssentialItem<T> 
    {
        private string actionName;
        private string controllerName;
        private bool enabled;
        private string routeName;
        private bool selected;
        private string text;
        private string url;

        protected EssentialItem()
        {
            this.Template = new HtmlTemplate();
            this.HtmlAttributes = new RouteValueDictionary();
            this.ImageHtmlAttributes = new RouteValueDictionary();
            this.RouteValues = new RouteValueDictionary();
            this.ContentHtmlAttributes = new RouteValueDictionary();
            this.Visible = true;
            this.Enabled = true;
            this.Encoded = true;
        }

        [ScriptIgnore]
        public string ActionName
        {
            get
            {
                return this.actionName;
            }
            set
            {
                this.actionName = value;
                this.routeName = (string) (this.url = null);
            }
        }

       
        [ScriptIgnore]
        public IDictionary<string, object> ContentHtmlAttributes { get; private set; }

        [ScriptIgnore]
        public string ControllerName
        {
            get
            {
                return this.controllerName;
            }
            set
            {
                this.controllerName = value;
                this.routeName = (string) (this.url = null);
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
                if (!this.enabled)
                {
                    this.selected = false;
                }
            }
        }

        public bool Encoded { get; set; }

        [ScriptIgnore]
        public IDictionary<string, object> LinkHtmlAttributes
        {
            get;
            private set;
        }

        [ScriptIgnore]
        public IDictionary<string, object> HtmlAttributes { get; private set; }

        [ScriptIgnore]
        public IDictionary<string, object> ImageHtmlAttributes { get; private set; }

        public string ImageUrl { get; set; }

       
        [ScriptIgnore]
        public string RouteName
        {
            get
            {
                return this.routeName;
            }
            set
            {
                this.routeName = value;
                this.controllerName = this.actionName = (string) (this.url = null);
            }
        }

        [ScriptIgnore]
        public RouteValueDictionary RouteValues { get; set; }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
                if (this.selected)
                {
                    this.enabled = true;
                }
            }
        }

        public string SpriteCssClasses { get; set; }

        [ScriptIgnore]
        public HtmlTemplate Template { get; private set; }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.routeName = this.controllerName = (string) (this.actionName = null);
                this.RouteValues.Clear();
            }
        }

        [ScriptIgnore]
        public bool Visible { get; set; }
    }
}
