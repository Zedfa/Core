using System.Collections.Generic;
using System.Linq;
using Core.Entity;
using Core.Cmn;

namespace Core.Rep
{
    public class CompanyChartRoleRepository : RepositoryBase<CompanyChartRole>
    {
        
         private List<Role> resultRoles=new List<Role>();
        
         public CompanyChartRoleRepository(IDbContextBase dbContext,IUserLog userLog)
             : base(dbContext,userLog)
         {
             
         }
         public IQueryable<Role> GetRoles(int CompanyChartId)
         {

             return Filter(a => a.CompanyChartId == CompanyChartId).Select(a=>a.Role);
          
         }

         public int Create(List<int> inserRoleIdList, List<int> deletedRoleList, int oChartId, bool allowSaveChange = true)
         {

             var companyChartRole = ContextBase.Set<CompanyChartRole>();
             foreach (var roleId in inserRoleIdList)
             {
                 var findedRole = companyChartRole.Where(a => a.RoleId == roleId && a.CompanyChartId == oChartId).SingleOrDefault();
                 
                 if (findedRole != null)
                 {
                    
                     Update(findedRole, false);
                 }
                 else
                 {
                    var oChartRole = new CompanyChartRole()
                     {
                         CompanyChartId = oChartId,
                         RoleId = roleId,
                   

                     };
                     companyChartRole.Add(oChartRole);
                 }
               
               
             }
             foreach (var roleId in deletedRoleList)
             {
                 var findedRole = companyChartRole.Where(a => a.RoleId == roleId && a.CompanyChartId == oChartId).SingleOrDefault();
                 if (findedRole != null)
                 {
                     
                     Update(findedRole,false);
                 }
             }

             if (allowSaveChange)
                 SaveChanges();
                    
          
             return 0;
         }

         public IList<Role> GetRoleOfRelatedChildOfChartNode(int parentId)
         {
             var CompanyChartRepo = ContextBase.Set<CompanyChart>();
            var findedCompany = CompanyChartRepo.SingleOrDefault(o => o.Id.Equals(parentId));
             var organChilds = findedCompany.ChildCompanyChart.ToList();
       
             foreach (var child in organChilds)
             {
                 var orgRoleRepo = ContextBase.Set<CompanyChartRole>();
                 var orgRole= orgRoleRepo.Where(o => o.CompanyChartId.Equals(child.Id)).ToList();
                 resultRoles.AddRange(orgRole.Select(o => o.Role).ToList());
             }

             if (findedCompany.ChildCompanyChart != null)
                 foreach (var child in findedCompany.ChildCompanyChart)
                 {
                     var chartNod = GetRoleOfRelatedChildOfChartNode(child.Id);
                     resultRoles.AddRange(chartNod);
                 }
             return resultRoles.Distinct().ToList();
         }
    }
}