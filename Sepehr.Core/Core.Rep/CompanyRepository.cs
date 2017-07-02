using Core.Cmn;
using Core.Entity;
using System.Linq;


namespace Core.Rep
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {

        }
        public void DeleteWithRoles(Company company)
        {
            var findedOrgRole = Find(company.Id);


            LogicalDelete(findedOrgRole, false);
            // var companyRoles = findedOrgRole.CompanyRoles;


            //وقتی سازمان را حذف می کنیم نقش ها ی تعریف شده در این سازمان را هم باید حذف کنیم
            var role = ContextBase.Set<Role>();
            var findedRole = role.Where(x => x.CurrentCompanyId == company.Id);

            Update(findedOrgRole);

        }
        

    }
}
