namespace Kendo.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Routing;

    public class SiteMapNode : LinkedObjectBase<SiteMapNode>, INavigatable
    {
        private string title;
        private string routeName;
        private string controllerName;
        private string actionName;
        private string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNode"/> class.
        /// </summary>
        public SiteMapNode()
        {
            Visible = true;
            RouteValues = new RouteValueDictionary();
            IncludeInSearchEngineIndex = true;
            Attributes = new RouteValueDictionary();
            ChildNodes = new LinkedObjectCollection<SiteMapNode>(this);
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {

                title = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapNode"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last modified at.
        /// </summary>
        /// <value>The last modified at.</value>
        public DateTime? LastModifiedAt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        /// <value>The name of the route.</value>
        public string RouteName
        {
            get
            {
                return routeName;
            }

            set
            {

                routeName = value;
                controllerName = actionName = url = null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName
        {
            get
            {
                return controllerName;
            }

            set
            {

                controllerName = value;

                routeName = url = null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName
        {
            get
            {
                return actionName;
            }

            set
            {

                actionName = value;

                routeName = url = null;
            }
        }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public RouteValueDictionary RouteValues
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Url might not be a valid uri.")]
        public string Url
        {
            get
            {
                return url;
            }

            set
            {

                url = value;

                routeName = controllerName = actionName = null;
                RouteValues.Clear();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include in search engine index].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include in search engine index]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeInSearchEngineIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, object> Attributes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        public IList<SiteMapNode> ChildNodes
        {
            get;
            private set;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Kendo.Mvc.SiteMapNode"/> to <see cref="Kendo.Mvc.SiteMapNodeBuilder"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SiteMapNodeBuilder(SiteMapNode node)
        {

            return node.ToBuilder();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SiteMapNodeBuilder ToBuilder()
        {
            return new SiteMapNodeBuilder(this);
        }
    }
}