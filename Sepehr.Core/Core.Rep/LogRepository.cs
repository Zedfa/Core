
using Core.Entity;
using System;
using System.Linq;
using Core.Rep.DTO;
using Core.Ef;
using Core.Cmn;
using Core.Cmn.Extensions;
namespace Core.Rep
{

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        #region Variable
        IDbContextBase _dc;
        #endregion

        #region Constructors

        public LogRepository(IDbContextBase dc)
            : base(dc)
        {
            _dc = dc;
            dc.DisableExceptionLogger = true;
        }
        public LogRepository()
            : base()
        {

            ContextBase.DisableExceptionLogger = true;
        }
        #endregion

        #region Methods

        #endregion


        protected override void SetQueryable(IQueryable<Log> queryable)
        {

            QueryableDtos = queryable.Select(
                                log =>
                                new LogDTO()
                                {
                                    ID = log.Id,
                                    Source = log.Source,
                                    CustomMessage = log.CustomMessage,
                                    CreateDate = log.CreateDate,
                                    InnerExceptionCount = log.InnerExceptionCount,
                                    ApplicationName = log.ApplicationName
                                });
        }


        public ExceptionLog GetExceptionLog(int logId)
        {
            var coreDbContext = (DbContextBase)_dc;
            var log = _dc.Set<Log>().Include("ExceptionLog").FirstOrDefault(l => l.Id == logId);
            if (log != null)
            {
                return log.ExceptionLog;
            }

            return null;
        }

        public ExceptionLog GetExceptionLogOfThisParent(int parentId)
        {

            var coreDbContext = (DbContextBase)_dc;
            // var gId = Guid.Parse("8B2D4C2F-D3A6-E411-89E8-005056C00008");
            var exceptionlog = _dc.Set<ExceptionLog>().Include("InnerException").Where(l => l.Id == parentId).Select(ell => ell.InnerException).First();
            if (exceptionlog != null)
            {
                return exceptionlog;
            }

            return null;

        }

        public bool HasAnyChildren(int Id)
        {
            //var gId = Guid.Parse("8B2D4C2F-D3A6-E411-89E8-005056C00008");   gId
            var coreDbContext = (DbContextBase)_dc;
            var hasExceptionlog = _dc.Set<ExceptionLog>().Include("InnerException").Any(l => l.Id == Id);
            return hasExceptionlog;
        }

    }
}
