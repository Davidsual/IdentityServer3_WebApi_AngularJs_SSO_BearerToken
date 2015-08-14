using System.Collections.Generic;
using System.Security.Claims;
using Thinktecture.IdentityServer.Core.Models;

namespace IdSrv
{
    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                /*
                // no human involved
                new Client
                {

                    ClientName = "Silicon-only Client",
                    ClientId = "silicon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 60,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AbsoluteRefreshTokenLifetime = 86400, // one day
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenExpiration  = TokenExpiration.Sliding,                    
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                    },
                    Claims = new List<Claim>() {new Claim("testClain","Davide","Potato")}
                },
                */
                // human is involved
                new Client
                {
                    ClientName = "Silicon on behalf of Carbon Client",
                    ClientId = "carbon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 60,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AbsoluteRefreshTokenLifetime = 86400, // one day
                    SlidingRefreshTokenLifetime = 43200,
                    RefreshTokenExpiration  = TokenExpiration.Sliding,
                    Flow = Flows.ResourceOwner,
                    ScopeRestrictions = new List<string>
                    {
                        StandardScopes.OfflineAccess.Name,
                        "api1",  
                        "api2"
                    },
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret("21B5F798-BE55-42BC-8AA8-0025B903DC3B".Sha256())
                    }
                }
            };
        }
    }
}