using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using System;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ICompanyChartRoleService), DomainName = "Core")]
    public class CompanyChartRoleService : ServiceBase<CompanyChartRole>, ICompanyChartRoleService
    {

        //private CompanyChartRoleRepository _companyChartRoleRepository;

        public CompanyChartRoleService(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
            _repositoryBase = new CompanyChartRoleRepository(dbContextBase);
        }
        public IQueryable<Role> GetRoles(int OrganizationChartId)
        {
            return (_repositoryBase as CompanyChartRoleRepository).GetRoles(OrganizationChartId);
        }

        public int Create(List<int> roleIdList, List<int> deletedRoleList, int OrganizationChartId, bool allowSaveChange = true)
        {
            return (_repositoryBase as CompanyChartRoleRepository).Create(roleIdList, deletedRoleList, OrganizationChartId, allowSaveChange);
        }


        public IList<Role> GetRoleOfRelatedChildOfChartNode(int parentId)
        {
            return (_repositoryBase as CompanyChartRoleRepository).GetRoleOfRelatedChildOfChartNode(parentId);
        }

    }
}
