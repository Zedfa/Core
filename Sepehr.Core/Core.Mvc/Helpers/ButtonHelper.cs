using Core.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Mvc.Helpers
{
    public enum ButtonKind
    {
        submit = 0,
        button = 1,
        reset = 2
    }
    public static class ButtonHelper
    {

        #region Submit

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string text, string uniqueNameElement, bool isConfirmButton = false, string name = "", string toolTip = "", string style = "", object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(uniqueNameElement))
            {
                return SubmitCr(helper, id, name, text, isConfirmButton, toolTip, style, htmlAttributes);
            }

            else
            {
                return Core.Service.AppBase.HasCurrentUserAccess(CustomMembershipProvider.GetUserIdCookie() ?? 0, null, uniqueNameElement) ?
                    SubmitCr(helper, id, name, text, isConfirmButton, toolTip, style, htmlAttributes) : SubmitCr(helper, id, name, text, isConfirmButton, toolTip, style, htmlAttributes, false);
            }

        }

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string name, string text, bool isConfirmButton, string toolTip, string style, object htmlAttributes, bool disable = false)
        {
            TagBuilder builder = new TagBuilder("input");

            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add("type", Enum.GetName(typeof(ButtonKind), ButtonKind.submit));

            attributes.Add("id", id);

            attributes.Add("value", text);

            if (!string.IsNullOrEmpty(name))
            {
                attributes.Add("name", name);
            }

            if (!string.IsNullOrEmpty(toolTip))
            {
                attributes.Add("title", toolTip);
            }

            if (!string.IsNullOrEmpty(style))
            {
                attributes.Add("Style", style);
            }

            if (isConfirmButton)
            {
                attributes.Add("default", "");
            }

            HtmlModifier.ManageAttributesWithPermissions(attributes, StyleKind.Button, disable);

            if (htmlAttributes != null)
            {
                var attributesObj = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                builder.MergeAttributes(attributesObj);
            }

            builder.MergeAttributes(attributes);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        #region Submit overrides
        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string name, string text, string toolTip)
        {
            return SubmitCr(helper, id, name, text, false, toolTip, string.Empty, (object)null);
        }

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string name, string text, bool isConfirmButton)
        {
            return SubmitCr(helper, id, name, text, isConfirmButton, string.Empty, string.Empty, (object)null);
        }

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string name, string text)
        {
            return SubmitCr(helper, id, name, text, false, string.Empty, string.Empty, (object)null);
        }

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string text, bool isConfirmButton)
        {
            return SubmitCr(helper, id, string.Empty, text, isConfirmButton, string.Empty, string.Empty, (object)null);
        }

        public static MvcHtmlString SubmitCr(this HtmlHelper helper, string id, string text)
        {
            return SubmitCr(helper, id, string.Empty, text, false, string.Empty, string.Empty, (object)null);
        }
        #endregion

        #endregion

        #region button


        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string text, string uniqueNameElement, bool isConfirmButton = false, string name = "", string style = "", object htmlAttributes = null)
        {

            if (string.IsNullOrEmpty(uniqueNameElement))
            {
                return ButtonCr(helper, id, name, text, isConfirmButton, style, htmlAttributes);
            }

            else
            {
                return Core.Service.AppBase.HasCurrentUserAccess(CustomMembershipProvider.GetUserIdCookie() ?? 0, null, uniqueNameElement) ?
                    ButtonCr(helper, id, name, text, isConfirmButton, style, htmlAttributes) : ButtonCr(helper, id, name, text, isConfirmButton, style, htmlAttributes, false);
            }
        }


        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string name, string text, bool isConfirmButton, string style, object htmlAttributes, bool disable = false)
        {
            TagBuilder builder = new TagBuilder("input");

            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add("type", Enum.GetName(typeof(ButtonKind), ButtonKind.button));

            attributes.Add("id", id);

            if (!string.IsNullOrEmpty(name))
            {
                attributes.Add("name", name);
            }

            attributes.Add("value", text);

            if (!string.IsNullOrEmpty(style))
            {
                attributes.Add("Style", style);
            }

            if (isConfirmButton)
            {
                attributes.Add("default", "");
            }

            HtmlModifier.ManageAttributesWithPermissions(attributes, StyleKind.Button, disable);

            builder.MergeAttributes(attributes);

            if (htmlAttributes != null)
            {
                if (htmlAttributes.GetType() == typeof(Dictionary<string, object>))
                {
                    builder.MergeAttributes((Dictionary<string, object>)htmlAttributes);
                }
                else
                {
                    var objAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                    builder.MergeAttributes(objAttributes);
                }

            }

            
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        #region button overrides

        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string name, string text, string style)
        {
            return ButtonCr(helper, id, name, text, false, style, (object)null);
        }
        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string name, string text, bool isConfirmButton)
        {
            return ButtonCr(helper, id, name, text, isConfirmButton, string.Empty, (object)null);
        }
        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string name, string text)
        {
            return ButtonCr(helper, id, name, text, false, string.Empty, (object)null);
        }

        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string text, bool isConfirmButton)
        {
            return ButtonCr(helper, id, string.Empty, text, isConfirmButton, string.Empty, (object)null);
        }
        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string text)
        {
            return ButtonCr(helper, id, string.Empty, text, false, string.Empty, (object)null);
        }

        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string text, bool isConfirmButton, object htmlAttributes)
        {
            return ButtonCr(helper, id, string.Empty, text, isConfirmButton, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString ButtonCr(this HtmlHelper helper, string id, string text, object htmlAttributes)
        {
            return ButtonCr(helper, id, string.Empty, text, false, string.Empty, htmlAttributes);
        }

        #endregion

        #endregion

        #region reset

        public static MvcHtmlString ResetButtonCr(this HtmlHelper helper, string id, string name, string text, string style, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("input");

            Dictionary<string, object> attributes = new Dictionary<string, object>();

            attributes.Add("type", Enum.GetName(typeof(ButtonKind), ButtonKind.reset));

            attributes.Add("id", id);

            attributes.Add("value", text);

            if (!string.IsNullOrEmpty(name))
            {
                attributes.Add("name", name);
            }

            if (!string.IsNullOrEmpty(style))
            {
                attributes.Add("Style", style);
            }

            HtmlModifier.ManageHtmlAttributes(attributes, StyleKind.Button);

            builder.MergeAttributes(attributes);

            if (htmlAttributes != null)
            {
                var objAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                builder.MergeAttributes(objAttributes);
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));


        }

        #region reset overrides
        public static MvcHtmlString ResetButtonCr(this HtmlHelper helper, string id, string name, string text, string style)
        {
            return ResetButtonCr(helper, id, name, text, style, null);
        }

        public static MvcHtmlString ResetButtonCr(this HtmlHelper helper, string id, string name, string text)
        {
            return ResetButtonCr(helper, id, name, text, string.Empty, null);
        }
        public static MvcHtmlString ResetButtonCr(this HtmlHelper helper, string id, string text)
        {
            return ResetButtonCr(helper, id, string.Empty, text, string.Empty, null);
        }

        #endregion

        #endregion

    }
}
