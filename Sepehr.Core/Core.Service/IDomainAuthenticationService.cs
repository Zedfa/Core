using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IDomainAuthenticationService
    {
        bool Logon(string userName, string password, string domain, out string fullName, out List<string> roles);
    }
}
