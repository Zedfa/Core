using System.Collections.Generic;
using System.Linq;
using Core.Service;
using Core.Entity;
using Core.Rep.DTO.UserRoleDTO;

namespace Core.Service
{
    public interface IUserRoleService : IServiceBase<UserRole>
    {
        IQueryable<UserRoleDTO> GetAllUserRolesDto();
        int Update(int newRoleId, int oldRoleId, int userId, bool allowSaveChange = true);
        IQueryable<UserRole> All(bool canUseCacheIfPossible = true);
        IQueryable<UserRole> GetRolesByUserId(int userId);
        bool HasDuplicateRoleAssigne(int selectedRoleId, int userId);
        //List<User> GetAllKishUsers();
    }
}
