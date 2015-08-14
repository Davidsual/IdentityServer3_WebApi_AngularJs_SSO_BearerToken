using Microsoft.Owin.Hosting;
using System;
using Thinktecture.IdentityServer.Core.Logging;

namespace IdSrv
{
    class Program
    {
        static void Main(string[] args)
        {           
            using (WebApp.Start<Startup>("https://HFL0100:44333"))
            {
                Console.WriteLine("server running...");
                Console.ReadLine();
            }
        }
    }
}