using LogCorner.EduSync.Notification.Common;
using LogCorner.EduSync.Speech.Consumer;
using LogCorner.EduSync.Speech.Projection.WorkerService;
using LogCorner.EduSync.Speech.Telemetry.Configuration;

IHost host = Host.CreateDefaultBuilder(args)

    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var kafkaEndpoint = configuration["kafkaUrl"];
        var elasticSearchEndpoint = configuration["elasticSearchUrl"];
        var notificationHubEndpoint = configuration["HubUrl"];
        services.AddConsumer(configuration, kafkaEndpoint, elasticSearchEndpoint, "speech");
        services.AddSignalRServices($"{notificationHubEndpoint}?clientName=LogCorner.EduSync.Speech.Consumer");
        services.AddOpenTelemetry(configuration);
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
        loggingBuilder.AddSerilog(context.Configuration);
        loggingBuilder.AddOpenTelemetry(context.Configuration);
    })
    .Build();

await host.RunAsync();