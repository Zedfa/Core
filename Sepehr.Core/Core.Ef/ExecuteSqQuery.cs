using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Ef
{
    public static class ExecuteSqQuery<T> where T : new()
    {
        public static IList<T> ExecuteSpForList(string spName, IList<SqlParameter> sqlParameters, DbContextBase contextBase)
        {
            var sqlParamNames = new StringBuilder("exec " + spName + " ");
            var sqlParametersValue = new List<SqlParameter>();

            foreach (var sqlParameter in sqlParameters)
            {
                sqlParamNames.Append(sqlParameter.ParameterName + ",");
                sqlParametersValue.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.SqlValue));
            }

            sqlParamNames.Remove(sqlParamNames.Length - 1, 1);

            return contextBase.Database.SqlQuery<T>(sqlParamNames.ToString(), sqlParametersValue.ToArray()).ToList();
        }
    }
}
