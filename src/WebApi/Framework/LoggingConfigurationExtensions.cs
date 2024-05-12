using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApi.Framework;

internal static class LoggingConfigurationExtensions
{
    public static void ConfigureWebApiLogging(this ILoggingBuilder loggingBuilder, IWebHostEnvironment environment)
    {
        const string serviceName = "WebApi";

        loggingBuilder.ClearProviders()
            .AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;

                logging.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName)
                    .AddAttributes([
                        new KeyValuePair<string, object>("environment", environment.EnvironmentName)
                    ])
                );

                logging.AddOtlpExporter();
            });

        loggingBuilder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing.SetSampler<AlwaysOnSampler>();
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddOtlpExporter();
                tracing.AddSource(DiagnosticsConfig.SourceName);
            })
            .WithMetrics(metrics =>
            {
                metrics.AddMeter(WebApiMetrics.MeterName);
                metrics.AddOtlpExporter();
            });
    }
}
