
using Core.Cmn;
using Core.Entity;


namespace Core.Rep
{
    public class ReportRepository : RepositoryBase<Report>
    {
        private IDbContextBase _dbContext;
         public ReportRepository()
             : base()
         {
             
         }
         public ReportRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
            _dbContext = dbContextBase;
        }
    }
}
