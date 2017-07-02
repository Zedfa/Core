using System.Linq;
using Core.Entity;
using Core.Rep.DTO.UserRoleDTO;
using Core.Cmn;
using Core.Cmn.Extensions;

namespace Core.Rep
{

    public class UserRoleRepository : RepositoryBase<UserRole>
    {
        #region Variable
        IDbContextBase _dc;
        #endregion


        public UserRoleRepository(IDbContextBase dbContext, IUserLog userLog)
            : base(dbContext, userLog)
        {
            _dc = dbContext;
        }

        public override IQueryable<UserRole> All(bool canUseCacheIfPossible = true)
        {
            return _dc.Set<UserRole>().AsNoTracking().Include("Role").Include("User");
        }

        public IQueryable<UserRole> GetRoles(int userId)
        {
            return Filter(a => a.UserId == userId);
            // var userRoles = _dbContextBase.Set<UserRole>();
            // var findedUser = users.Include("Roles").Single(u => u.Id == userId);
            // Comment For Change Many To Many Struture
            //return findedUser.Roles.ToList();
            // return new List<Role>();

        }
        public IQueryable<UserRoleDTO> GetAllUserRolesDto()
        {
            return All().Select(userRole => new UserRoleDTO
            {
                RoleId = userRole.RoleID,
                RoleName = userRole.Role.Name,
                UserId = userRole.UserId
            });
        }
        public int Update(int newRoleId, int oldRoleId, int userId, bool allowSaveChange = true)
        {

            var userRole = ContextBase.Set<UserRole>();
            var oldUserRole = userRole.Where(a => a.RoleID == oldRoleId && a.UserId == userId).SingleOrDefault();
            userRole.Remove(oldUserRole);

            var newUserRole = new UserRole()
            {
                RoleID = newRoleId,
                UserId = userId,
            };
            userRole.Add(newUserRole);

            if (allowSaveChange)
                SaveChanges();
            return 0;
        }
        //public List<User> GetKishUsers()
        //{
        //    return _dc.Set<UserRole>().Where(item => item.RoleID == Constants.KishConstants.KishRole)
        //        .Select(item => item.User).ToList();
        //}
        public bool HasDuplicateRoleAssigne(int selectedRoleId, int userId)
        {
            var foundRoles = Filter(a => a.RoleID == selectedRoleId && a.UserId == userId).ToList();
            if (foundRoles.Any())
                return true;
            return false;
        }
    }
}