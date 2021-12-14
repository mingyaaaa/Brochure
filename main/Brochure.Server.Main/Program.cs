using System;
using System.IO;
using Brochure.Core.PluginsDI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Brochure.Server.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new PluginServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}