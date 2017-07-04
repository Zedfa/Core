using System.Collections.Generic;
using Core.Service;
using Core.Entity;
using System.Linq;

namespace Core.Service
{

    public interface ICompanyChartService : IServiceBase<CompanyChart>
    {
        IQueryable<CompanyChart> GetCompanyChart(int? id);
        int Delete(int id, string userName);
        void SetCompanyChartInfo(string userName);
    }
}
