using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.Client;

namespace Apis
{
    
    
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public HttpResponseMessage Login([FromBody] LoginViewModel input)
        {
            var client = new OAuth2Client(
                    new Uri("https://HFL0100:44333/connect/token"),
                    "carbon",
                    "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // Ok(client.RequestResourceOwnerPasswordAsync(input.Username, input.Password, "api1 offline_access").Result);

            
            var response = client.RequestResourceOwnerPasswordAsync(input.Username, input.Password, "api1 api2 offline_access").Result;
            var response2 = new HttpResponseMessage(HttpStatusCode.OK);
            var cookie = new CookieHeaderValue("Halo-Secure", response.AccessToken);
            cookie.Expires = DateTimeOffset.Now.AddMinutes(1);
            cookie.Domain = null;//Request.RequestUri.Host;
            cookie.Path = "/";
            cookie.HttpOnly = true;            
            cookie.Secure = false;    //in production is TRUE        
            response2.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            
            return response2;            
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage Logout([FromBody] LogoutViewModel input)
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var Name = ClaimsPrincipal.Current.Identity.Name;
            var Name1 = User.Identity.Name;

            var userName = principal.Claims.Where(c => c.Type == "sub").Single().Value;
            var a = HttpContext.Current.GetOwinContext();
            var b = a.Authentication;

            var client = new HttpClient();
            client.SetBasicAuthentication("carbon", "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            var postBody = new System.Collections.Generic.Dictionary<string, string>
            {
            { "token", input.Access_Token },
            //{ "token_type_hint", "refresh_token" }
            { "token_type_hint", "access_token" }
            };

            if (!string.IsNullOrEmpty(input.Access_Token))
            {
                var result = client.PostAsync("https://HFL0100:44333/connect/revocation", new FormUrlEncodedContent(postBody)).Result;
            }

            postBody = new System.Collections.Generic.Dictionary<string, string>
            {
            { "token", input.Refresh_Token },
            { "token_type_hint", "refresh_token" }
            //{ "token_type_hint", "access_token" }
            };

            if (!string.IsNullOrEmpty(input.Refresh_Token))
            {
                var result2 = client.PostAsync("https://HFL0100:44333/connect/revocation", new FormUrlEncodedContent(postBody)).Result;
            }
            var response2 = new HttpResponseMessage(HttpStatusCode.OK);
            var test = Request.Headers.GetCookies("Halo-Secure").SingleOrDefault();
            if (test != null)
            {
                test.Expires = DateTimeOffset.Now.AddDays(-1);
                response2.Headers.AddCookies(new CookieHeaderValue[] { test });
            }
            return response2;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("refreshtokens")]
        public IHttpActionResult RefreshToken([FromBody] RefreshTokenViewModel input)
        {
            var client = new OAuth2Client(
                new Uri("https://HFL0100:44333/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            return Ok < TokenResponse >(client.RequestRefreshTokenAsync(input.Refresh_Token).Result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("createuser")]
        public IHttpActionResult CreateUserInMembership([FromBody] CreateUserViewModel input)
        {
            //Create User / Login
            return Ok();
        }
    }

    public class CreateUserViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

    }


    public class LogoutViewModel
    {
        public string Grant_Type { get; set; }

        public string Refresh_Token { get; set; }

        public string Access_Token { get; set; }

        public string Client_Id { get; set; }
    }

    public class RefreshTokenViewModel
    {
        public string Grant_Type { get; set; }

        public string Refresh_Token { get; set; }

        public string Client_Id { get; set; }
    }
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_Type { get; set; }
    }
}