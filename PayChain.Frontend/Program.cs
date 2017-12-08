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
                var config = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build();

                var hostUrl = config["hosturl"];
                if (string.IsNullOrEmpty(hostUrl))
                {
                    hostUrl = "http://0.0.0.0:5000";
                }

                BuildWebHost(args, config, hostUrl).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot config, string hostUrl) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(hostUrl)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
