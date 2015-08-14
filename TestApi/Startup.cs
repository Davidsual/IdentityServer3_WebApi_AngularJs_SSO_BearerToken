using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Thinktecture.IdentityServer.AccessTokenValidation;
using System.Net;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Thinktecture.IdentityServer.AccessTokenValidation;
using Microsoft.Owin.Security;
using System.Web.Cors;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;
using System.Linq;
[assembly: OwinStartup(typeof(TestApi.Startup))]

namespace TestApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            app.Use(async (context, next) =>
            {
                try
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
                }
                catch (Exception)
                {

                    throw;
                }
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


            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://HFL0100:44333",
                RequiredScopes = new[] { "api2" },
                ValidationMode = ValidationMode.ValidationEndpoint,
                AuthenticationType = "Bearer",
                AuthenticationMode = AuthenticationMode.Active

            });
            // configure web api
            var config = new HttpConfiguration();
            config.Filters.Add(new AuthorizeAttribute());
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
