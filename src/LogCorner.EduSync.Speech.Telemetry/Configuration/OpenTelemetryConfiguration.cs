using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;

namespace LogCorner.EduSync.Speech.Telemetry.Configuration
{
    public static class OpenTelemetryConfiguration
    {
        public static void AddOpenTelemetry(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            string openTelemetryServiceName = configuration["OpenTelemetry:ServiceName"];
            var environment = GetEnvironnement();
            loggingBuilder
                   .AddOpenTelemetry(options =>
                   {
                       options.IncludeFormattedMessage = true;
                       options.IncludeScopes = true;
                       options.ParseStateValues = true;

                       options
                           .SetResourceBuilder(
                               ResourceBuilder
                                   .CreateDefault()
                                   .AddService(openTelemetryServiceName)
                                   .AddAttributes(new Dictionary<string, object>
                                   {
                                    { "environment", environment }
                                   })
                                   .AddTelemetrySdk());
                   });
        }

        public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            string sourceName = configuration["OpenTelemetry:SourceName"];
            string openTelemetryServiceName = configuration["OpenTelemetry:ServiceName"];

            var environment = GetEnvironnement();
            var resourceBuilder = ResourceBuilder
                .CreateDefault()
                .AddService(openTelemetryServiceName)
                .AddAttributes(new Dictionary<string, object>
                {
                    {"environment", environment}
                })
                .AddTelemetrySdk();

            // Configure the OpenTelemetry SDK for tracing
            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                // Step 1. Declare the resource to be used by this tracer provider.
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder);

                // Step 2. Configure the SDK to listen to the following auto-instrumentation
                tracerProviderBuilder
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(s => s.SetDbStatementForText = true);

                // Step 3. Configure the SDK to listen to custom instrumentation.
                tracerProviderBuilder
                    .AddSource(sourceName);

                string environmentVariable = GetEnvironnement();
                if (environmentVariable != "Kubernetes.aks")
                {
                    tracerProviderBuilder.AddNewRelicExporter(configuration);
                    if (environmentVariable == "Development")
                    {
                        tracerProviderBuilder.AddConsoleExporter();
                    }

                    tracerProviderBuilder.AddJaegerExporter(configuration);

                    tracerProviderBuilder.AddZipkinExporter(configuration);
                }

                //tracerProviderBuilder.AddAzureMonitorExporter(configuration);
            });

            services.AddOpenTelemetryMetrics(meterProviderBuilder =>
            {
                // Step 1. Declare the resource to be used by this meter provider.
                meterProviderBuilder
                    .SetResourceBuilder(resourceBuilder);

                // Step 2. Configure the SDK to listen to the following auto-instrumentation
                meterProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                meterProviderBuilder.AddOtlpMetricExporter(configuration);
            });
            services.AddTelemetryServices(configuration);
        }

        private static string GetEnvironnement()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        }
    }
}