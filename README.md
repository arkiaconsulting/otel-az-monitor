# OpenTelemetry in .NET

For each commit:

1. Starting point​
2. Setting up OTEL with console exporter​
3. Export to Aspire dashboard using OTLP​
4. Create some traces​
5. Export to Azure Monitor

# Useful links

- [Practical Open Telemetry using Net8 – Martin Thwaites](https://youtu.be/WzZI_IT6gYo)
- [Improving your application telemetry using .NET 8 and Open Telemetry](https://youtu.be/BnjHArsYGLM)
- [Get started with Open Telemetry in .Net - Nick Chapsas](https://youtu.be/nFU-hcHyl2s)
- [Using instrumentation libraries](https://opentelemetry.io/docs/languages/net/libraries/)
- [OpenTelemetry and Application Insights CustomEvents (github issue)](https://github.com/Azure/azure-sdk-for-net/issues/42157)
- [Aspire Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/explore)

## Run Aspire Dashboard locally

```bash
docker run --rm -it -p 18888:18888 -p 4317:18889 --name aspire-dashboard mcr.microsoft.com/dotnet/nightly/aspire-dashboard:8.0.0-preview.6
```

## Add Application Insights connection string
In your launch settings:

```json
"environmentVariables": {
    "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=your_key;IngestionEndpoint=......."
}
```
