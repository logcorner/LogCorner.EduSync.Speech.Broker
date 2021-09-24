using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.Synchro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace LogCorner.EduSync.Speech.Synchro
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
            string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

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
                    var kafkaUrl = _configuration["kafkaUrl"];
                    services.AddSingleton<ISynchroService, SynchroService>();
                    services.AddHostedService<SynchroHostedService>();

                    var connectionString = _configuration["ConnectionStrings:SpeechDB"];

                    services.AddDbContext<LogcornerEdusyncSpeechDataContext>(o => o.UseSqlServer(connectionString), ServiceLifetime.Singleton);

                    services.AddSharedKernel();

                    services.AddSingleton<IKafkaProducer>(x =>
                        {
                            var producerConfig = new ProducerConfig { BootstrapServers = kafkaUrl };

                            var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

                            return new KafkaProducer(producer,
                                x.GetRequiredService<IJsonSerializer>()
                            );
                        }
                    );
                });
    }
}