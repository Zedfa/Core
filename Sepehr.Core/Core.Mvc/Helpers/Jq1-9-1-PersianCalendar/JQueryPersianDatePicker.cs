using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Mvc.Helpers.Jq1_9_1_PersianCalendar
{
    public class JQueryPersianDatePicker 
    {
        private System.Web.Mvc.HtmlHelper helper;
        private string inputID;
        private string dateFormat;
        private string imageRelAddress;
        private string inputName;
        private string yearRange;
        private Dictionary<string, string> htmlAttributes;

        public JQueryPersianDatePicker(System.Web.Mvc.HtmlHelper helper, string inputID, string inputName, string yearRange, string dateFormat, string imageRelAddress, Dictionary<string, string> htmlAttributes)
        {
            // TODO: Complete member initialization
            this.helper = helper;
            this.inputID = inputID;
            this.inputName = inputName;
            this.dateFormat = dateFormat ?? "yy/mm/dd";
            this.imageRelAddress = imageRelAddress;
            this.htmlAttributes = htmlAttributes;
            this.yearRange = yearRange;
        }

        internal System.Web.Mvc.MvcHtmlString Render()
        {
            var dateInputScript = string.Empty;
           
            Dictionary<string, object> _htmlAttributes = new Dictionary<string, object>();
            _htmlAttributes.Add("Id", inputID);
            var styles = string.Empty;
            if (htmlAttributes != null)
            {
                if (htmlAttributes.Count > 0)
                 {
                     foreach (var item in htmlAttributes)
                     {

                         _htmlAttributes.Add(item.Key, item.Value);
                         if(item.Key == "style")
                         {
                             styles = item.Value;
                         }
                     }
                }
            }

           MvcHtmlString textbox = helper.TextBoxCr(inputName,string.Format("position:relative !important; z-index:1000000 !important; {0} ", styles) ,_htmlAttributes);
           var yearRangeStr = string.Format("{0}", !string.IsNullOrEmpty(this.yearRange) ? string.Format(" , yearRange:'{0}'", yearRange) : string.Empty);
           var datePickerScrpt = string.Format("$('#{3}').css('font-size', 'x-small');" +
                                                 "  " + "$(function(e) {0}  $('#{2}').datepicker({0} changeMonth: true , changeYear: true ,showButtonPanel: true {4} , dateFormat: '{5}' , showOn: 'button' , buttonImage: 'Scripts/Plugin/DatePicker/css/images/calendar1.png' {1});" +
                                                  "$('#{3}').css('font-size', 'x-small');" +
                                                 "{1}); "
                          , "{", "}", this.inputID, "ui-datepicker-div", yearRangeStr, this.dateFormat);
            dateInputScript = string.Format("<script> {0} </script>", datePickerScrpt);
            var finalScrpt = string.Format("{0}", dateInputScript);

            return (new MvcHtmlString("<span>" + finalScrpt + textbox.ToString() + "</span>"));//
        }
    }
}
