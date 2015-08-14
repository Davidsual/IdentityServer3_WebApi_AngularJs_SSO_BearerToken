using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Apis
{
    public class TokenFilterAttribute : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cookie = request.Headers.GetCookies("tes").SingleOrDefault();

            if (cookie != null && cookie.Cookies.Any() && !string.IsNullOrEmpty(cookie.Cookies[0].Value))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie.Cookies[0].Value);

            return base.SendAsync(request, cancellationToken);
        }
        /*
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            
            var cookie = actionContext.Request.Headers.GetCookies("tes").SingleOrDefault();

            if (cookie != null && cookie.Cookies.Any() && !string.IsNullOrEmpty(cookie.Cookies[0].Value))
                actionContext.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie.Cookies[0].Value);
                
                
            base.OnAuthorization(actionContext);
        } */
    }

    public sealed class TrackingMiddleware : OwinMiddleware
    {
        public TrackingMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async System.Threading.Tasks.Task Invoke(IOwinContext context)
        {

            
            //var cookie = context.Request.Headers.GetCookies("tes").SingleOrDefault();

            //if (cookie != null && cookie.Cookies.Any() && !string.IsNullOrEmpty(cookie.Cookies[0].Value))
            //    context.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie.Cookies[0].Value);


            if(context.Request.Method != "OPTIONS" && context.Request.Cookies.Any())
            {
                var cookie = context.Request.Cookies["tes"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    context.Request.Headers.Remove("Authorization");
                    context.Request.Headers.Add("Authorization", new[] { "Bearer " + cookie }); //.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie.Cookies[0].Value);
                }
            }

            await Next.Invoke(context);
        }
    }
}