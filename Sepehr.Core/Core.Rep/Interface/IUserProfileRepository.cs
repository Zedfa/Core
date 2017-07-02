using Core.Entity;
using Core.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep.Interface
{
    public interface IUserProfileRepository :IRepositoryBase<UserProfile>
    {
        //bool IsUserActive(string userName);
        //bool HasAdminRecord();
    }
}
