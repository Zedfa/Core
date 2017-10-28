using Core.Mvc.Controller;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Core.Mvc.Controllers.ReportViewer
{
    public class StimReportViewerController : ControllerBaseCr
    {
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
                return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ViewerEvent()
        {
            var report = StiMvcViewer.GetReportObject(HttpContext);

            return StiMvcViewer.ViewerEventResult(HttpContext);
        }

        public ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(HttpContext);
        }

        public FileResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult(HttpContext);
        }

        public ActionResult Interaction()
        {
            return StiMvcViewer.InteractionResult(HttpContext);
        }
    }
}