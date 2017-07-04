
using System.Collections.Generic;
using System.Linq;
using Core.Cmn.Extensions;
using Core.Ef;
using Core.Entity;
using System.Data.SqlClient;
using Core.Cmn;
using System.Security.Cryptography;
using Core.Cmn.Attributes;
using Core.Rep.DTO;
using Core.Rep.Interface;

namespace Core.Rep
{

    public class UserRepository : RepositoryBase<User> 
    {
        private IDbContextBase _dbContext;
        private IUserProfileRepository _userProfileRepository;
        public UserRepository()
            : base()
        {

            _dbContext = ContextBase;
            _userProfileRepository = AppBase.DependencyInjectionManager.Resolve<IUserProfileRepository>();
        }
       
        public UserRepository(IDbContextBase dbContextBase)
            : base(dbContextBase)
        {
            _dbContext = dbContextBase;
            _userProfileRepository = AppBase.DependencyInjectionManager.Resolve<IUserProfileRepository>();

        }
        

        public User GetUser(string userName, string password)
        {
            // UserService userService = new UserService();
            var md5Password = Core.Cmn.Security.GetMd5Hash(MD5.Create(), password);
            var user = ContextBase.Set<User>().FirstOrDefault(r =>
                r.UserProfile.UserName.ToLower() == userName.ToLower() &&
                r.UserProfile.Password == md5Password);

            return user;
        }
        public override User Create(User t, bool allowSaveChange = true)
        {
            //choon hanooz malom nist mikhaym multi company bashim ya na hameye user ha be sepehr system vasl mishan
            t.CurrentCompanyId= t.CurrentCompanyId ?? 1;
            t.CompanyChartId = t.CompanyChartId ?? 1;
            return base.Create(t, allowSaveChange);
        }
        public override int Update(User user, bool allowSaveChange = true)
        {
            var foundedUser = ContextBase.Set<User>().AsNoTracking().Include("UserProfile").First(u => u.Id == user.Id);

            foundedUser.FName = user.FName;
            foundedUser.CompanyChartId = user.CompanyChartId;
            foundedUser.LName = user.LName;
            foundedUser.UserProfile.UserName = user.UserProfile.UserName;
            foundedUser.UserProfile.Password = user.UserProfile.Password;
            // findedUser.Email = user.Email;
            foundedUser.Active = user.Active;
            foundedUser.CurrentCompanyId = user.CurrentCompanyId;


            if (allowSaveChange)
                return SaveChanges();

            return 0;
        }

        public User FindUserAndUserProfileWithUserId(int userId)
        {
            return All().Include(user => user.UserProfile).First(user => user.Id == userId);
        }

        //public int UpdateHeaderUser(User user, int currentOrganizationId, bool allowSaveChange = true)
        //{


        //    var CurrentUser = ContextBase.Set<User>().Find(user.Id);
        //    _dbContext.Entry(CurrentUser).CurrentValues.SetValues(user);
        //    foreach (var role in user.UserRoles.ToList())
        //    {

        //        var rep = new UserRoleRepository(_dbContext, UserLog);
        //        rep.Update(role, false);


        //    }


        //    if (allowSaveChange)
        //        return SaveChanges();
        //    return 0;


        //}

        public bool IsDuplicateUserName(string userName, int userId)
        {
            var useName = userName.Trim().CorrectPersianChars();
            return ContextBase.Set<User>().Any(a => a.UserProfile.UserName == useName && a.Id != userId);

        }

        public void DeleteWithRoles(User user)
        {
            var findedUser = _dbContext.Set<User>().AsNoTracking().Include("UserProfile").FirstOrDefault(a => a.Id == user.Id);
            if (findedUser != null)
            {
                LogicalDelete(findedUser, true);
            }

            //var userRoles = findedUser.UserRoles;


            //Update(findedUser);
        }

        public bool HasAdminRecord()
        {

            return ContextBase.Set<UserProfile>().Any(a => a.UserName == GeneralConstant.AdminUserName);
        }

        //Add for PUblish...
        public void SetCountOfLogin(int userId)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter()
            {
                ParameterName = "@UserId",
                SqlValue = 1
            });
            ExecuteSpRp.ExecuteSp(GeneralSpName.SpUpdateCountOfUserLogin, sqlParams, ContextBase);
        }

        public bool IsUserActive(string userName)
        {

            var foundedUserProfile =_userProfileRepository.All()
                .Where(a => a.UserName.ToLower() == userName.ToLower()).SingleOrDefault();

            return foundedUserProfile !=null ? foundedUserProfile.User.Active: false;
        }

        public IQueryable<User> GetAllHeadUsers()
        {
            var userRolelist = ContextBase.Set<UserRole>();
            var userlist = ContextBase.Set<User>();
            var rolelist = ContextBase.Set<Role>();

            var query = from a in userlist
                        join b in userRolelist on a.Id equals b.UserId
                        join c in rolelist on b.RoleID equals c.ID
                        select a;

            if (query.Any())
            {
                return query;
            }
            return null;
        }
        public List<UserDTO> GetAllUsersExceptedAdmin()
        {
            return All()
                .Where(x => x.UserProfile.UserName != GeneralConstant.AdminUserName)
                .Select(user => new UserDTO
                {
                    FName = user.FName,
                    LName = user.LName,
                    UserName = user.UserProfile.UserName,
                    Password = user.UserProfile.Password,
                    ConfirmPassword = user.UserProfile.Password,
                    Active = user.Active,
                    Id = user.Id

                }).ToList();
        }

        public IQueryable<User> GetAllNormalUsers()
        {
            //var currentOrganizationId = UserLog.GetCompanyId();
            //var userRolelist = ContextBase.Set<UserRole>();
            //var userlist = ContextBase.Set<User>();
            //var rolelist = ContextBase.Set<Role>();
            var users = _dbContext.Set<User>();

            var n = All().Include(c => c.UserProfile);
            var k = All().Include(c => c.UserProfile)
                .Where(x => x.UserProfile.UserName != GeneralConstant.AdminUserName);

            return null;
        }
        public User FindUserById(int id)
        {
            return All_UsersRole_Role_ViewElementRoles_ViewElement()
                .FirstOrDefault(user => user.Id.Equals(id));

        }
        [Cacheable(EnableUseCacheServer = false, ExpireCacheSecondTime = 100, EnableAutomaticallyAndPeriodicallyRefreshCache = true)]
        public static IQueryable<User> All_UsersRole_Role_ViewElementRoles_ViewElement_Cache(IQueryable<User> query)
        {
            return query.Include(element => element.UserRoles)
               .Include(element => element.UserProfile)
                  .Include(element => element.UserRoles.Select(userroles => userroles.Role))
                  .Include(element => element.UserRoles.Select(userroles => userroles.Role.ViewElementRoles))
                  .Include(element => element.UserRoles.Select(userroles => userroles.Role.ViewElementRoles.Select(viewelemntrole => viewelemntrole.ViewElement)));
        }
        public IQueryable<User> All_UsersRole_Role_ViewElementRoles_ViewElement(bool canUseCacheIfPossible = true)
        {
            return Cache(All_UsersRole_Role_ViewElementRoles_ViewElement_Cache, canUseCacheIfPossible);
        }
        public IQueryable<User> All_UserProfile(bool canUseCacheIfPossible = true)
        {
            return All().Include(user => user.UserProfile);
        }

        public User GetUserAndUserProfileByUserId(int userId)
        {
            return All_UserProfile().FirstOrDefault(user => user.Id == userId);
        }

    }
}