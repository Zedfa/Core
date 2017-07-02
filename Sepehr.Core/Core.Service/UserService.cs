using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Rep;
using System.Security.Cryptography;
using System.Text;
using Core.Rep.DTO;
using Core.Cmn.Attributes;
using Core.Cmn;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IUserService), DomainName = "core")]
    public class UserService : ServiceBase<User>, IUserService
    {


        public UserService(IDbContextBase dbContextBase, IUserLog userLog)
            : base(dbContextBase, userLog)
        {
           _repositoryBase = new UserRepository(dbContextBase, userLog);


        }
        public UserService()
            : base()
        {
            _repositoryBase = new UserRepository();
        }


        public bool IsDuplicateUserName(string userName, int userId)
        {

            return (_repositoryBase as UserRepository).IsDuplicateUserName(userName, userId);
        }

        public User GetUserAndUserProfileByUserId(int userId)
        {
            return (_repositoryBase as UserRepository).GetUserAndUserProfileByUserId(userId);
        }
        public override int Update(User user, bool allowSaveChange = true)
        {

            return (_repositoryBase as UserRepository).Update(user, allowSaveChange);
        }
        //public int UpdateHeaderUser(User user, int currentOrganizationId, bool allowSaveChange = true)
        //{
        //    return (_repositoryBase as UserRepository).UpdateHeaderUser(user, currentOrganizationId, allowSaveChange);
        //}

        public IList<Role> FindUserRoles(int userId)
        {
            var _userRoles = (_repositoryBase as UserRepository).FindUserById(userId);
            return _userRoles.UserRoles.Select(a => a.Role).ToList();


        }

        public User FindUserAndUserProfileWithUserId(int userId)
        {
            return (_repositoryBase as UserRepository).FindUserAndUserProfileWithUserId(userId);
        }



        public string GetMd5Hash(string password)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public int DeleteWithRoles(User model)
        {

            (_repositoryBase as UserRepository).DeleteWithRoles(model);
            return 0;

        }

        public bool HasAdminRecord()
        {

            return (_repositoryBase as UserRepository).HasAdminRecord();
        }


        public void SetCountOfLogin(int userId)
        {

            (_repositoryBase as UserRepository).SetCountOfLogin(userId);

        }

        public int GetCountOfLogin(int id)
        {
            var foundeduser = Find(id);
            return foundeduser.Count;
        }

        public bool IsUserActive(string userName)
        {
            return (_repositoryBase as UserRepository).IsUserActive(userName);
        }

        public IQueryable<User> GetAllHeadUsers()
        {
            return (_repositoryBase as UserRepository).GetAllHeadUsers();
        }
        public List<UserDTO> GetAllUsersExceptedAdmin()
        {
            return (_repositoryBase as UserRepository).GetAllUsersExceptedAdmin();
        }
        public IQueryable<User> GetAllNormalUsers()
        {
            return (_repositoryBase as UserRepository).GetAllNormalUsers();
        }


        public override User Create(User user, bool allowSaveChange = true)
        {

            return (_repositoryBase as UserRepository).Create(user, allowSaveChange);


        }





        public User GetUser(string userName, string password)
        {

            return (_repositoryBase as UserRepository).GetUser(userName, password);
        }


    }
}
