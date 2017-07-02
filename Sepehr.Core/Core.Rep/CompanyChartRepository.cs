using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Core.Cmn.Extensions;
using Core.Entity;
using Core.Cmn;
using Core.Ef;

namespace Core.Rep
{
    public class CompanyChartRepository :RepositoryBase<CompanyChart>, ICompanyChartRepository
    {

        private IUserLog _userLog;
       

        public CompanyChartRepository(IDbContextBase dbContext,IUserLog userLog)
            : base(dbContext, userLog)
        {
            _userLog = userLog;
        }

        public IQueryable<CompanyChart> GetCompanyChart(int? id)
        {
           // var currentCMPchartId = UserLog.GetCompanyId();
            return ContextBase.Set<CompanyChart>().Where(a => a.ParentId == id).Include("ChildCompanyChart").Include("CompanyChartRoles");
      
        }

        public int Delete(int id)
        {
            var dtResult = CheckRelationBeforeDelete(string.Format("{0}.{1}", Schema, TableName), KeyName, id.ToString());
            if (dtResult.Count >= 1)
            {
                var s = new StringBuilder();
                s.Append(" این رکورد در جداول");
                s.Append("<br/>");
                foreach (var item in dtResult)
                {
                  s.Append("<br/>");
                  s.Append(item.inUsedTbName);
                  
                }
                s.Append("در حال استفاده می باشد");
                throw new Exception(s.ToString(), new Exception(s.ToString(), null));
            }

            var uName = _userLog.GetUserName();
            var userProfile =
             ContextBase.Set<UserProfile>().SingleOrDefault(a => a.UserName.ToLower() == uName);

            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter( )
            {
                  ParameterName = "@companyId",
              SqlValue = id
            });

            sqlParams.Add(new SqlParameter()
            {
                ParameterName = "@currentUserId",
                SqlValue = userProfile.User.Id
            });


            return (int)ExecuteSpRp.ExecuteSp(GeneralSpName.DeleteChartCompany, sqlParams,  ContextBase);
            // return  _dbContextBase.Database.ExecuteSqlCommand("EXEC DeleteChartOrganization @organizationId", ietsParameter);

        }
    }
}




    