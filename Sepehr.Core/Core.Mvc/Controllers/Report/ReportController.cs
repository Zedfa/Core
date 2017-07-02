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
            IReportRequest requestParams = Newtonsoft.Json.JsonConvert.DeserializeObject(Paramas, iReportService.RequetType) as IReportRequest;
            byte[] reportData = new Core.Report.ReportManagement().GetReportPDF(
                requestParams,
                iReportService,
                System.Web.Hosting.HostingEnvironment.MapPath("~")
                );

            return GetReportFileContent(reportData, requestParams);
        }

        [HttpGet]
        public ActionResult GetExcel(string serviceName, string Paramas)
        {
            IReportService iReportService = ReportConfig.GetReportServiceByName(serviceName);
            IReportRequest requestParams = Newtonsoft.Json.JsonConvert.DeserializeObject(Paramas, iReportService.RequetType) as IReportRequest;
            byte[] reportData = new Core.Report.ReportManagement().GetReportExcel(
                requestParams,
                iReportService,
                System.Web.Hosting.HostingEnvironment.MapPath("~")
                );

            return GetReportFileContent(reportData, requestParams);
        }
    }
}