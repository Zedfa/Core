using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;

using Core.Rep;
using Core.Rep.DTO.UserRoleDTO;
using Core.Cmn.Attributes;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IUserRoleService), DomainName = "Core")]
    public class UserRoleService : ServiceBase<UserRole>, IUserRoleService
    {

        private UserRoleRepository _userRoleRepository;


        public UserRoleService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new UserRoleRepository(ContextBase, userLog);
            _userRoleRepository = new UserRoleRepository(ContextBase, userLog);
        }
        public IQueryable<UserRoleDTO> GetAllUserRolesDto()
        {
            return (_repositoryBase as UserRoleRepository).GetAllUserRolesDto();
        }

        public IQueryable<UserRole> GetRolesByUserId(int userId)
        {

            return _userRoleRepository.GetRolesByUserId(userId);
        }



        public int Update(int newRoleId, int oldRoleId, int userId, bool allowSaveChange = true)
        {

            return _userRoleRepository.Update(newRoleId, oldRoleId, userId, allowSaveChange);

        }

        public IQueryable<UserRole> All(bool canUseCacheIfPossible = true)
        {
            return _userRoleRepository.All();
        }

        public bool HasDuplicateRoleAssigne(int selectedRoleId, int userId)
        {
            return _userRoleRepository.HasDuplicateRoleAssigne(selectedRoleId, userId);

        }

        //public List<User> GetAllKishUsers()
        //{
        //    var supplierInfoList = ContextBase.Set<SupplierInfo>();
        //    var usersList = _userRoleRepository.GetKishUsers();



        //    var kishUsers = from user in usersList
        //                    where !supplierInfoList.Any(supplier => supplier.UserID == user.Id)
        //                    select user;
        //    return kishUsers.ToList();
        //}
    }
}
