using LogCorner.EduSync.Notification.Common;
using LogCorner.EduSync.Speech.Command.SharedKernel;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.Telemetry.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var kafkaUrl = _configuration["kafkaUrl"];
                    var hubUrl = _configuration["hubUrl"];
                    var elasticSearchUrl = _configuration["elasticSearchUrl"];
                    services.AddSingleton<IClusterManager, KafkaClusterManager>(
                        ctx => new KafkaClusterManager(_configuration)
                        );
                    services.AddServiceBus(kafkaUrl);
                    services.AddSingleton<IConsumerService, ConsumerService>();
                    services.AddHostedService<ConsumerHostedService>();
                    services.AddSignalRServices($"{hubUrl}?clientName=LogCorner.EduSync.Speech.Consumer", _configuration);
                    services.AddSharedKernel();
                    services.AddOpenTelemetry(_configuration);

                    //
                    //services.AddScoped<ITraceService, TraceService>();
                    //services.AddOpenTelemetryTracing((builder) =>
                    //{
                    //    builder.AddAspNetCoreInstrumentation()
                    //        .AddHttpClientInstrumentation()
                    //        .AddSource("LogCorner.EduSync.Speech.Consumer.Program")
                    //        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("consumer-service"))
                    //        .AddJaegerExporter(opts =>
                    //        {
                    //            opts.AgentHost = "localhost";
                    //            opts.AgentPort = 6831;
                    //            opts.ExportProcessorType = ExportProcessorType.Simple;
                    //        });
                    //});

                    //
                    services.AddElasticSearch<SpeechProjection>(elasticSearchUrl, "speech");
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddSerilog(_configuration);
                    loggingBuilder.AddOpenTelemetry(_configuration);
                });
    }
}