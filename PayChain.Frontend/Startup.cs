using System;
using System.IO;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PayChain.Backend.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace PayChain.Frontend
{
    public class Startup
    {
        private readonly string _environmentName;

        public Startup(IHostingEnvironment env)
        {
            if (env.EnvironmentName == null)
            {
                env.EnvironmentName = "Development";
            }

            _environmentName = env.EnvironmentName;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    // set the json return values to be the same casing as in the c# object
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                    // ignore self referencing loop for related objects
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info { Title = "PayChain", Version = "v1", Description = "" });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "PayChain.Frontend.xml");
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, filePath));
            });

            services.AddCors();

            services.AddOptions();

            services.AddSingleton<IRequestClient, RequestClient>();
            services.AddSingleton<IChain, Chain>();

            // Rate limiting setup
            if (_environmentName != "Development")
            {
                services.AddMemoryCache();
                services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
                services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole()
                .AddDebug();

            // Disable RateLimiting when running locally
            if (_environmentName != "Development")
            {
                app.UseIpRateLimiting();
            }
            
            app.UseSwagger(swagger => swagger.RouteTemplate = "swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(swagger =>
            {
                swagger.RoutePrefix = "swagger";
                swagger.SwaggerEndpoint("v1/swagger.json", "PayChain");
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc();
        }
    }
}
