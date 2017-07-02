using Core.Entity;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IRoleService : IServiceBase<Role>
    {
        Role FindRoleWithRoleHotelAndRoleDestinationByRoleId(int roleId);
        Role GetRoleByTitle(string roleTitle);
        IQueryable<Role> GetAllRolesByCompany(int userId);
        IQueryable<RoleDTO> GetAllRoleDTO();
    }
}
