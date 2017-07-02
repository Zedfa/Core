using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections;

namespace Core.Mvc.Helpers
{
    public static class SelectionHelper
    {

        const string _optionLabel = "انتخاب نشده";

        #region DropDownListCr
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, string dependentDropDownName,  object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                {
                    source.Read(read =>
                    {
                        read.Url(url).Type(HttpVerbs.Get);
                    });
                })
                .HtmlAttributes(htmlAttributes)
                .CascadeFrom(dependentDropDownName)
                .OptionLabel(optionLabel);
               
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, string dependentDropDownName, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                {
                    source.Read(read =>
                    {
                        read.Url(url).Type(HttpVerbs.Get);
                    });
                })
                .CascadeFrom(dependentDropDownName)
                .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, object htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .HtmlAttributes(htmlAttributes)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                   {
                       source.Read(read =>
                         {
                             read.Url(url).Type(HttpVerbs.Get);
                         });
                   })
                   .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                {
                    source.Read(read =>
                    {
                        read.Url(url).Type(HttpVerbs.Get);
                    });
                })
                   .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable<SelectListItem> list, object htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .HtmlAttributes(htmlAttributes)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .OptionLabel(optionLabel); ;

        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable<SelectListItem> list, string dependentDropDownName, string optionLabel = _optionLabel)
        {
            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .CascadeFrom(dependentDropDownName)
                .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable<SelectListItem> list, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable list, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable<SelectListItem> list, string defaultValue, object htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .HtmlAttributes(htmlAttributes)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .Value(defaultValue);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable<SelectListItem> list, string dependentDropDownName, string defaultValue, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .CascadeFrom(dependentDropDownName)
                .Value(defaultValue);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, IEnumerable list, string defaultValue, object htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .HtmlAttributes(htmlAttributes)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .BindTo(list)
                .Value(defaultValue);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string defaultValue, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .Value(defaultValue);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, IEnumerable list, string defaultValue, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .Value(defaultValue);
        }
        
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string optionLabel = _optionLabel)
        {
            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .OptionLabel(optionLabel);
        }
        
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dependentDropDownName, IEnumerable<SelectListItem> list, string optionLabel = _optionLabel)
        {
            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .CascadeFrom(dependentDropDownName)
                .OptionLabel(optionLabel);
        }
        
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, string dependentDropDownName, IEnumerable list, string optionLabel = _optionLabel)
        {
            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .CascadeFrom(dependentDropDownName)
                .OptionLabel(optionLabel);
        }
       
        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, Dictionary<string,object> htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .HtmlAttributes(htmlAttributes)
                .OptionLabel(optionLabel); 

        }

        public static Kendo.Mvc.UI.Fluent.DropDownListBuilder DropDownListCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string defaultValue, Dictionary<string, object> htmlAttributes, string optionLabel = _optionLabel)
        {

            return helper.Kendo().DropDownList().Name(name)
                .BindTo(list)
                .HtmlAttributes(htmlAttributes)
                .Value(defaultValue)
                .OptionLabel(optionLabel);

        }

        #endregion

        #region MultiSelectCr
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string dataTextField, string dataValueField, IEnumerable<string> defaultValue,FilterType filter, object htmlAttributes, string optionLabel = _optionLabel)
        {
            
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Placeholder(optionLabel)
                .Value(defaultValue)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .HtmlAttributes(htmlAttributes)
                .Filter(filter);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string dataTextField, string dataValueField, IEnumerable<string> defaultValue, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Placeholder(optionLabel)
                .Value(defaultValue)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .HtmlAttributes(htmlAttributes);
               
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, IEnumerable<string> defaultValue, FilterType filter, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                                 source.Read(read =>
                                                read.Url(url).Type(HttpVerbs.Get)))
                .Value(defaultValue)
                .HtmlAttributes(htmlAttributes)
                .Filter(filter);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, IEnumerable<string> defaultValue, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source =>
                                 source.Read(read =>
                                                read.Url(url).Type(HttpVerbs.Get)))
                .Value(defaultValue)
                .HtmlAttributes(htmlAttributes);
        }

        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source => source.Read(read => read.Url(url).Type(HttpVerbs.Get)))
                .HtmlAttributes(htmlAttributes);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<string> defaultValue, string dataTextField, string dataValueField, string url, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source => source.Read(read => read.Url(url).Type(HttpVerbs.Get)))
                .Value(defaultValue);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<string> defaultValue, string dataTextField, string dataValueField, FilterType filter, string url, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source => source.Read(read => read.Url(url).Type(HttpVerbs.Get)))
                .Value(defaultValue)
                .Filter(filter);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .DataSource(source => source.Read(read => read.Url(url).Type(HttpVerbs.Get)));
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string url,FilterType filter, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .Filter(filter)
                .DataSource(source => source.Read(read => read.Url(url).Type(HttpVerbs.Get)));
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField);
        }

        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField,FilterType filter)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(_optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .Filter(filter) ;
        }

        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, string htmlTemplate,FilterType filter)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(_optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .Filter(filter)
                .ItemTemplate (htmlTemplate);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, string dataTextField, string dataValueField, FilterType filter,object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .Placeholder(optionLabel)
                .DataTextField(dataTextField)
                .DataValueField(dataValueField)
                .Filter(filter)
                .HtmlAttributes (htmlAttributes) ;
        }

        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, IEnumerable<string> defaultValue, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Placeholder(optionLabel)
                .Value(defaultValue);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, IEnumerable<string> defaultValue, FilterType filter, object htmlAttributes, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Placeholder(optionLabel)
                .Value(defaultValue)
                .Filter(filter);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list,FilterType filter, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Filter(filter)
                .Placeholder(optionLabel);
        }
        public static Kendo.Mvc.UI.Fluent.MultiSelectBuilder MultiSelectCr(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, string optionLabel = _optionLabel)
        {
            return helper.Kendo().MultiSelect()
                .Name(name)
                .BindTo(list)
                .Placeholder(optionLabel);
        }
        #endregion
    }
}
