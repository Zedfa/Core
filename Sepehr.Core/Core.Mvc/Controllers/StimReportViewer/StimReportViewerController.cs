using Core.Mvc.Controller;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Mvc.Controllers.ReportViewer
{
    public class StimReportViewerController : ControllerBaseCr
    {
        /*
        //
        // GET: /ReportViewr/
        public string ReportType { get; set; }

        public ActionResult Index(string reportType)
        {
            ReportType = reportType;
            return View();
        }

        private static List<object> GetDataSource()
        {
            List<object> result = null;

            return result;
        }

        public ActionResult GetReportSnapshot()
        {
            var reportType = ReportType;
            var cities = GetDataSource();

            // Create the report object and load data from xml file
            StiReport report = new StiReport();

            //report.Dictionary.Synchronize();
            //report.Compile(Server.MapPath("~/Content/Reports/Report.mrt"));
            report.Load(Server.MapPath("~/Content/Reports/Report.mrt"));
            report.RegBusinessObject("City", cities);

            try
            {
                return StiMvcViewer.GetReportResult(report);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ViewerEvent()
        {
            var report = StiMvcViewer.GetReportObject();

            return StiMvcViewer.ViewerEventResult();
        }

        public ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult();
        }

        public ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult();
        }

        public ActionResult Interaction()
        {
            return StiMvcViewer.InteractionResult();
        }
        */
    }
}