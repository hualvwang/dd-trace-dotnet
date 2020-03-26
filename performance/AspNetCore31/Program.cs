using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AspNetCore31
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var process = Process.GetCurrentProcess())
            {
                Console.WriteLine($"ProcessID = {process.Id}");
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                                           {
                                               config.AddEnvironmentVariables(prefix: "DD_");
                                           })
                .ConfigureWebHostDefaults(webBuilder =>
                                          {
                                              webBuilder.UseStartup<Startup>();
                                          });
    }
}
