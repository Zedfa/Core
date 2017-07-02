using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Cmn.Extensions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace Core.Mvc.Helpers
{
    public abstract class TreeViewBase
    {
        internal static readonly string DeferredScriptsKey = "$DeferredScriptsKey$";
        private string name;

        protected TreeViewBase(ViewContext viewContext, ViewDataDictionary viewData = null)
        {
            this.ViewContext = viewContext;

            ViewData = viewData ?? viewContext.ViewData;
           
           
            this.HtmlAttributes = new RouteValueDictionary();
            this.IsSelfInitialized = true;
            this.Events = new Dictionary<string, object>();
        }
                
        protected TreeViewBase(System.Web.Mvc.ViewContext viewContext, Kendo.Mvc.Infrastructure.IJavaScriptInitializer initializer, ViewDataDictionary viewData = null)
            : this(viewContext, viewData)
        {
            this.Initializer = initializer;
        }

        private void AppendScriptToContext(string script)
        {
            IDictionary items = this.ViewContext.HttpContext.Items;
            string str = string.Empty;
            if (items.Contains(DeferredScriptsKey))
            {
                str = (string)items[DeferredScriptsKey];
            }
            items[DeferredScriptsKey] = str + script;
        }

        public void Render()
        {
            using (HtmlTextWriter writer = new HtmlTextWriter(this.ViewContext.Writer))
            {
                this.WriteHtml(writer);
            }
        }

        public MvcHtmlString ToClientTemplate()
        {
            this.IsInClientTemplate = true;
            return MvcHtmlString.Create(this.ToHtmlString().Replace("</script>", @"<\/script>"));
        }

        public string ToHtmlString()
        {
            using (StringWriter writer = new StringWriter())
            {
                this.WriteHtml(new HtmlTextWriter(writer));
                return writer.ToString();
            }
        }

        public virtual void VerifySettings()
        {
            var _constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
           

            if (!this.Name.HasValue())
            {
                var msg = string.Empty;
                _constantService.TryGetValue<string>("NameCantBeEmpty", out msg);
                throw new InvalidOperationException(msg/*Core.Resources.ExceptionMessage.NameCantBeEmpty*/);
            }
            if (!this.Name.Contains("<#=") && (this.Name.IndexOf(" ") != -1))
            {
                var msg = string.Empty;
                _constantService.TryGetValue<string>("NameCannotContainsWhiteSpace", out msg);
                throw new InvalidOperationException(msg/*Core.Resources.ExceptionMessage.NameCannotContainsWhiteSpace*/);
            }

        }

        protected virtual void WriteDeferredScriptInitialization()
        {
            StringWriter writer = new StringWriter();
            this.WriteInitializationScript(writer);
            this.AppendScriptToContext(writer.ToString());
        }

        protected virtual void WriteHtml(HtmlTextWriter writer)
        {
            this.VerifySettings();
            if (this.IsSelfInitialized)
            {
                if (this.HasDeferredInitialization)
                {
                    this.WriteDeferredScriptInitialization();
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);
                    this.WriteInitializationScript(writer);
                    writer.RenderEndTag();
                }
            }
        }

        public virtual void WriteInitializationScript(TextWriter writer)
        {
        }

        internal IDictionary<string, object> Events { get; set; }

        public bool HasDeferredInitialization { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; private set; }

        public string Id
        {
            get
            {
                return HtmlModifier.SanitizeId((!this.HtmlAttributes.ContainsKey("id") ? this.Name : ((string)this.HtmlAttributes["id"])));
            }
        }

        public Kendo.Mvc.Infrastructure.IJavaScriptInitializer Initializer { get; set; }

        public bool IsInClientTemplate { get; private set; }

        public bool IsSelfInitialized { get; set; }

        public System.Web.Mvc.ModelMetadata ModelMetadata { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Selector
        {
            get
            {
                return ((!this.IsInClientTemplate ? "#" : @"\#") + this.Id);
            }
        }

        public System.Web.Mvc.ViewContext ViewContext { get; private set; }

        public ViewDataDictionary ViewData { get; private set; }

        
    }
}
