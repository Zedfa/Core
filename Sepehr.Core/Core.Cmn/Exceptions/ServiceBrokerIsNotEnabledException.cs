using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Exceptions
{
    public class ServiceBrokerIsNotEnabledException : CoreExceptionBase
    {
        public ServiceBrokerIsNotEnabledException(string dbName)
        {
            DbName = dbName;
        }
        public string DbName { get; private set; }
        public override string Message
        {
            get
            {
                return $"'Service Broker' for DataBase '{DbName}' is not enabled. Cache needs to use SqlDependency which uses 'Service Broker'.";
            }
        }
    }
}
