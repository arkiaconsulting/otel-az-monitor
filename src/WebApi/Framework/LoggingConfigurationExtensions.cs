namespace WebApi.Framework;

internal static class LoggingConfigurationExtensions
{
    public static void ConfigureWebApiLogging(this ILoggingBuilder loggingBuilder) =>
        loggingBuilder.ClearProviders()
            .AddSimpleConsole(cfg =>
            {
                cfg.IncludeScopes = true;
                cfg.SingleLine = false;
            });
}
