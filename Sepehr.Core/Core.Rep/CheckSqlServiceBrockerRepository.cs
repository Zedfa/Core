using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Cmn.Exceptions;
using Core.Cmn.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Rep
{
    [Injectable(InterfaceType =typeof(ICheckSqlServiceBrockerRepository))]
    public class CheckSqlServiceBrockerRepository : ICheckSqlServiceBrockerRepository
    {
        public void CheckServiceBrokerOnDb()
        {
            var dbName = Core.Cmn.AppBase.DependencyInjectionFactory.CreateContextInstance().Database.Connection.Database;
            var query = $"Select is_broker_enabled from sys.databases where name = '{dbName}'";
            var isServiceBrokerEnabled = Core.Cmn.AppBase.DependencyInjectionFactory.CreateContextInstance().Database.SqlQueryForSingleResult<bool>(query);
            if (!isServiceBrokerEnabled)
            {
                throw new ServiceBrokerIsNotEnabledException(dbName);
            }
        }
    }
}
