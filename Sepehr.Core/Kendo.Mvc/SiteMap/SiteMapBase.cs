namespace Kendo.Mvc
{
    using System.ComponentModel;

    public abstract class SiteMapBase
    {
        private static float defaultCacheDurationInMinutes = 0;
        private static bool defaultCompress = true;
        private static bool defaultGenerateSearchEngineMap = true;

        private float cacheDurationInMinutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapBase"/> class.
        /// </summary>
        protected SiteMapBase()
        {
            CacheDurationInMinutes = DefaultCacheDurationInMinutes;
            Compress = DefaultCompress;
            GenerateSearchEngineMap = DefaultGenerateSearchEngineMap;

            RootNode = new SiteMapNode();
        }

        /// <summary>
        /// Gets or sets the default cache duration in minutes.
        /// </summary>
        /// <value>The default cache duration in minutes.</value>
        public static float DefaultCacheDurationInMinutes
        {
            get
            {
                return defaultCacheDurationInMinutes;
            }

            set
            {

                defaultCacheDurationInMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [default compress].
        /// </summary>
        /// <value><c>true</c> if [default compress]; otherwise, <c>false</c>.</value>
        public static bool DefaultCompress
        {
            get
            {
                return defaultCompress;
            }

            set
            {
                defaultCompress = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [default generate search engine map].
        /// </summary>
        /// <value>
        /// <c>true</c> if [default generate search engine map]; otherwise, <c>false</c>.
        /// </value>
        public static bool DefaultGenerateSearchEngineMap
        {
            get
            {
                return defaultGenerateSearchEngineMap;
            }

            set
            {
                defaultGenerateSearchEngineMap = value;
            }
        }

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        public SiteMapNode RootNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cache duration in minutes.
        /// </summary>
        /// <value>The cache duration in minutes.</value>
        public float CacheDurationInMinutes
        {
            get
            {
                return cacheDurationInMinutes;
            }

            set
            {

                cacheDurationInMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SiteMapBase"/> is compress.
        /// </summary>
        /// <value><c>true</c> if compress; otherwise, <c>false</c>.</value>
        public bool Compress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [generate search engine map].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generate search engine map]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateSearchEngineMap
        {
            get;
            set;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Kendo.Mvc.SiteMapBase"/> to <see cref="Kendo.Mvc.SiteMapBuilder"/>.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SiteMapBuilder(SiteMapBase siteMap)
        {

            return siteMap.ToBuilder();
        }

        /// <summary>
        /// Returns a new builder.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SiteMapBuilder ToBuilder()
        {
            return new SiteMapBuilder(this);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public virtual void Reset()
        {
            CacheDurationInMinutes = DefaultCacheDurationInMinutes;
            Compress = DefaultCompress;
            GenerateSearchEngineMap = DefaultGenerateSearchEngineMap;

            RootNode = new SiteMapNode();
        }
    }
}