using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PayChain.Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
    }
}
