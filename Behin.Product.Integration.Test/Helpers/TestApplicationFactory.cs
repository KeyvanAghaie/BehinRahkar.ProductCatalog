using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Behin.IntegrationTests
{
    public class TestApplicationFactory<TStartup, TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                            .ConfigureWebHost(builder =>
                            {
                                builder.UseStartup<TTestStartup>();
                            })
                            .ConfigureAppConfiguration((context, conf) =>
                            {
                                var projectDir = Directory.GetCurrentDirectory();
                                var configPath = Path.Combine(projectDir, "appsettings.json");

                                conf.AddJsonFile(configPath);
                            });

            return host;
        }
    }
}