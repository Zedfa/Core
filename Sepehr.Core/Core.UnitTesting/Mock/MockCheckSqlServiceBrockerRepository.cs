using Core.Cmn.Attributes;
using Core.Cmn.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitTesting.Mock
{
    [Injectable(InterfaceType = typeof(ICheckSqlServiceBrockerRepository))]
    public class MockCheckSqlServiceBrockerRepository : ICheckSqlServiceBrockerRepository
    {
        public void CheckServiceBrokerOnDb()
        {
            // throw new NotImplementedException();
        }
    }
}
