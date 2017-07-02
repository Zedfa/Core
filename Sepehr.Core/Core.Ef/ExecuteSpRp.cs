using Core.Cmn;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Ef
{
    public  static class ExecuteSpRp
    {
        
        public static object ExecuteSp(string spName,IList<SqlParameter> sqlParameters,IDbContextBase  contextBase )
        {
           
            var  sqlParameterName=new StringBuilder("");
            
            foreach (var param in sqlParameters)
            {
                sqlParameterName.Append(param.ParameterName).Append(",");
                
            }

            
               var  parametersName = sqlParameterName.Remove(sqlParameterName.Length - 1, 1);
           
            return contextBase.Database.ExecuteSqlCommand("EXEC" + " " + spName + " " + parametersName.ToString(), sqlParameters.ToArray());
        }



    }

     
}