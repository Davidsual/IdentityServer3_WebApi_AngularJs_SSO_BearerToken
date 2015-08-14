using System;
using System.Net;
using System.Net.Http;
using Thinktecture.IdentityModel.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //var response = GetClientToken();
            //CallApi(response);
            /*
            var response = GetUserTokenLock();
            response = GetUserTokenLock();
            response = GetUserTokenLock();
            response = GetUserTokenLock();
            response = GetUserTokenLock();
            */
            var response = GetUserToken();
            CallApi(response);
            var response3 = RefreshToken(response.RefreshToken);
            if (response3 == null)
                return;
            CallApi(response);
            var response2 = RevokeToken(response.AccessToken);
            CallApi(response);
            CallApi(response);



            response = GetUserToken();
            response = GetUserToken();
            response = GetUserToken();
            CallApi(response);
        }

        static void CallApi(TokenResponse response)
        {
            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);
            Console.WriteLine(client.GetStringAsync("http://localhost:14869/test").Result);
            //Console.WriteLine(client.GetStringAsync("http://gblonsvcs01:14870/test").Result);
            //Console.WriteLine(client.GetStringAsync("http://HFL0125:14871/test").Result);
        }

        //static TokenResponse GetClientToken()
        //{
        //    var client = new OAuth2Client(
        //        new Uri("https://HFL0100:44333/connect/token"),
        //        "silicon",
        //        "F621F470-9731-4A25-80EF-67A6F7C5F4B8");
        //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //    return client.RequestClientCredentialsAsync("api1").Result;
        //}

        static TokenResponse RevokeToken(string token)
        {
            //var client = new OAuth2Client(
            //    new Uri("https://HFL0100:44333/connect/revocation"),
            //    "carbon",
            //    "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var a = new System.Collections.Generic.Dictionary<string, string>();
            //a.Add("token", token);
            //a.Add("token_type_hint", "access_token");
            //return client.  (a).Result;


            var client = new HttpClient();
            client.SetBasicAuthentication("carbon", "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            var postBody = new System.Collections.Generic.Dictionary<string, string>
            {
            { "token", token },
            //{ "token_type_hint", "refresh_token" }
            { "token_type_hint", "access_token" }
            };
            var result = client.PostAsync("https://HFL0100:44333/connect/revocation", new FormUrlEncodedContent(postBody)).Result;

            return null;
        }

        static TokenResponse RefreshToken(string refreshToken)
        {
            var client = new OAuth2Client(
                new Uri("https://HFL0100:44333/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            return client.RequestRefreshTokenAsync(refreshToken).Result;
        }
        static TokenResponse GetUserToken()
        {
            var client = new OAuth2Client(
                new Uri("https://HFL0100:44333/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            return client.RequestResourceOwnerPasswordAsync("Davidao", "Davide1981!", "api1 offline_access").Result;
        }

        static TokenResponse GetUserTokenLock()
        {
            var client = new OAuth2Client(
                new Uri("https://HFL0100:44333/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B",OAuth2Client.ClientAuthenticationStyle.PostValues);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            return client.RequestResourceOwnerPasswordAsync("Davidao", "Davide1981!1", "api1").Result;
        }
    }
}