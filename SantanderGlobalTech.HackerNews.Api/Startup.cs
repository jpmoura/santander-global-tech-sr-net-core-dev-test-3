using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SantanderGlobalTech.HackerNews.Infra.IoC;
using System;
using System.IO;
using System.Reflection;

namespace SantanderGlobalTech.HackerNews.Api
{
    /// <summary>
    /// Web Host startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup the application
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        /// <param name="env">Host environment</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Host environment
        /// </summary>
        public IWebHostEnvironment Env { get; }

        /// <summary>
        /// Configure all necessary services
        /// </summary>
        /// <param name="services">Collection of services used by the application</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            if (Env.IsEnvironment("unit-testing"))
            {
                return;
            }

            services.AddInfrastructure();

            services.AddApplication();

            services.AddMemoryCache();

            services.AddHealthChecks();

            services.AddResponseCaching();

            services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Santander Global Tech Hacker News API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "moura.joaoepdro@gmail.com",
                        Name = "João Pedro Santos de Moura",
                        Url = new Uri("https://github.com/jpmoura")
                    }
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configure the application
        /// </summary>
        /// <param name="app">Web Application</param>
        /// <param name="env">Running environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsEnvironment("unit-testing"))
            {
                return;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseHsts();

            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Santander Global Tech Hacker News API");
            });
        }
    }
}
