using Core.Cmn;
using Core.Cmn.Attributes;
using Core.Entity;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    [Injectable(InterfaceType = typeof(IDomainAuthenticationService), DomainName = "core")]
    public class DomainAuthenticationService : ServiceBase<UserProfile>
                                              /*ToDo: chon mikhastam az method e cache estefade konam va ServiceBase ghabeliate non generic ro nadasht majboor shodam felan ba type e user poresh konam  */,
        IDomainAuthenticationService
    {
        public DomainAuthenticationService(IDbContextBase context) : base(context)
        {

        }
        public DomainAuthenticationService() : base()
        {

        }
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

        public bool ValidateUser(UserProfile info)
        {

            var result = false;
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, info.DCName, null, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer, info.UserName, info.Password))
                {
                    result = context.ValidateCredentials(info.UserName, info.Password);
                }
            }
            catch (Exception ex)
            {
                Core.Cmn.AppBase.LogService.Handle(ex, $"validate user: {info.UserName} in domain:{info.DCName} was failed.");
            }
            return result;
        }



        [Cacheable(
            EnableSaveCacheOnHDD = true,
            EnableCoreSerialization = true,
            AutoRefreshInterval = 1800 /* 30 min*/,
            CacheRefreshingKind = Cmn.Cache.CacheRefreshingKind.Slide,
            EnableUseCacheServer = true
            )]
        public static bool UpdateDCUser()
        {
            var userProfileService = DependencyInjectionFactory.CreateInjectionInstance<IUserProfileService>();

            var domainAuthenticationService = DependencyInjectionFactory.CreateInjectionInstance<IDomainAuthenticationService>();
            //user hayi az DC ke login kardand vali password e DC anha taghir karde ra migirad 
            //meghdare DCPassword ra dar cache null mikonad ta user az halate motabr boodan kharej shavad 


            var DCUserProfileList = userProfileService.All()
                .Where(profile =>
                       profile.IsDCUser && !string.IsNullOrEmpty(profile.DCPassword)).ToList()
                       .Select(profile =>
                       {

                           var info = new UserProfile()
                           {
                               UserName = profile.UserName,
                               Password = profile.DCPassword
                           };
                           //if (!domainAuthenticationService.ValidateUser(info))
                           if (!userProfileService.ValidateUser(info))
                               profile.DCPassword = null;
                           return profile;
                       });


            return DCUserProfileList.Count() > 0;

        }


    }
}
