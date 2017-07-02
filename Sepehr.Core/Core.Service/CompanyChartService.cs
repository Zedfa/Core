using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ICompanyChartService), DomainName = "Core")]

    public class CompanyChartService : ServiceBase<CompanyChart>, ICompanyChartService
    {

        //[Dependency]
        private IUserProfileService UserProfileService { get; set; }


        public CompanyChartService(IDbContextBase dbContextBase, IUserLog userLog, IUserProfileService userProfileService)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new CompanyChartRepository(ContextBase, userLog);
            UserProfileService = userProfileService;
        }
        public IQueryable<CompanyChart> GetCompanyChart(int? id)
        {
            return (_repositoryBase as CompanyChartRepository).GetCompanyChart(id);
        }


        public int Delete(int id)
        {
            return (_repositoryBase as CompanyChartRepository).Delete(id);
        }

        public void SetCompanyChartInfo(string userName)
        {
            var user = UserProfileService.GetUserProfile(userName);
            if (user != null)
            {
                if (user.User.CompanyChart != null)
                {
                    appBase.CompanyChartName = user.User.CompanyChart.Title;
                    appBase.CompanyChartId = user.User.CompanyChart.Id;
                }

            }
        }
    }
}
