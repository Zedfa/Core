using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service;
using Core.Entity;
using Core.Rep.DTO;

namespace Core.Service
{
    public interface IUserProfileService : IServiceBase<UserProfile>
    {
        //void AddOnlineUsers(string userName);
        void AddOnlineUsers(UserProfile userProfile);
        void RemoveOnlineUsers(string userName);
        UserProfile GetUserProfile(string userName);
      
        //bool ValidateUser(UserProfileDTO info);
        bool ValidateUser(UserProfile info);
        string GetMd5Hash(string value);

        //IQueryable<UserProfileDTO> GetAllUserProfileDTO(bool canUseCacheIfPossible = true);

    }
}
