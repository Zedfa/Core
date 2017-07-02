using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Core.Mvc.Helpers
{
    public static class DatePickerHelper
    {
         const string _culture = "fa-IR";
         const string _format = "yyyy/MM/dd";

        #region DatePickerCr
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, StartCalendar startFrom, DateTime maxDate, DateTime minDate, DateTime defaultValue, IDictionary<string, object> htmlAttributes, string footerText, string formatDate =_format ,string culture=_culture )
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Max(maxDate)
                .Min(minDate)
                .Start((CalendarView)startFrom)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate)
                .HtmlAttributes(htmlAttributes)
                .Footer(footerText);
        }
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, StartCalendar startFrom, DateTime maxDate, DateTime minDate, DateTime defaultValue, object htmlAttributes, string footerText, string formatDate = _format, string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Max(maxDate)
                .Min(minDate)
                .Start((CalendarView)startFrom)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate)
                .HtmlAttributes(htmlAttributes)
                .Footer(footerText);
        }

        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, StartCalendar startFrom, DateTime defaultValue, IDictionary<string, object> htmlAttributes,string formatDate =_format , string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Start((CalendarView)startFrom)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate)
                .HtmlAttributes(htmlAttributes);
        }
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, DateTime maxDate, DateTime minDate, DateTime defaultValue, IDictionary<string, object> htmlAttributes,string formatDate =_format , string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Max(maxDate)
                .Min(minDate)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate)
                .HtmlAttributes(htmlAttributes);
        }
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, DateTime maxDate, DateTime minDate, DateTime defaultValue,string formatDate =_format , string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Max(maxDate)
                .Min(minDate)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate);
        }
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, DateTime defaultValue,string formatDate =_format , string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Value(defaultValue)
                .Culture(culture)
                .Format(formatDate);
        }
        public static Kendo.Mvc.UI.Fluent.DatePickerBuilder DatePickerCr(this HtmlHelper helper, string name, string formatDate =_format ,string culture = _culture)
        {
            return helper.Kendo().DatePicker()
                .Name(name)
                .Culture(culture)
                .Format(formatDate);
        }
        #endregion

        
    }
    public enum StartCalendar
    {
        Year = CalendarView.Year,
        Month = CalendarView.Month,
        Decade = CalendarView.Decade,
        Century = CalendarView.Century
    }
}
