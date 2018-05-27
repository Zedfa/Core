using System;
using System.Collections.Generic;

namespace Core.Cmn.Report
{
    public interface IReportService
    {
        string ServiceName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportRequet"></param>
        /// <param name="currentUserId">
        /// moghe generate kardane ye report, momkene bekhaim check konim ke aya user
        /// be in data dastresi dare ya na.
        /// </param>
        /// <returns></returns>
        List<ReportDataSource> GetReportData(object reportRequet, int currentUserId);

        string GetReportName(object reportRequet);

        Type RequetType { get; }
    }
}
