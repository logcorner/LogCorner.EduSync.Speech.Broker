using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Collections.Generic;
using System;

namespace LogCorner.EduSync.Speech.Telemetry.Configuration
{
    public static class OpenTelemetryConfiguration
    {
        public static void UseSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            string? serviceName = configuration["OpenTelemetry:Otlp:ServiceName"];
            if (serviceName is null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = $"{configuration["OpenTelemetry:Otlp:Endpoint"]}/v1/logs";
                    options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;

                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = serviceName
                    };
                }));
        }

        public static void UseSerilog(this IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string? serviceName = configuration["OpenTelemetry:Otlp:ServiceName"];
            if (serviceName is null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = $"{configuration["OpenTelemetry:Otlp:Endpoint"]}/v1/logs";
                    options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = serviceName
                    };
                })
                .CreateLogger();
        }

        public static void AddOpenTelemetryServices(this IServiceCollection Services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            string? serviceName = configuration["OpenTelemetry:Otlp:ServiceName"];
            if (serviceName is null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }
            string? otlpEndpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
            if (otlpEndpoint is null)
            {
                throw new ArgumentNullException(nameof(otlpEndpoint));
            }
            Action<ResourceBuilder> appResourceBuilder =
                resource => resource
                    .AddTelemetrySdk()
                    .AddService(serviceName);

            Services.AddOpenTelemetry()
                .ConfigureResource(appResourceBuilder)
                .WithTracing(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("APITracing")
                .AddConsoleExporter()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otlpEndpoint);
                    })
                    .AddZipkinExporter(b =>
                    {
                        var zipkinHostName = configuration["OpenTelemetry:Zipkin:Hostname"];
                        var zipkinPort = configuration["OpenTelemetry:Zipkin:PortNumber"];

                        var endpoint = new Uri($"http://{zipkinHostName}:{zipkinPort}/api/v2/spans");
                        b.Endpoint = endpoint;
                    })
                 )
                .WithMetrics(builder => builder
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("System.Net.Http")
                    .AddPrometheusExporter()

                    .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                    {
                        metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                    })
                    .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint)));


            Services.AddSingleton<ITraceService, TraceService>();
        }

        //public static void AddOpenTelemetryLogging(this ILoggingBuilder Logging)
        //{
        //    Logging.AddOpenTelemetry(options =>
        //    {
        //        options.AddOtlpExporter();
        //        options.AddConsoleExporter();
        //    });
        //}
    }
}