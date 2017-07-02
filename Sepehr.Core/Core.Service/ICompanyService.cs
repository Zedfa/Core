using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service;
using Core.Entity;


namespace Core.Service
{

    public interface ICompanyService : IServiceBase<Company>
    {
        void SetCompany(int currentCompanyId);
        int DeleteWithRoles(Company model);
    }
}
