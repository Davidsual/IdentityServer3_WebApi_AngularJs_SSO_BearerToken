using System.Collections.Generic;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Models;

namespace IdSrv
{
    static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "api1",
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim
                        {
                            AlwaysIncludeInIdToken = true,
                            Name = "HaloCustomerId",
                            Description = "Halo customer id"
                        },
                        new ScopeClaim
                        {
                            AlwaysIncludeInIdToken = true,
                            Name = Constants.ClaimTypes.Role
                        }
                    }
                },
                new Scope
                {
                    Name = "api2",
                    Claims = new List<ScopeClaim>()
                    {
                        new ScopeClaim
                        {
                            AlwaysIncludeInIdToken = true,
                            Name = "HaloCustomerId",
                            Description = "Halo customer id"
                        },
                        new ScopeClaim
                        {
                            AlwaysIncludeInIdToken = true,
                            Name = Constants.ClaimTypes.Role
                        }
                    }
                },
                StandardScopes.OfflineAccess
            };
        }
    }
}