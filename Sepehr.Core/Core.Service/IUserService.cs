using System.Collections.Generic;
using System.Linq;
using Core.Service;
using Core.Entity;
using System;
using Core.Rep.DTO;

namespace Core.Service
{
    public interface IUserService : IServiceBase<User>
    {
        User FindUserAndUserProfileWithUserId(int userId);
        List<UserDTO> GetAllUsersExceptedAdmin();
        User GetUserAndUserProfileByUserId(int userId);
        bool IsDuplicateUserName(string userName, int userId);

        IList<Role> FindUserRoles(int userId);
        string GetMd5Hash(string password);
        int DeleteWithRoles(User model);
        bool HasAdminRecord();

        void SetCountOfLogin(int userId);
        int GetCountOfLogin(int id);
        bool IsUserActive(string userName);
        IQueryable<User> GetAllHeadUsers();
        IQueryable<User> GetAllNormalUsers();

        //int UpdateHeaderUser(User user, int currentOrganizationId, bool allowSaveChange = true);

        User GetUser(string userName, string password);
       
    }
}
