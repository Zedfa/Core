using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.ViewModel
{
    public class StimReportViewModel
    {
        public string ReportType
        {
            get
            {
                if (HttpContext.Current.Session["StimReportType"] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session["StimReportType"].ToString();
                }
            }
            set
            {
                if (HttpContext.Current.Session["StimReportType"] == null)
                {
                    HttpContext.Current.Session.Add("StimReportType", value);
                }
                else
                {
                    HttpContext.Current.Session["StimReportType"] = value;
                }
            }
        }
    }
}