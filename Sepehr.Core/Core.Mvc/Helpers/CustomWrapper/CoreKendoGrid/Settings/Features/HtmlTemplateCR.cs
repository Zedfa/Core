using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{

    
    /// <summary>
    /// مربوط به امور تمپلیت 
    /// </summary>
    [Serializable()]
    public class HtmlTemplateCr  : IUrlBuilderCr
    {
        [NonSerialized]
        private HtmlHelper _correspondingHtmlHelper;
        public Type PartialViewModel { get; set; }

        public string ControllerName
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        
        public HtmlHelper CorrespondingHtmlHelper { get { return _correspondingHtmlHelper; } set { _correspondingHtmlHelper = value; } }
        /// <summary>
        /// Relative path of partial View from which the partial view is generated or presented as a view to client.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the templeateId based on the relative Url of specified partial view
        /// </summary>
        public string TemplateID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Url))
                {
                    return Path.GetFileNameWithoutExtension(this.Url);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
