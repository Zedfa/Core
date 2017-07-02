using Core.Entity;

using Core.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Rep
{
    public interface ICompanyChartRepository : IRepositoryBase<CompanyChart>
    {
        IQueryable<CompanyChart> GetCompanyChart(int? id);
        int Delete(int id);
    }
}
