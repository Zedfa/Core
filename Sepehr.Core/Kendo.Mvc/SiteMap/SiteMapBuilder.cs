namespace Kendo.Mvc
{
    using System.ComponentModel;

    public class SiteMapBuilder : IHideObjectMembers
    {
        private readonly SiteMapBase siteMap;
        private readonly SiteMapNodeBuilder siteMapNodeBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapBuilder"/> class.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        public SiteMapBuilder(SiteMapBase siteMap)
        {

            this.siteMap = siteMap;
            siteMapNodeBuilder = new SiteMapNodeBuilder(this.siteMap.RootNode);
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <value>The root node.</value>
        public SiteMapNodeBuilder RootNode
        {
            get
            {
                return siteMapNodeBuilder;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Kendo.Mvc.SiteMapBuilder"/> to <see cref="Kendo.Mvc.SiteMapBase"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SiteMapBase(SiteMapBuilder builder)
        {

            return builder.ToSiteMap();
        }

        /// <summary>
        /// Returns the internal sitemap.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SiteMapBase ToSiteMap()
        {
            return siteMap;
        }

        /// <summary>
        /// Caches the duration in minutes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual SiteMapBuilder CacheDurationInMinutes(float value)
        {
            siteMap.CacheDurationInMinutes = value;

            return this;
        }

        /// <summary>
        /// Compresses the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public virtual SiteMapBuilder Compress(bool value)
        {
            siteMap.Compress = value;

            return this;
        }

        /// <summary>
        /// Generates the search engine map.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public virtual SiteMapBuilder GenerateSearchEngineMap(bool value)
        {
            siteMap.GenerateSearchEngineMap = value;

            return this;
        }
    }
}