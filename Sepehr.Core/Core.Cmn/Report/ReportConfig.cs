using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Report
{
    public class ReportConfig
    {
        public static ConcurrentDictionary<string, IReportService> ReportDataDic { get; set; }

        public static void OnReportConfig(string projectName, Type dBContext)
        {
            string repName;
            System.Reflection.Assembly repAssembly;
            //var servicename = "{0}.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            repName = "{0}, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            ReportDataDic = new ConcurrentDictionary<string, IReportService>();
                repAssembly = Assembly.Load(string.Format(repName, projectName));

            //var serviceAssembly = Assembly.Load(string.Format(servicename, projectName));
            var types = repAssembly.GetTypes().Where(r => typeof(IReportService).IsAssignableFrom(r));

            foreach (Type type in types)
            {
                var reportService = Activator.CreateInstance(type) as IReportService;

                ReportDataDic.TryAdd(reportService.ServiceName, reportService);
            }
        }

        public static IReportService GetReportServiceByName(string serviceName)
        {
            IReportService iReportService;
            if (!ReportConfig.ReportDataDic.TryGetValue(serviceName, out iReportService))
            {
                throw (new Exception($"Error on getting report service of '{serviceName}'"));
            }
            return iReportService;
        }
    }
}
