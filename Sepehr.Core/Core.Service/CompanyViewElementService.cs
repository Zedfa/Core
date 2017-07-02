using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ICompanyViewElementService), DomainName = "Core")]
    public class CompanyViewElementService : ServiceBase<CompanyViewElement>, ICompanyViewElementService
    {
        //private CompanyViewElementRepository _companyViewElementRepository;
        private IDbContextBase _dbcontcontext;

        public CompanyViewElementService(IDbContextBase appContextBase, IUserLog userLog)
            : base(appContextBase, userLog)
        {

            _repositoryBase = new CompanyViewElementRepository(appContextBase, userLog);
            _dbcontcontext = appContextBase;
        }
       
        public int Create(List<int> inseretedCompanyViewElement, List<int> deletedCompanyViewElement, int companyId, bool allowSaveChange = true)
        {

            var viewlementCompany = (_repositoryBase as CompanyViewElementRepository).Create(inseretedCompanyViewElement, deletedCompanyViewElement, companyId, allowSaveChange);

            var viewElementRoleRepo = new ViewElementRoleRepository(_dbcontcontext, UserLog);

            var _companyService = new CompanyService(_dbcontcontext, UserLog);
            var foundedCMP = _companyService.Find(companyId);
            var CompanyRole = foundedCMP.CompanyRoles.FirstOrDefault();
            viewElementRoleRepo.Create(inseretedCompanyViewElement, deletedCompanyViewElement, CompanyRole.RoleId, true);
            return viewlementCompany;
        }

        public IList<ViewElement> GetViewElement(int? id)
        {
            return (_repositoryBase as CompanyViewElementRepository).GetViewElementList(id);
        }

    }
}
