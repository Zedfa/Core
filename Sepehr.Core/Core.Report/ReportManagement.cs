using Core.Cmn.Report;
using Core.Service;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Report
{
    public class ReportManagement
    {
        private ReportService _reportService;

        public ReportManagement()
        {
            _reportService = new ReportService();
        }

        public static void SyncReports()
        {
            try
            {
                long lastUpdateTick = DateTime.Now.Ticks;
                var reportFiles = Directory.GetFiles(Core.Cmn.ConfigHelper.GetConfigValue<string>("ReportFilesPath"), "*.*", SearchOption.TopDirectoryOnly);
                var reports = new ReportService().All().ToList();
                List<string> newFiles = new List<string>();
                List<Core.Entity.Report> deleteFiles = new List<Core.Entity.Report>();
                List<Core.Entity.Report> updateFiles = new List<Core.Entity.Report>();

                foreach (var reportFile in reportFiles)
                {
                    var item = reports.FirstOrDefault(r => r.Name == Path.GetFileName(reportFile));
                    if (item == null)
                    {
                        newFiles.Add(reportFile);
                    }
                    else
                    {
                        updateFiles.Add(item);
                    }
                }
                deleteFiles.AddRange(reports.Where(r => !reportFiles.Contains(r.Name)));
            }
            catch 
            {

            }
        }

        public byte[] GetReportPDF(
            IReportRequest reportRequest,
            IReportService reportService,
            string serverRootPath,
            int currentUserId
            )
        {
            string reportFileName = string.Format(@"{0}\{1}_Fa.mrt",
                serverRootPath + "\\Areas\\sepehr",
                reportService.GetReportName(reportRequest)
                );

            byte[] reportTemplate = Core.Cmn.FileHelper.ReadFile(reportFileName);
            List<ReportDataSource> reportData = reportService.GetReportData(reportRequest, currentUserId);

            return GetPdfBytes(reportTemplate, reportData);            
        }

        public byte[] GetReportExcel(
            IReportRequest reportRequest,
            IReportService reportService,
            string serverRootPath,
            int currentUserId
            )
        {
            string reportFileName = string.Format(@"{0}\{1}_Fa.mrt",
                serverRootPath + "\\Areas\\sepehr",
                reportService.GetReportName(reportRequest)
                );

            byte[] reportTemplate = Core.Cmn.FileHelper.ReadFile(reportFileName);
            List<ReportDataSource> reportData = reportService.GetReportData(reportRequest, currentUserId);

            return GetExcelBytes(reportTemplate, reportData);            
        }

        private static byte[] GetPdfBytes(byte[] reportTemplate, List<ReportDataSource> dataSources)
        {
            using (Stimulsoft.Report.StiReport stiReport = new Stimulsoft.Report.StiReport())
            {
                foreach (var dataSource in dataSources)
                {
                    stiReport.RegBusinessObject(dataSource.DataSourceName, dataSource.DataSource);
                }

                stiReport.Load(reportTemplate);
                stiReport.Render();

                byte[] result = null;
                using (MemoryStream stream = new MemoryStream())
                {
                    StiPdfExportService pdfService = new StiPdfExportService();
                    pdfService.ExportPdf(stiReport, stream);
                    result = stream.ToArray();
                }
                return result;
            }
        }

        private static byte[] GetExcelBytes(byte[] reportTemplate, List<ReportDataSource> dataSources)
        {
            using (Stimulsoft.Report.StiReport stiReport = new Stimulsoft.Report.StiReport())
            {
                foreach (var dataSource in dataSources)
                {
                    stiReport.RegBusinessObject(dataSource.DataSourceName, dataSource.DataSource);
                }
                stiReport.Load(reportTemplate);
                stiReport.Render();


                byte[] result = null;
                using (MemoryStream stream = new MemoryStream())
                {
                    StiExcel2007ExportService excelService = new StiExcel2007ExportService();
                    excelService.ExportExcel(stiReport, stream);
                    result = stream.ToArray();
                }
                return result;
            }
        }
    }
}
