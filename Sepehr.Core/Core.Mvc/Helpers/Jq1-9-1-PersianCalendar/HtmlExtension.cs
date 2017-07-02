using Core.Entity;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Core.Mvc.Helpers.Jq1_9_1_PersianCalendar
{
    public static class HtmlExtension
    {
        public static MvcHtmlString JQPersianCalendarCr(this HtmlHelper helper, string inputID, string inputName , string dateFormat , string imageRelAddress ,
            Dictionary<string, string> htmlAttributes , string yearRangeLowerBound = null, string yearRangeHigherBound = null)// where T : class
        {
            var yearRange = string.Empty;
            if (!(string.IsNullOrEmpty(yearRangeHigherBound) || string.IsNullOrEmpty(yearRangeLowerBound)))
            {
                yearRange = string.Format("-{0}:+{1}", yearRangeLowerBound, yearRangeHigherBound);
            }
            return (new JQueryPersianDatePicker(helper, inputID, inputName, yearRange , dateFormat, imageRelAddress, htmlAttributes)).Render();
        }

        public static MvcHtmlString JQPersianCalendarCr<TModel, TProperty>(this HtmlHelper<TModel> helper, 
            Expression<System.Func<TModel, TProperty>> expression,
            string format, 
            Dictionary<string, string> htmlAttributes,
            string yearRangeLowerBound = null, string yearRangeHigherBound = null)
        {
            //var inputId = helper.ViewDataContainer.ToString().Split('.')[1] + "_";
            var retMemberExpression = (MemberExpression)expression.Body;
            var yearRange = string.Empty;  
            if (!(string.IsNullOrEmpty(yearRangeHigherBound) || string.IsNullOrEmpty(yearRangeLowerBound)))
            {
                yearRange = string.Format("-{0}:+{1}", yearRangeLowerBound, yearRangeHigherBound);
            }
            
            return (new JQueryPersianDatePicker(helper, retMemberExpression.Member.Name, retMemberExpression.Member.Name, yearRange , null, null, htmlAttributes)).Render();
        }
    }
}
