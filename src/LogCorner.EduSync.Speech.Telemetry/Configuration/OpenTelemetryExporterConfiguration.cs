using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using System;

namespace LogCorner.EduSync.Speech.Telemetry.Configuration
{
    public static class OpenTelemetryExporterConfiguration
    {
        public static void AddOtlpMetricExporter(this MeterProviderBuilder meterProviderBuilder,
            IConfiguration configuration)
        {
            var newRelicHostName = configuration["OpenTelemetry:NewRelic:Hostname"];
            var newRelicPortNumber = Helper.ParseInt(configuration["OpenTelemetry:NewRelic:PortNumber"]);
            var newRelicApiKey = configuration["OpenTelemetry:NewRelic:LicenceKey"];
            if (Uri.CheckSchemeName($"{newRelicHostName}:{newRelicPortNumber}"))
            {
                meterProviderBuilder
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri($"{newRelicHostName}:{newRelicPortNumber}");
                        options.Headers = $"api-key={newRelicApiKey}";
                    });
            }
        }

        public static void AddNewRelicExporter(this TracerProviderBuilder tracerProviderBuilder, IConfiguration configuration)
        {
            var newRelicHostName = configuration["OpenTelemetry:NewRelic:Hostname"];
            var newRelicPortNumber = Helper.ParseInt(configuration["OpenTelemetry:NewRelic:PortNumber"]);
            var newRelicApiKey = configuration["OpenTelemetry:NewRelic:LicenceKey"];
            tracerProviderBuilder
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri($"{newRelicHostName}:{newRelicPortNumber}");
                    options.Headers = $"api-key={newRelicApiKey}";
                });
        }

        public static void AddAzureMonitorExporter(this TracerProviderBuilder tracerProviderBuilder, IConfiguration configuration)
        {
            var appInsightConnectionString = configuration["OpenTelemetry:AppInsight:ConnectionString"];
            if (string.IsNullOrWhiteSpace(appInsightConnectionString))
            {
                throw new TelemetryException("appInsightConnectionString should be provided");
            }

            tracerProviderBuilder
                .AddAzureMonitorTraceExporter(c =>
                {
                    c.ConnectionString = appInsightConnectionString;
                });
        }

        public static void AddZipkinExporter(this TracerProviderBuilder tracerProviderBuilder, IConfiguration configuration)
        {
            var zipkinHostName = configuration["OpenTelemetry:Zipkin:Hostname"];
            var zipkinPort = configuration["OpenTelemetry:Zipkin:PortNumber"];

            var endpoint = new Uri($"http://{zipkinHostName}:{zipkinPort}/api/v2/spans");
            Log.Debug($"OpenTelemetryExporterConfiguration::AddZipkinExporter:Endpoint {endpoint}");
            tracerProviderBuilder.AddZipkinExporter(b =>
            {
                b.Endpoint = endpoint;
            });
        }

        public static void AddJaegerExporter(this TracerProviderBuilder tracerProviderBuilder, IConfiguration configuration)
        {
            var jaergerHostName = configuration["OpenTelemetry:Jaeger:Hostname"];
            var jaergerPort = configuration["OpenTelemetry:Jaeger:PortNumber"];
            tracerProviderBuilder.AddJaegerExporter(o =>
            {
                o.AgentHost = jaergerHostName;
                o.AgentPort = int.Parse(jaergerPort);
                o.ExportProcessorType = ExportProcessorType.Simple;
            });
        }
    }
}