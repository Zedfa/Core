using Core.Cmn.Report;
using Core.Mvc.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Mvc.Controllers.Report
{
    public class ReportController : ControllerBaseCr
    {
        private FileContentResult GetReportFileContent(
            byte[] reportData,
            IReportRequest requestParams
            )
        {
            return File(
                reportData,
                MimeMapping.GetMimeMapping(requestParams.FileName),//System.Net.Mime.MediaTypeNames.Application.Octet, 
                string.Format(
                    /// ma bekhatere bug firefox majborim esme file ro bezarim to double quatation
                    /// http://stackoverflow.com/questions/21442903/firefox-has-problems-when-downloading-with-a-space-in-filename
                    "\"{0}-{1}{2}\"",
                    Path.GetFileNameWithoutExtension(requestParams.FileName),
                    DateTime.Now.ToString("HHmmss"),
                    Path.GetExtension(requestParams.FileName)
                    )
                );
        }

        [HttpGet]
        public ActionResult Index(string serviceName, string Paramas)
        {
            IReportService iReportService = ReportConfig.GetReportServiceByName(serviceName);
            IReportRequest reportRequest = Newtonsoft.Json.JsonConvert.DeserializeObject(Paramas, iReportService.RequetType) as IReportRequest;

            byte[] reportData = new Core.Report.ReportManagement().GetReportPDF(
                reportRequest,
                iReportService,
                System.Web.Hosting.HostingEnvironment.MapPath("~"),
                GetCurrentUserId()
                );

            return GetReportFileContent(reportData, reportRequest);
        }

        [HttpGet]
        public ActionResult GetExcel(string serviceName, string Paramas)
        {
            IReportService iReportService = ReportConfig.GetReportServiceByName(serviceName);
            IReportRequest reportRequest = Newtonsoft.Json.JsonConvert.DeserializeObject(Paramas, iReportService.RequetType) as IReportRequest;

            byte[] reportData = new Core.Report.ReportManagement().GetReportExcel(
                reportRequest,
                iReportService,
                System.Web.Hosting.HostingEnvironment.MapPath("~"),
                GetCurrentUserId()
                );

            return GetReportFileContent(reportData, reportRequest);
        }

        private int GetCurrentUserId()
        {
            /// in session felan faghat to narmafzare sepehr darim
            if (
                System.Web.HttpContext.Current != null
                &&
                System.Web.HttpContext.Current.Session["AgencyUser_ID"] != null
                )
            {
                return Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyUser_ID"]);
            }
            else
            {
                return 0;
            }
        }
    }
}