using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Validation;

namespace IdSrv.Config
{
    public class MyCustomTokenValidator : ICustomTokenValidator
    {

        public Task<Thinktecture.IdentityServer.Core.Validation.TokenValidationResult> ValidateAccessTokenAsync(Thinktecture.IdentityServer.Core.Validation.TokenValidationResult result)
        {
            //result.IsError = true;
            //result.Error = Constants.ProtectedResourceErrors.InvalidToken;
            //result.Claims = null;
            return Task.FromResult(result);
        }

        public Task<Thinktecture.IdentityServer.Core.Validation.TokenValidationResult> ValidateIdentityTokenAsync(Thinktecture.IdentityServer.Core.Validation.TokenValidationResult result)
        {
            throw new NotImplementedException();
        }
    }
}
