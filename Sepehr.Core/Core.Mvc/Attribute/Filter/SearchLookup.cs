using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Attribute.Filter
{
    public class SearchLookupAttribute : System.Attribute 
    {

        //type, title, viewModelName, ViewInfoName, lookupName, displayName, valueName, bindingName, isMultiselect,
        /// <summary>
        /// Type of the lookup('Grid' or 'Tree')
        /// </summary>
        public string LookupType { get; set; }
        /// <summary>
        /// Title of the window whithin which the content of the lookup is displayed.
        /// </summary>
        public string LookupTitle { get; set; }
        /// <summary>
        /// Fully qualified name of the ViewModel type within current executing assembly, and which is navigated through an outer ViewModel.
        /// </summary>
        public string ViewModelName { get; set; }
        /// <summary>
        /// Static property which gives the Grid information of the navigating lookup property.Used to fill the grid of the lookup which is being built.
        /// </summary>
        public string ViewModelGridInfoName { get; set; }
        /// <summary>
        ///An arbitrary name for the navigating lookup property.
        /// </summary>
        public string LookupName { get; set; }
        /// <summary>
        /// Name of the property of the ViewModel which is bound to the navigating lookup property. 
        /// </summary>
        public string NavigateViewModelDisplayName { get; set; }
        /// <summary>
        /// Value of the property of the ViewModel which is bound to the navigating lookup property. 
        /// </summary>
        public string NavigateViewModelValueName { get; set; }
        /// <summary>
        /// Name of the property within the current ViewModel type
        /// </summary>
        public string ModelBindingName { get; set; }
        /// <summary>
        /// Deifnes whether the lookup is in Multi Selection Mode , regardless of the type assigned to it('Grid' or 'Tree').
        /// </summary>
        public bool   IsMultiSelect { get; set; }

        public string NavigatePropertyUnderlayingModelName { get; set; }

        public string NavigatePropertyUnderlayingModelIdName { get; set; }

    }
}
