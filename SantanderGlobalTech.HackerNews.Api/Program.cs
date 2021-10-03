using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the Web host
        /// </summary>
        /// <param name="args">Argument list</param>
        /// <returns>Builder of web host</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}
