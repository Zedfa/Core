using Core.Cmn.Attributes;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    [Injectable(InterfaceType =typeof(IDomainAuthenticationService),DomainName ="core")]
    public class DomainAuthenticationService : IDomainAuthenticationService
    {
        public bool Logon(string userName, string password, string domain, out string fullName, out List<string> roles)
        {
            roles = null;
            fullName = String.Empty;

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain, null, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer, userName, password))
                {
                    if (pc.ValidateCredentials(userName, password))
                    {
                        roles = new List<string>();
                        using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, userName))
                        {
                            fullName = userPrincipal.DisplayName;

                            //var groups = userPrincipal.GetGroups();
                            //foreach (var group in groups)
                            //{
                            //    roles.Add(group.Name.Trim().ToUpper());
                            //}

                            /// estefadeh az method GetGroups baes mishod error "Information about the domain could not be retrieved (1355)"
                            /// roye server begirim. va natonestam rahe hali ham barash pida konam, vase hamin az in method estefadeh kardam
                            DirectoryEntry de = (userPrincipal.GetUnderlyingObject() as DirectoryEntry);
                            foreach (object group in (object[])de.Properties["memberOf"].Value)
                            {
                                roles.Add(group.ToString().Split(',')[0].Split('=')[1]);
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                throw;
                //return false;
            }
        }
    }
}
