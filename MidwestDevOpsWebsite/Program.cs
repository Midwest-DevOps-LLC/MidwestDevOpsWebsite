using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MidwestDevOpsWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel();
                    webBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());
                    webBuilder.UseUrls("http://localhost:6969");
                    //webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
