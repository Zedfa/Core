using System;
using System.Collections.Generic;

namespace Core.Cmn.Report
{
    public abstract class ReportBase<RequestType> : IReportService
    {
        public abstract string ServiceName { get; }

        public abstract List<ReportDataSource> GetReportData(object reportRequet);

        public abstract string GetReportName(object reportRequet);

        public Type RequetType
        {
            get
            {
                return typeof(RequestType);
            }
        }

    }
}
