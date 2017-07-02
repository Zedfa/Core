using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Rep;
using Core.Cmn;
using Core.Entity;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ICompanyRoleService), DomainName = "Core")]
    public class CompanyRoleService : ServiceBase<CompanyRole>, ICompanyRoleService
    {

        public CompanyRoleService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new CompanyRoleRepository(dbContextBase, userLog);

        }
        //public List<ViewElement> GetRootViewElementsDependOnCompany(int? id)
        //{
        //    return (_repositoryBase as CompanyRoleRepository).GetRootViewElementsDependOnCompany(id);
        //}
        public CompanyRole Create(string roleTitle, int organizationId, bool allowSaveChange)
        {
            return (_repositoryBase as CompanyRoleRepository).Create(roleTitle, organizationId, allowSaveChange);
        }

        public CompanyRole Update(CompanyRole entity, Role role, bool allowSaveChange = true)
        {

            return (_repositoryBase as CompanyRoleRepository).Update(entity, role, allowSaveChange);
        }
        public CompanyRole GetCompanyRoleByRoleId(int roleId)
        {

            return (_repositoryBase as CompanyRoleRepository).GetCompanyRoleByRoleId(roleId);
        }
        public IQueryable<CompanyRole> All_Role_Company()
        {
            return (_repositoryBase as CompanyRoleRepository).All_Role_Company();
        }

    }
}
