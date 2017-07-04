using System.Linq;
using Core.Rep;
using Core.Cmn;
using Core.Entity;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ICompanyService), DomainName = "Core")]
    public class CompanyService : ServiceBase<Company>, ICompanyService
    {

        private CompanyRepository _companyRepository;

        public CompanyService(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {

            _companyRepository = new CompanyRepository(dbContextBase);
            
        }
        public void SetCompany(int currentCompanyId)
        {

            var founded = Find(currentCompanyId);
            if (founded != null)
            {
                appBase.CompanyId = currentCompanyId;
                appBase.CompanyName = founded.Title;
                appBase.NationalId = founded.NationalId;
                //appBase.CompanyDomainName = founded.DomainName;
            }
        }


        public int DeleteWithRoles(Company model)
        {

            _companyRepository.DeleteWithRoles(model);
            return 0;

        }

      
    }
}
