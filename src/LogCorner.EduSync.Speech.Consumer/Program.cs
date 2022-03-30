using LogCorner.EduSync.Notification.Common;
using LogCorner.EduSync.Speech.Command.SharedKernel;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace LogCorner.EduSync.Speech.Consumer
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
                .ConfigureServices((_, services) =>
                {
                    var kafkaUrl = _configuration["kafkaUrl"];
                    var hubUrl = _configuration["hubUrl"];
                    var elasticSearchUrl = _configuration["elasticSearchUrl"];
                    services.AddSingleton<IClusterManager, KafkaClusterManager>(
                        _ => new KafkaClusterManager(_configuration));
                    services.AddServiceBus(kafkaUrl);
                    services.AddSingleton<IConsumerService, ConsumerService>();
                    services.AddHostedService<ConsumerHostedService>();
                    services.AddSignalRServices($"{hubUrl}?clientName=LogCorner.EduSync.Speech.Consumer", _configuration);
                    services.AddSharedKernel();

                    services.AddElasticSearch<SpeechProjection>(elasticSearchUrl, "speech");
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();

                    bool.TryParse(settings["isAuthenticationEnabled"], out var isAuthenticationEnabled);
                    if (!context.HostingEnvironment.IsDevelopment() && isAuthenticationEnabled)
                    {
                        // Configure Azure Key Vault Connection
                        var uri = settings["AzureKeyVault:Uri"];
                        var clientId = settings["AzureKeyVault:ClientId"];
                        var clientSecret = settings["AzureKeyVault:ClientSecret"];

                        // Check, if Client ID and Client Secret credentials for a Service Principal
                        // have been provided. If so, use them to connect, otherwise let the connection 
                        // be done automatically in the background
                        if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
                        {
                            Console.WriteLine($"******** using service principal to read secrets from keyvault : {uri} ********");
                            config.AddAzureKeyVault(uri, clientId, clientSecret);
                        }
                        else
                        {
                            Console.WriteLine($"******** using managed identity to read secrets from keyvault : {uri} ********");
                            config.AddAzureKeyVault(uri);
                        }
                    }
                });
    }
}