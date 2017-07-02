using Core.Cmn.Report;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class ReportService : ServiceBase<Report>
    {
        //private IUserLog _userLog;
        ////private ReportRepository _reportRepository;
        //public abstract string ServiceName { get; }
        //public abstract object GetReportData(object t);
        //public abstract string GetReportName(object t);
        //public Type RequetType { get { return typeof(RequestType); } }

        public ReportService()
            : base()
        {
            _repositoryBase = new ReportRepository();
        }
        public ReportService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
            //_userLog = userLog;
            _repositoryBase = new ReportRepository(ContextBase, userLog);
        }
    }
}
