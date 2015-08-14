using Owin;
using System.Collections.Generic;
using IdSrv.Config;
using Thinktecture.IdentityServer.AccessTokenValidation;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Services;
using Thinktecture.IdentityServer.Core.Services.InMemory;
using IdSrv.Custom;
using Thinktecture.IdentityServer.Core.Logging.LogProviders;
using Thinktecture.IdentityServer.Core.Logging;

namespace IdSrv
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            //var factory2 = new IdentityServerServiceFactory();

            //LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());
            //var factory = InMemoryFactory.Create(
            //    scopes: Scopes.Get(),
            //    clients: Clients.Get(),
            //    users: Users2.Get()
            //    );
            var factory = new IdentityServerServiceFactory();            
            var scopeStore = new InMemoryScopeStore(Scopes.Get());
            factory.ScopeStore = new Registration<IScopeStore>(scopeStore);
            var clientStore = new InMemoryClientStore(Clients.Get());
            factory.ClientStore = new Registration<IClientStore>(clientStore);
            factory.TokenService = new Registration<ITokenService>(typeof(MyCustomTokenService));
            factory.RefreshTokenStore = new Registration<IRefreshTokenStore>(typeof(MyCustomRefreshTokenStore));
            factory.CustomTokenValidator = new Registration<ICustomTokenValidator>(new MyCustomTokenValidator());
            factory.TokenHandleStore = new Registration<ITokenHandleStore>(new MyCustomTokenHandleStore());
            factory.ConfigureUserService("AspId");
            LogProvider.SetCurrentLogProvider(new NLogLogProvider());
            //LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());
            //factory.TokenHandleStore = new Registration<ITokenHandleStore>();
            //factory.RefreshTokenStore = new Registration<IRefreshTokenStore>();
            //factory.CustomTokenValidator = new Registration<ICustomTokenValidator>(new MyCustomTokenValidator());
            //factory.Register(new Registration<IUserService, MyCustomUserService>());
            //factory.Register(new Registration<IMyCustomLogger, MyCustomLogger>());
            //factory.UserService = new Registration<IUserService>(typeof(IUserService));
            var options = new IdentityServerOptions
            {
                Factory = factory,
                //IssuerUri = "https://idsrv3.com",
                SiteName = "Thinktecture IdentityServer3 Halo",
                SigningCertificate = Certificate.Get(),
                RequireSsl = false,                
                CspOptions = new CspOptions
                {
                    Enabled =true,
                },
                Endpoints = new EndpointOptions
                {
                    EnableAccessTokenValidationEndpoint = true,
                    EnableTokenEndpoint = true,
                    EnableTokenRevocationEndpoint = true,
                    EnableIdentityTokenValidationEndpoint = true,   
                    
                    //remove in production
                    EnableDiscoveryEndpoint = true,
                                 
                    EnableAuthorizeEndpoint= false,
                    EnableClientPermissionsEndpoint= false,
                    EnableCspReportEndpoint= false,


                    EnableEndSessionEndpoint=false,
                    EnableCheckSessionEndpoint = false,
                    EnableUserInfoEndpoint = false                   
                },
                AuthenticationOptions = new AuthenticationOptions
                {
                    EnableLocalLogin = true,
                    EnableLoginHint = false,                    
                },
                LoggingOptions = new LoggingOptions
                {
                    EnableHttpLogging=true,
                    EnableWebApiDiagnostics=true,
                    IncludeSensitiveDataInLogs=true,
                    WebApiDiagnosticsIsVerbose=true
                },
                EnableWelcomePage = false,                                
                IssuerUri = "https://HFL0100:44333"
                
            };
            options.CorsPolicy.AllowedOrigins.Add("http://localhost:14869/");


            app.UseHsts();
            app.UseIdentityServer(options);
        }
    }
}
