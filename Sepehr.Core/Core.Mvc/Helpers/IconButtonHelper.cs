using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Cmn.Extensions;
namespace Core.Mvc.Helpers
{
  public static class IconButtonHelper
    {

      public static MvcHtmlString ImageLinkButtonCr(this HtmlHelper helper, string id, string text,string url, string imagePath, string buttonCssClass, string script)
      {
          TagBuilder ButtonBuilder = new TagBuilder("a");

          if (id.HasValue())
          {
              ButtonBuilder.MergeAttribute("id", HtmlModifier.ModifyId(id));
          }

          if (text.HasValue())
          {
              ButtonBuilder.SetInnerText(text);
          }

          if (url.HasValue())
          {
              ButtonBuilder.Attributes.Add("href", url);
          }
          
          if (buttonCssClass.HasValue())
          {
              ButtonBuilder.AddCssClass(buttonCssClass);
          }
          else
              ButtonBuilder.AddCssClass(StyleKind.Button);


          TagBuilder IconBuilder = new TagBuilder("img");

          IconBuilder.AddCssClass(StyleKind.Icons.Icon);

          IconBuilder.Attributes.Add("src", imagePath);

          ButtonBuilder.InnerHtml += IconBuilder.ToString( TagRenderMode.SelfClosing);

          if (script.HasValue())
          {
              ButtonBuilder.InnerHtml += script;
          }

          return MvcHtmlString.Create(ButtonBuilder.ToString());
      }

      public static MvcHtmlString ImageLinkButtonCr(this HtmlHelper helper, string id, string text, string url, string imagePath)
      {
          return ImageLinkButtonCr(helper, id, text, url, imagePath, string.Empty, string.Empty);
      }

      public static MvcHtmlString ImageLinkButtonCr(this HtmlHelper helper, string id, string text, string url, string imagePath, string script)
      {
          return ImageLinkButtonCr(helper, id, text, url, imagePath, string.Empty, script);
      }

      public static MvcHtmlString IconButtonCr(this HtmlHelper helper,string id ,  string text, string buttonCssClass, string iconCssClass, string script)
      {
          TagBuilder ButtonBuilder = new TagBuilder("a");
          
          if (id.HasValue())
          {
              ButtonBuilder.MergeAttribute("id",HtmlModifier.ModifyId( id));
          }

          if (text.HasValue())
          {
              ButtonBuilder.SetInnerText(text);
          }

          if (buttonCssClass.HasValue())
          {
              ButtonBuilder.AddCssClass(buttonCssClass);
          }
          else
              ButtonBuilder.AddCssClass(StyleKind.Button);

          TagBuilder IconBuilder = new TagBuilder("span");
         
          IconBuilder.AddCssClass(iconCssClass);

          ButtonBuilder.InnerHtml += IconBuilder.ToString();

          if (script.HasValue())
          {
              ButtonBuilder.InnerHtml += script;
          }

          return MvcHtmlString.Create(ButtonBuilder.ToString());
      }
      
      public static MvcHtmlString IconButtonCr(this HtmlHelper helper, string buttonCssClass, string iconCssClass)
      {
          return IconButtonCr(helper,string.Empty, string.Empty, buttonCssClass, iconCssClass, string.Empty);
      }

      //public static MvcHtmlString IconButtonCr(this HtmlHelper helper,string id, string buttonCssClass, string iconCssClass)
      //{
      //    return IconButtonCr(helper, id, string.Empty, buttonCssClass, iconCssClass,string.Empty);
      //}

      public static MvcHtmlString IconButtonCr(this HtmlHelper helper, string id, string text, string iconCssClass)
      {
          return IconButtonCr(helper, id,text, string.Empty, iconCssClass,string.Empty);
      }

      public static MvcHtmlString IconButtonCr(this HtmlHelper helper, string id, string buttonCssClass, string iconCssClass, string script)
      {
          return IconButtonCr(helper, id, string.Empty, buttonCssClass, iconCssClass, script);
      }
      /// <summary>
      ///It is not a helper, but just a method to create Icon button 
     /// </summary>
     /// <param name="id"></param>
     /// <param name="text"></param>
     /// <param name="buttonCssClass"></param>
     /// <param name="iconCssClass"></param>
     /// <param name="script"></param>
     /// <returns></returns>
      internal static MvcHtmlString IconButton(string id, string text, string buttonCssClass, string iconCssClass, string script)
      {
          TagBuilder ButtonBuilder = new TagBuilder("a");

          if (id.HasValue())
          {
              ButtonBuilder.MergeAttribute("id", HtmlModifier.ModifyId(id));
          }

          ButtonBuilder.AddCssClass(buttonCssClass);

          TagBuilder IconBuilder = new TagBuilder("span");

          IconBuilder.AddCssClass(iconCssClass);

          ButtonBuilder.InnerHtml = IconBuilder.ToString();

          if (text.HasValue())
          {
              ButtonBuilder.SetInnerText(text);
          }

          if (script.HasValue())
          {
              ButtonBuilder.InnerHtml += script;
          }

          return MvcHtmlString.Create(ButtonBuilder.ToString());
      }


    }
}
