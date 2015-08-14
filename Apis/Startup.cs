using System.Net;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Apis.Config;
using Thinktecture.IdentityServer.AccessTokenValidation;
using Microsoft.Owin.Security;
using System.Web.Cors;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(Apis.Startup))]

namespace Apis
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


            app.Use(async (context, next) =>
            {
                if (context.Request.Method != "OPTIONS" && context.Request.Cookies.Any())
                {                    
                    var cookie = context.Request.Cookies["Halo-Secure"];
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        context.Request.Headers.Remove("Authorization");
                        context.Request.Headers.Add("Authorization", new[] { "Bearer " + cookie }); //.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie.Cookies[0].Value);
                    }
                }

                await next.Invoke();
            });


            var corsPolicy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = false,
                SupportsCredentials = true
            };            
            corsPolicy.Origins.Add("http://localhost:32150");
            //corsPolicy.Origins.Add("https://localhost:32150");
            //corsPolicy.Origins.Add("http://localhost:32150/");
            //corsPolicy.Origins.Add("https://localhost:32150/");
            corsPolicy.ExposedHeaders.Add("X-Custom-Header");
            app.UseCors(new Microsoft.Owin.Cors.CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            });

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "https://HFL0100:44333",
                    RequiredScopes = new[] { "api1" },
                    ValidationMode = ValidationMode.Local,
                    AuthenticationType = "Bearer",
                    AuthenticationMode = AuthenticationMode.Active,                                        
            });
            // configure web api
            var config = new HttpConfiguration();
            config.Filters.Add(new AuthorizeAttribute());
            config.MapHttpAttributeRoutes();       
            app.UseWebApi(config);
        }
    }
}