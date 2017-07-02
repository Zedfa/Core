using Core.Cmn.Attributes;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IRoleService), DomainName = "Core")]
    public class RoleService : ServiceBase<Role>, IRoleService
    {
        public RoleService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new RoleRepository(dbContextBase, userLog);


        }
        public IQueryable<RoleDTO> GetAllRoleDTO()
        {
            return (_repositoryBase as RoleRepository).GetAllRoleDTO();
        }
       public Role FindRoleWithRoleHotelAndRoleDestinationByRoleId(int roleId)
        {
            return (_repositoryBase as RoleRepository).FindRoleWithRoleHotelAndRoleDestinationByRoleId(roleId);
        }
        public Role GetRoleByTitle(string roleTitle)
        {
            return (_repositoryBase as RoleRepository).GetRoleByTitle(roleTitle);
        }
        public IQueryable<Role> GetAllRolesByCompany(int userId) {
            return (_repositoryBase as RoleRepository).GetAllRolesByCompany(userId);
        }
        
    }
}
