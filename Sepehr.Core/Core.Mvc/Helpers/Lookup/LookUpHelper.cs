using Core.Cmn;
using Core.Entity;
using Core.Mvc.Extensions;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace Core.Mvc.Helpers
{
    /// <summary> 						            
    ///Created by :Tabandeh
    ///Created on Date :1392/5/16
    ///cerate lookup by providing textbox for display text and HiddenField for keeping value.
    ///lookup have two kind(tree, grid).look up result have two kind(textbox , multiselect).
    ///if developers need to set validation for lookup ,they can set validationCr array 
    /// </summary>
    public static class LookUpHelper
    {
        private static string _Id;

        private static string Id
        {
            get
            {
                return HtmlModifier.ModifyId(_Id);
            }
            set { _Id = value; }
        }

        private static string Title { get; set; }

        private static ConcurrentDictionary<string, GridInfo> _allGridLookups;

        public static ConcurrentDictionary<string, GridInfo> AllGridLookups
        {
            get
            {
                if (_allGridLookups == null)
                {
                    _allGridLookups = new ConcurrentDictionary<string, GridInfo>();

                }
                return _allGridLookups;
            }
            //set
            //{
            //    _allGridLookups = value;
            //}
        }
        private static ConcurrentDictionary<string, TreeInfo> _allTreeLookups;
        public static ConcurrentDictionary<string, TreeInfo> AllTreeLookups
        {
            get
            {
                if (_allTreeLookups == null)
                {
                    _allTreeLookups = new ConcurrentDictionary<string, TreeInfo>();

                }
                return _allTreeLookups;
            }
        }

        #region textbox for grid result

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo,
            string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, object htmlAttributes,
            bool readOnly, Dictionary<string, object> lookupHtmlAttributes, ClientDependentFeature clientDependentFeature, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {

            Id = id;
            // Title = title;

            TagBuilder container = new TagBuilder("span");
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                container.MergeAttributes(attributes);
            }

            TagBuilder containerState = new TagBuilder("span");


            MvcHtmlString textbox;
            lookupHtmlAttributes = lookupHtmlAttributes ?? new Dictionary<string, object>();

            //---value
            TagBuilder hiddenValue = new TagBuilder("input");
            hiddenValue.MergeAttribute("type", "hidden");
            hiddenValue.MergeAttribute("id", propertyNameForBinding);

            if (!string.IsNullOrEmpty(name))
            {
                hiddenValue.MergeAttribute("name", name);
            }

            hiddenValue.MergeAttribute("data-bind", string.Format("value:{0}", propertyNameForBinding));

            if (validationCr != null && validationCr.Count() > 0)
            {
                hiddenValue.MergeAttributes(CreateValidationForlookup(lookupHtmlAttributes, validationCr));
            }

            textbox = helper.TextBoxCr(Id, readOnly, lookupHtmlAttributes);


            string gridID = string.Format("lookupGrid_{0}", Id);
            ///we must new from static instance to set  info itself


            var info = gridInfo.DeepCopy<GridInfo>();
            info.GridID = gridID;
            info.ClientDependentFeatures = clientDependentFeature;

            // gridInfo.GridID = gridID;

            //gridInfo.ClientDependentFeatures = clientDependentFeature;


            //var gridInfoKey = string.Format("{0}_{1}", gridInfo.GetHashCode(), gridInfo.GridID);
            var gridInfoKey = string.Format("{0}_{1}", gridInfo.GetHashCode(), gridID);


            AllGridLookups.TryAdd(gridInfoKey, info);

            var lookupInfo = new Lookup.Grid
            {
                Title = title,

                LookupName = Id,

                GridID = info.GridID,

                ViewModel = typeof(TModel).AssemblyQualifiedName,

                // ViewInfoKey = viewInfoName,

                ViewInfoKey = gridInfoKey,

                UseMultiSelect = false,

                PropertyNameForDisplay = propertyNameForDisplay,

                PropertyNameForValue = propertyNameForValue,

                PropertyNameForBinding = propertyNameForBinding,

                ClientDependentFeatures = info.ClientDependentFeatures

            };

            //----create Lookup
            containerState.InnerHtml = textbox.ToHtmlString()

                + hiddenValue.ToString(TagRenderMode.SelfClosing)

                + CreatLookupButton(lookupInfo);

            container.InnerHtml = containerState.ToString();

            return MvcHtmlString.Create(container.ToString());


        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, null, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, null, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, ClientDependentFeature clientDependentFeature) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, null, clientDependentFeature, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes, ClientDependentFeature clientDependentFeature) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, clientDependentFeature, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, null, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, null, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null, null);
        }
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, null, null, null);
        }


        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, null, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, lookupTextBoxHtmlAttributes, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, Dictionary<string, object> lookupTextBoxHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, lookupTextBoxHtmlAttributes, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, null, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, null, null);
        }


        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, Dictionary<string, object> lookupTextBoxHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, lookupTextBoxHtmlAttributes, null, validationCr);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool readOnly, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, null, readOnly, lookupTextBoxHtmlAttributes, null, null);
        }
        #endregion

        #region multi select for grid result
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo
            , string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, object htmlAttributes, bool readOnly
            , Dictionary<string, object> lookupHtmlAttributes, ClientDependentFeature clientDependentFeature, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {

            Id = id;

            TagBuilder container = new TagBuilder("span");

            container.AddCssClass("rp-lookup");

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                container.MergeAttributes(attributes);
            }

            TagBuilder containerState = new TagBuilder("span");



            //-----multi select
            Kendo.Mvc.UI.Fluent.MultiSelectBuilder multiSelect = helper.MultiSelectCr(propertyNameForBinding, propertyNameForDisplay, propertyNameForValue, Kendo.Mvc.UI.FilterType.Contains);

            lookupHtmlAttributes = lookupHtmlAttributes ?? new Dictionary<string, object>();

            Dictionary<string, string> _htmlAttributes = null;

            if (validationCr != null && validationCr.Count() > 0)
            {

                _htmlAttributes = CreateValidationForlookup(lookupHtmlAttributes, validationCr);
            }

            else
            {
                _htmlAttributes = lookupHtmlAttributes.ToDictionary(t => t.Key, t => (string)t.Value);
            }

            if (readOnly)
            {
                _htmlAttributes.Add("readOnly", "true");
            }

            if (!string.IsNullOrEmpty(Id))
            {
                _htmlAttributes.Add("id", Id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                _htmlAttributes.Add("name", name);
            }

            multiSelect.HtmlAttributes(_htmlAttributes.ToDictionary(t => t.Key, t => (object)t.Value));

            var multiselectHtml = multiSelect.ToHtmlString();

            containerState.AddCssClass(multiselectHtml.Contains("data-val-required") ? StyleKind.RequiredInput : StyleKind.OptionalInput);


            //string gridID = string.Format("lookupGrid_{0}", Id);

            //gridInfo.GridID = gridID;

            //gridInfo.ClientDependentFeatures = clientDependentFeature;

            ////var gridInfoHashKey = gridInfo.GetHashCode();
            //// var gridInfohKey = new Guid();
            //var gridInfoKey = string.Format("{0}_{1}", gridInfo.GetHashCode(), gridInfo.GridID);
            //AllGridLookups.TryAdd(gridInfoKey, gridInfo);



            string gridID = string.Format("lookupGrid_{0}", Id);
            ///we must new from static instance to set  info itself


            var info = gridInfo.DeepCopy<GridInfo>();
            info.GridID = gridID;
            info.ClientDependentFeatures = clientDependentFeature;

            // gridInfo.GridID = gridID;

            //gridInfo.ClientDependentFeatures = clientDependentFeature;


            //var gridInfoKey = string.Format("{0}_{1}", gridInfo.GetHashCode(), gridInfo.GridID);
            var gridInfoKey = string.Format("{0}_{1}", gridInfo.GetHashCode(), gridID);


            AllGridLookups.TryAdd(gridInfoKey, info);


            var lookupInfo = new Lookup.Grid
            {
                Title = title,

                LookupName = Id,

                GridID = gridID,

                ViewModel = typeof(TModel).AssemblyQualifiedName,

                ViewInfoKey = gridInfoKey,

                UseMultiSelect = true,

                PropertyNameForDisplay = propertyNameForDisplay,

                PropertyNameForValue = propertyNameForValue,

                PropertyNameForBinding = propertyNameForBinding,

                ClientDependentFeatures = info.ClientDependentFeatures

            };

            containerState.InnerHtml = multiselectHtml
                                     + CreatLookupButton(lookupInfo);

            container.InnerHtml = containerState.ToString();



            return MvcHtmlString.Create(container.ToString());

        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, bool isMultiSelect, bool readOnly) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, id, isMultiSelect, null, readOnly, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, bool readOnly) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, isMultiSelect, null, readOnly, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, bool readOnly, Dictionary<string, object> lookupHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, isMultiSelect, null, readOnly, lookupHtmlAttributes, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, bool isMultiSelect, bool readOnly, Dictionary<string, object> lookupHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, id, isMultiSelect, null, readOnly, lookupHtmlAttributes, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, bool readOnly, Dictionary<string, object> lookupHtmlAttributes, ClientDependentFeature clientDependentFeature) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, isMultiSelect, null, readOnly, lookupHtmlAttributes, clientDependentFeature, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, bool readOnly, ClientDependentFeature clientDependentFeature) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, isMultiSelect, null, readOnly, null, clientDependentFeature, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, GridInfo gridInfo, string propertyNameForValue, string propertyNameForDisplay, string propertyNameForBinding, bool isMultiSelect, bool readOnly) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, gridInfo, propertyNameForValue, propertyNameForDisplay, propertyNameForBinding, isMultiSelect, null, readOnly, null, null, null);
        }

        #endregion

        #region text box for tree
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding
            , object htmlAttributes, bool readOnly, Dictionary<string, object> lookupHtmlAttributes, params  ValidationBase[] validationCr)
            where TModel : IViewModel, new()
        {

            Id = id;

            TagBuilder container = new TagBuilder("span");

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                container.MergeAttributes(attributes);
            }

            TagBuilder containerState = new TagBuilder("span");

            //-----textbox
            MvcHtmlString textbox;

            lookupHtmlAttributes = lookupHtmlAttributes ?? new Dictionary<string, object>();

            //---value
            TagBuilder hiddenValue = new TagBuilder("input");

            hiddenValue.MergeAttribute("type", "hidden");

            hiddenValue.MergeAttribute("id", propertyNameForBinding);

            if (!string.IsNullOrEmpty(name))
            {
                hiddenValue.MergeAttribute("name", name);
            }

            hiddenValue.MergeAttribute("data-bind", string.Format("value:{0}", propertyNameForBinding));

            if (validationCr != null && validationCr.Count() > 0)
            {
                hiddenValue.MergeAttributes(CreateValidationForlookup(lookupHtmlAttributes, validationCr));
            }

            textbox = helper.TextBoxCr(Id, readOnly, lookupHtmlAttributes);

            //string treeID = string.Format("lookupTree_{0}", Id);

            //treeInfo.Name = treeID;

            //var treeInfoHashKey = treeInfo.GetHashCode();

            //AllTreeLookups.TryAdd(treeInfoHashKey, treeInfo);

            string treeID = string.Format("lookupTree_{0}", Id);
            ///we must new from static instance to set  info itself
            var info = treeInfo.DeepCopy<TreeInfo>();
            info.Name = treeID;
            var treeInfoKey = string.Format("{0}_{1}", treeInfo.GetHashCode(), treeID);
            AllTreeLookups.TryAdd(treeInfoKey, info);

            var lookupInfo = new Lookup.Tree
            {
                Title = title,

                LookupName = Id,

                TreeID = treeID,

                ViewModel = typeof(TModel).AssemblyQualifiedName,

                ViewInfoKey = treeInfoKey,

                UseMultiSelect = false,

                PropertyNameForDisplay = treeInfo.DataTextField,

                PropertyNameForBinding = propertyNameForBinding

            };


            //-----create lookup----
            containerState.InnerHtml = textbox.ToHtmlString()
                + hiddenValue
                + CreatLookupButton(lookupInfo);

            container.InnerHtml = containerState.ToString();

            return MvcHtmlString.Create(container.ToString());

        }

        #region text box for tree overrides
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, TreeInfo treeInfo, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, treeInfo, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null);
        }


        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, treeInfo, propertyNameForBinding, null, false, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding, Dictionary<string, object> lookupTextBoxHtmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, treeInfo, propertyNameForBinding, null, false, lookupTextBoxHtmlAttributes, null);
        }


        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, TreeInfo treeInfo, string propertyNameForBinding) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, treeInfo, propertyNameForBinding, null, false, null, null);
        }
        #endregion

        #endregion

        #region multi select for tree
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding, bool isMultiSelect,
            object htmlAttributes, bool readOnly, Dictionary<string, object> lookupHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {

            Id = id;

            TagBuilder container = new TagBuilder("span");

            container.AddCssClass("rp-lookup");

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                container.MergeAttributes(attributes);
            }

            TagBuilder containerState = new TagBuilder("span");

            //-----MultiSelect

            Kendo.Mvc.UI.Fluent.MultiSelectBuilder multiSelect = helper.MultiSelectCr(propertyNameForBinding, treeInfo.DataTextField, treeInfo.DataSource.ModelCr.ModelIdName, Kendo.Mvc.UI.FilterType.Contains);

            lookupHtmlAttributes = lookupHtmlAttributes ?? new Dictionary<string, object>();

            Dictionary<string, string> _htmlAttributes = null;

            if (validationCr != null && validationCr.Count() > 0)
            {

                _htmlAttributes = CreateValidationForlookup(lookupHtmlAttributes, validationCr);
            }

            else
            {
                _htmlAttributes = lookupHtmlAttributes.ToDictionary(t => t.Key, t => (string)t.Value);
            }

            if (readOnly)
            {
                _htmlAttributes.Add("readOnly", "true");
            }

            if (!string.IsNullOrEmpty(Id))
            {
                _htmlAttributes.Add("id", Id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                _htmlAttributes.Add("name", name);
            }

            multiSelect.HtmlAttributes(_htmlAttributes.ToDictionary(t => t.Key, t => (object)t.Value));

            var multiSelectHtml = multiSelect.ToHtmlString();

            containerState.AddCssClass(multiSelectHtml.Contains("data-val-required") ? StyleKind.RequiredInput : StyleKind.OptionalInput);

            //string treeID = string.Format("lookupTree_{0}", Id);

            //treeInfo.Name = treeID;

            //var treeInfoHashKey = treeInfo.GetHashCode();

            //AllTreeLookups.TryAdd(treeInfoHashKey, treeInfo);

            string treeID = string.Format("lookupTree_{0}", Id);
            ///we must new from static instance to set  info itself
            var info = treeInfo.DeepCopy<TreeInfo>();
            info.Name = treeID;
            var treeInfoKey = string.Format("{0}_{1}", treeInfo.GetHashCode(), treeID);
            AllTreeLookups.TryAdd(treeInfoKey, info);


            var lookupInfo = new Lookup.Tree
            {
                Title = title,

                LookupName = Id,

                TreeID = treeID,
                // ViewModel = typeof(TModel).FullName,
                ViewModel = typeof(TModel).AssemblyQualifiedName,

                ViewInfoKey = treeInfoKey,

                UseMultiSelect = true,

                PropertyNameForDisplay = treeInfo.DataTextField,

                PropertyNameForValue = treeInfo.DataSource.ModelCr.ModelIdName,

                PropertyNameForBinding = propertyNameForBinding

            };


            //-----Create lookup----
            containerState.InnerHtml = multiSelectHtml + CreatLookupButton(lookupInfo);

            container.InnerHtml = containerState.ToString();

            container.InnerHtml = containerState.ToString();

            return MvcHtmlString.Create(container.ToString());

        }

        #region multi select for tree overrides
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string title, TreeInfo treeInfo, string propertyNameForBinding, bool readOnly, bool isMultiSelect) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, string.Empty, title, treeInfo, propertyNameForBinding, isMultiSelect, null, readOnly, null, null);
        }

        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding, bool readOnly, bool isMultiSelect) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, treeInfo, propertyNameForBinding, isMultiSelect, null, readOnly, null, null);
        }
        public static MvcHtmlString LookUpRP<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding, bool readOnly, bool isMultiSelect, Dictionary<string, object> lookupHtmlAttributes, params  ValidationBase[] validationCr) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, treeInfo, propertyNameForBinding, isMultiSelect, null, readOnly, lookupHtmlAttributes, validationCr);
        }
        public static MvcHtmlString LookUpCr<TModel>(this HtmlHelper helper, string id, string name, string title, TreeInfo treeInfo, string propertyNameForBinding, bool readOnly, bool isMultiSelect, object htmlAttributes) where TModel : IViewModel, new()
        {
            return LookUpCr<TModel>(helper, id, name, title, treeInfo, propertyNameForBinding, isMultiSelect, htmlAttributes, readOnly, null, null);
        }


        #endregion

        #endregion


        //private static void CreateValidationForlookup(TagBuilder tagForModel, Dictionary<string, object> lookupHtmlAttributes, ValidationBase[] validationCr)
        //{
        //    foreach (var item in validationCr)
        //    {
        //        switch (item.GetType().Name)
        //        {
        //            case "Required":
        //                {
        //                    lookupHtmlAttributes.Add("Style", "border-left-color:rgb(236,99,22);border-left-width:5px;");

        //                    break;
        //                }
        //        }

        //        tagForModel.MergeAttributes(item.CreateRelatedValidation());

        //    }
        //}
        private static Dictionary<string, string> CreateValidationForlookup(Dictionary<string, object> lookupHtmlAttributes, ValidationBase[] validationCr)
        {
            IEnumerable<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (var item in validationCr)
            {
                switch (item.GetType().Name)
                {
                    case "Required":
                        {
                            lookupHtmlAttributes.Add("Style", "border-left-color:rgb(236,99,22);border-left-width:5px;");
                            break;
                        }
                }
                result = item.CreateRelatedValidation().Concat(result);

            }
            result.ToList().ForEach(item => res.Add(item.Key, item.Value));
            return res;
        }

        //old
        //private static TagBuilder CreatLookupButton()
        //{
        //    TagBuilder button = new TagBuilder("span");
        //    button.MergeAttribute("id", string.Format("btnRP_{0}", Id));
        //    button.AddCssClass(string.Format("{0} {1}", StyleKind.RighSpace, StyleKind.Button));
        //    TagBuilder icon = new TagBuilder("span");
        //    icon.AddCssClass(StyleKind.Icons.LookUP);
        //    button.InnerHtml = icon.ToString() + ShowLookupScript();
        //    return button;
        //}



        //private static string ShowLookupScript()
        //{
        //    var script = @"<script>  $(document).ready(function () {  " +
        //       "$('#" + string.Format("btnRP_{0}", Id) + "').click(function () { var temp =$(this);  if( temp.attr('disabled')!== 'disabled') "
        //          + "LookUp.show('" + Title + "','lkp_" + Id + "_Div'" + @" );"
        //           + "else return false;    });} );</script>";
        //    return script;
        //}

        //----------------------------new
        private static TagBuilder CreatLookupButton(Lookup.Grid lookInfo)
        {
            TagBuilder button = new TagBuilder("span");
            button.MergeAttribute("id", string.Format("btnCr_{0}", Id));
            button.AddCssClass(string.Format("{0} {1}", StyleKind.RighSpace, StyleKind.Button));
            TagBuilder icon = new TagBuilder("span");
            icon.AddCssClass(StyleKind.Icons.LookUP);
            button.InnerHtml = icon.ToString() + ShowLookupScript(lookInfo);
            return button;
        }

        private static TagBuilder CreatLookupButton(Lookup.Tree lookInfo)
        {
            TagBuilder button = new TagBuilder("span");
            button.MergeAttribute("id", string.Format("btnCr_{0}", Id));
            button.AddCssClass(string.Format("{0} {1}", StyleKind.RighSpace, StyleKind.Button));
            TagBuilder icon = new TagBuilder("span");
            icon.AddCssClass(StyleKind.Icons.LookUP);
            button.InnerHtml = icon.ToString() + ShowLookupScript(lookInfo);
            return button;
        }




        private static string ShowLookupScript(Lookup.Grid info)
        {
            var script = @"<script>  $(document).ready(function () {  " +
               "$('#" + string.Format("btnCr_{0}", Id) + "').click(function () { var temp =$(this);  if( temp.attr('disabled')!== 'disabled') "
                  + string.Format("Lookup.loadGrid('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10}",
                                       info.Title,
                                       info.ViewModel,
                                       info.ViewInfoKey,
                                       info.LookupName,
                                       info.GridID,
                                       info.PropertyNameForDisplay,
                                       info.PropertyNameForValue,
                                       info.PropertyNameForBinding,
                                       info.UseMultiSelect,
                                       info.Width,
                                       info.Height
                                        ) + @" );"
                   + "else return false;    });} );</script>";
            return script;
        }

        private static string ShowLookupScript(Lookup.Tree info)
        {
            var script = @"<script>  $(document).ready(function () {  " +
               "$('#" + string.Format("btnCr_{0}", Id) + "').click(function () { var temp =$(this);  if( temp.attr('disabled')!== 'disabled') "
                  + string.Format("Lookup.loadTree('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9}",
                                       info.Title,
                                       info.ViewModel,
                                       info.ViewInfoKey,
                                       info.LookupName,
                                       info.TreeID,
                                       info.PropertyNameForDisplay,
                                       info.PropertyNameForBinding,
                                       info.UseMultiSelect,
                                       info.Width,
                                       info.Height
                                        ) + @" );"
                   + "else return false;    });} );</script>";
            return script;
        }
    }
}
