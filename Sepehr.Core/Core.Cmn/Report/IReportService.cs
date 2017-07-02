using System;
using System.Collections.Generic;

namespace Core.Cmn.Report
{
    public interface IReportService
    {
        string ServiceName { get; }

        List<ReportDataSource> GetReportData(object reportRequet);

        string GetReportName(object reportRequet);

        Type RequetType { get; }
    }
}
