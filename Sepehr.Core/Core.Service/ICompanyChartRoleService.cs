using System.Collections.Generic;
using System.Linq;
using Core.Service;
using Core.Entity;
using System;

namespace Core.Service
{
    public interface ICompanyChartRoleService : IServiceBase<CompanyChartRole>
    {
        int Create(List<int> inseretedRole, List<int> deleteRoles, int CompanyChartId, bool allowSaveChange = true);

        IQueryable<Role> GetRoles(int CompanyChartId);
        //IQueryable<Role> GetRoles(Guid userId);

        IList<Role> GetRoleOfRelatedChildOfChartNode(int parentId);

    }
}
