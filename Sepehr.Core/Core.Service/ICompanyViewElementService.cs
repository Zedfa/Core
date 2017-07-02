using System.Collections.Generic;
using Core.Entity;


namespace Core.Service
{
    public interface ICompanyViewElementService : IServiceBase<CompanyViewElement>
    {

        int Create(List<int> inseretedOrganizationViewElement, List<int> deletedOrganizationViewElement, int organizationId, bool allowSaveChange = true);
        IList<ViewElement> GetViewElement(int? id);

    }
}
