using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn.Extensions;
using Core.Cmn;

namespace Core.Rep
{
    public class CompanyRoleRepository : RepositoryBase<CompanyRole>
    {

        public CompanyRoleRepository(IDbContextBase dbContextBase, Core.Entity.IUserLog userLog)
            : base(dbContextBase, userLog)
        {


        }


        public CompanyRole Create(string roleTitle, int CompanyId, bool allowSaveChange)
        {

            var role = new Role()
            {
                Name = roleTitle,

            };
            var CompanyRole = new CompanyRole()
            {
                Role = role,
                CurrentCompanyId = CompanyId,
                CompanyId = CompanyId

            };
            ContextBase.Set<CompanyRole>().Add(CompanyRole);
            if (allowSaveChange)
                SaveChanges();

            return CompanyRole;
        }

        public CompanyRole Update(CompanyRole entity, Role role, bool allowSaveChange = true)
        {

            if (allowSaveChange)
                SaveChanges();


            return entity;

        }
        public CompanyRole GetCompanyRoleByRoleId(int roleId)
        {
            return All().Include(companyRole => companyRole.Role).FirstOrDefault(companyRole => companyRole.RoleId == roleId);
        }


        public IQueryable<CompanyRole> All_Role_Company()
        {
            return All().Include(companyRole => companyRole.Role).Include(companyRole => companyRole.Company);
        }



        //public List<ViewElement> GetRootViewElementsDependOnCompany(int? id)
        //{
        //    List<ViewElement> vmList = new List<ViewElement>();
        //    //            var currentCompany = UserLog.GetCompanyId();
        //    //return DbSet.Where(a => a.CompanyId == currentCompany).Select(a => a.ViewElement).Where(a => a.ParentId == id && a.IsHidden != true && (!a.InVisible)).Include("ChildViewElement").ToList();
        //    var companyId = UserLog.GetCompanyId().Value;
        //    var allViewElementsBasedOnCompany = All().Include(item => item.Role.ViewElementRoles.Select(x => x.ViewElement.ChildViewElement))
        //        .

        //       // .Where(x => x.CompanyId == companyId).Select(t => t.Role.ViewElementRoles.Select(vm => vm.ViewElement));
                   
        //        //.Where(item=>item.Role.ViewElementRoles.Select(x => x.ViewElement.ParentId==id && x.ViewElement.IsHidden!=true && x.ViewElement.InVisible==false));




        //    List<ViewElement> viewElemntsList = new List<ViewElement>();
        //    foreach (var list in allViewElementsBasedOnCompany)
        //    {
        //        foreach (var items in list)
        //        {
        //            foreach (var item in items)
        //            {
        //                viewElemntsList.Add(item); 
        //            }
                   
        //            //foreach (var item1 in item)
        //            //{
        //            //     viewElemntsList.Add(item1);
        //            //}
                   


        //        }

        //    }

        //    return viewElemntsList;
        //}
    }
}
