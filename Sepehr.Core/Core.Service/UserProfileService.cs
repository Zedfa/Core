using System.Linq;
using Core.Entity;
using Core.Cmn;
using Core.Rep;
using Core.Cmn.Attributes;
using System.Security.Cryptography;
using System.Text;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IUserProfileService), DomainName = "Core")]
    public class UserProfileService : ServiceBase<UserProfile>, IUserProfileService
    {
        private readonly IDomainAuthenticationService _domainAuthenticationService;
        public UserProfileService(IDbContextBase dbContextBase, IDomainAuthenticationService domainAuthenticationService)
            : base(dbContextBase)
        {
            _repositoryBase = new UserProfileRepository(dbContextBase);
            _domainAuthenticationService = domainAuthenticationService;
        }

        public UserProfileService(IDbContextBase dbContextBase, IUserLog userLog, IDomainAuthenticationService domainAuthenticationService)
            : base(dbContextBase, userLog)
        {
            _repositoryBase = new UserProfileRepository(dbContextBase, userLog);
            _domainAuthenticationService = domainAuthenticationService;

        }

        public void AddOnlineUsers(UserProfile userProfile)
        {
            lock (appBase.OnlineUsers)
            {

                if (appBase.OnlineUsers.ToList().Any(u => u.UserName.ToLower().Equals(userProfile.UserName)))
                {
                    //Maybe same users loged in together...
                    return;
                }
                appBase.OnlineUsers.Add(userProfile);
            }
        }

        public void RemoveOnlineUsers(string userName)
        {
            appBase.OnlineUsers.RemoveAll(p => p.UserName.ToLower().Equals(userName.ToLower()));
        }

        public override IQueryable<UserProfile> Filter(System.Linq.Expressions.Expression<System.Func<UserProfile, bool>> predicate, bool allowFilterDeleted = true)
        {
            return (_repositoryBase as UserProfileRepository).Filter(predicate, allowFilterDeleted);
        }


        public UserProfile GetUserProfile(string userName)
        {
            return (_repositoryBase as UserProfileRepository).GetUserProfile(userName);
        }


        public bool ValidateUser(UserProfile info)
        {
           // info.UserName = info.UserName.StartsWith("@")? $"{info.UserName.Remove(0, 1)}@{GeneralConstant.SepehrSystemsDC}": info.UserName;
           
            //var userProfileDTO = (_repositoryBase as UserProfileRepository).GetAllUserProfileDTO().FirstOrDefault(entity => entity.UserName.Equals(info.UserName));
            var userProfile = (_repositoryBase as UserProfileRepository).All().FirstOrDefault(entity => entity.UserName.Equals(info.UserName));

            var result = false;

            if (userProfile != null)
            {
                //if user in DC
                if (info.IsDCUser)
                {
                    if (userProfile.DCPassword == null)
                    {
                        if (_domainAuthenticationService.ValidateUser(info))
                        {
                            userProfile.DCPassword = info.Password;
                            // userProfileDTO.IP = info.IP;
                            result = true;
                        }
                    }
                    else
                    {
                        if (userProfile.DCPassword == info.Password)
                        {
                            //if (userProfileDTO.IP.Equals(info.IP))
                            result = true;
                        }
                    }
                }
                // if user in DB
                else
                {
                    var md5Hash = GetMd5Hash(info.Password);
                    result = userProfile.Password.Equals(md5Hash);
                }
            }

            return result;

        }
        public string GetMd5Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        //public UserProfile GetDCUserProfile(string userName)
        //{
        //    var DCUserProfile = All()
        //         .Where(profile =>
        //                profile.IsDCUser && !string.IsNullOrEmpty(profile.DCPassword &&  )).ToList()
        //                .Select(profile =>
        //                {

        //                    var info = new UserProfile()
        //                    {
        //                        UserName = profile.UserName,
        //                        Password = profile.DCPassword
        //                    };
        //                    //if (!domainAuthenticationService.ValidateUser(info))
        //                    //    profile.DCPassword = null;
        //                    return profile;
        //                });
        //    return DCUserProfile;
        //}

        //public IQueryable<UserProfileDTO> GetAllUserProfileDTO(bool canUseCacheIfPossible = true)
        //{
        //    return (_repositoryBase as UserProfileRepository).GetAllUserProfileDTO(canUseCacheIfPossible);
        //}
    }
}
