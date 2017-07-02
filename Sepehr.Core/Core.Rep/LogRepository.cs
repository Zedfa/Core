
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
                                l =>
                                new LogDTO()
                                {
                                    ID = l.ID,
                                    UserId = l.UserId,
                                    CustomMessage = l.CustomMessage,
                                    CreateDate = l.CreateDate,
                                    InnerExceptionCount = l.InnerExceptionCount,
                                    LogType = l.LogType
                                });
        }


        public ExceptionLog GetExceptionLog(Guid logId)
        {
            var coreDbContext = (DbContextBase)_dc;
            var log = _dc.Set<Log>().Include("ExceptionLog").FirstOrDefault(l => l.ID == logId);
            if (log != null)
            {
                return log.ExceptionLog;
            }

            return null;
        }

        public ExceptionLog GetExceptionLogOfThisParent(Guid parentId)
        {

            var coreDbContext = (DbContextBase)_dc;
            // var gId = Guid.Parse("8B2D4C2F-D3A6-E411-89E8-005056C00008");
            var exceptionlog = _dc.Set<ExceptionLog>().Include("InnerException").Where(l => l.ID == parentId).Select(ell => ell.InnerException).First();
            if (exceptionlog != null)
            {
                return exceptionlog;
            }

            return null;

        }

        public bool HasAnyChildren(Guid Id)
        {
            //var gId = Guid.Parse("8B2D4C2F-D3A6-E411-89E8-005056C00008");   gId
            var coreDbContext = (DbContextBase)_dc;
            var hasExceptionlog = _dc.Set<ExceptionLog>().Include("InnerException").Any(l => l.ID == Id);
            return hasExceptionlog;
        }

    }
}
