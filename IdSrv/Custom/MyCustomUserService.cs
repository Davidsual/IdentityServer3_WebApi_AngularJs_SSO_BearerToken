using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.AspNetIdentity;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.AspNetIdentity;
using Thinktecture.IdentityServer.Core.Configuration;


namespace IdSrv.Custom
{

    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString)
        {
            factory.UserService = new Registration<IUserService, MyCustomUserService>();
            factory.Register(new Registration<UserManager>());
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<Context>(resolver => new Context(connString)));
        }
    }

    public class MyCustomUserService : AspNetIdentityUserService<User, string>
    {
        private readonly UserManager _userManager;
        public MyCustomUserService(UserManager userMgr)
            : base(userMgr)
        {
            _userManager = userMgr;
            _userManager.UserLockoutEnabledByDefault = true;
            _userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
            _userManager.UserLockoutEnabledByDefault = true;
            _userManager.DefaultAccountLockoutTimeSpan = new TimeSpan(0, 1, 0);
            
        }

        protected override Task<AuthenticateResult> AccountCreatedFromExternalProviderAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return null;
        }
        public override Task<AuthenticateResult> AuthenticateExternalAsync(ExternalIdentity externalUser, SignInMessage message)
        {
            return null;
        }
        protected override Task<AuthenticateResult> ProcessNewExternalAccountAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            return null;
        }

        public override async Task<AuthenticateResult> AuthenticateLocalAsync(string username, string password, SignInMessage message = null)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user != null)
            {               
                if (await _userManager.IsLockedOutAsync(user.Id))
                {                    
                    return new AuthenticateResult("User Locked out");
                }

                if(await _userManager.CheckPasswordAsync(user, password))
                {
                    _userManager.ResetAccessFailedCount(user.Id);
                    var res =  new AuthenticateResult(user.Id, user.UserName, await base.GetClaimsFromAccount(user), "idsrv", null);
                    return res;
                }

                await _userManager.AccessFailedAsync(user.Id);
                return null;
            }

            return null;
        }          
    }
}
