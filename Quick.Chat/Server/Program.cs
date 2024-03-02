using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Chat.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string hostUrl = "";//$"http://0.0.0.0:{config.GetSection("AppSettings:ServicePort").Get<int>()}";

            //Console.WriteLine($"Service listening:{hostUrl}");
            CreateHostBuilder(hostUrl, args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string hostUrl, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls(hostUrl);
                });
    }
}
