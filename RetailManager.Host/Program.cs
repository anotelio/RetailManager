using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace RetailManager.WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            //.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    config.AddJsonFile("kestrelConfig.json", optional: false, reloadOnChange: true);
            //})
            .ConfigureWebHostDefaults(webBuilder =>
            {
                //webBuilder.UseUrls("http://localhost:4240");
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    //serverOptions.Listen(IPAddress.Parse("127.0.0.1"), 4240);
                    // Set properties and call methods on options
                })
                .UseStartup<Startup>();
            });
}