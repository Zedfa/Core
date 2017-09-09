using Core.Cmn.Attributes;
using System;
using System.Threading;

namespace Core.Cmn
{
    [Injectable(DomainName = "core", InterfaceType = typeof(IRequest), LifeTime = LifetimeManagement.PerRequestLifetime)]
    public class RequestInfo : IRequest
    {

        public IUserRequest UserRequest
        {
            get;

            set;
        }
       
    }
}