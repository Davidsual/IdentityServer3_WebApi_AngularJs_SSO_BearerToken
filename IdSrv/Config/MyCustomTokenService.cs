using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.Default;

namespace IdSrv.Config
{
    public class MyCustomTokenService : DefaultTokenService
    {
        private readonly ITokenHandleStore _tokenHandleStore;
        public MyCustomTokenService(IdentityServerOptions options, IClaimsProvider claimsProvider, ITokenHandleStore tokenHandles, ITokenSigningService signingService, IEventService events)
            :base(options, claimsProvider, tokenHandles, signingService, events)
        {
            _tokenHandleStore = tokenHandles;
        }

        public override async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var token = await base.CreateAccessTokenAsync(request);

            await _tokenHandleStore.StoreAsync(await _signingService.SignTokenAsync(token),token);

            return token; 
        }
    }
}
