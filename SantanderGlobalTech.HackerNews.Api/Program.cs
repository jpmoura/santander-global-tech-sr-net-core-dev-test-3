using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace SantanderGlobalTech.HackerNews.Api
{
    /// <summary>
    /// Application main file
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Argument list</param>
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Create the Web host
        /// </summary>
        /// <param name="args">Argument list</param>
        /// <returns>Builder of web host</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).UseSerilog()
                                                  .ConfigureWebHostDefaults(webBuilder =>
                                                  {
                                                      webBuilder.UseStartup<Startup>();
                                                      webBuilder.UseKestrel((options) =>
                                                      {
                                                          options.AddServerHeader = false;
                                                      });
                                                      webBuilder.UseSentry(o =>
                                                      {
                                                          o.Dsn = "https://9687ea30a87442e78b27efceaeb6b9c5@o1024826.ingest.sentry.io/5990681";
                                                          o.Debug = false;
                                                          o.TracesSampleRate = 1.0;
                                                      });
                                                  });
        }
    }
}
