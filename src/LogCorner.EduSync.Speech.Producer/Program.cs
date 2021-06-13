using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace LogCorner.EduSync.Speech.Producer
{
    public class Program
    {
        private static IConfigurationRoot _configuration;

        public static void Main(string[] args)
        {
            ConfigureEnvironment();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureEnvironment()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", false)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var hubUrl = _configuration["HubUrl"];
                    var kafkaUrl = _configuration["kafkaUrl"];
                    services.AddSingleton<IProducerService, ProducerService>();
                    services.AddHostedService<ProducerHostedService>();
                    services.AddSignalRServices(hubUrl, _configuration);
                    services.AddSharedKernel();

                    services.AddServiceBus(kafkaUrl);
                });
    }
}