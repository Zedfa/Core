using Core.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Core.Service
{
    public interface ICompanyRoleService : IServiceBase<CompanyRole>
    {
        CompanyRole Create(string roleTitle, int CompanyId, bool allowSaveChange = true);
        CompanyRole Update(CompanyRole entity, Role role, bool allowSaveChange = true);

        //List<ViewElement> GetRootViewElementsDependOnCompany(int? id);
        CompanyRole GetCompanyRoleByRoleId(int roleId);
        IQueryable<CompanyRole> All_Role_Company();
    }
}
