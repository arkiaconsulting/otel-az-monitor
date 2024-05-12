using System.Diagnostics;

namespace WebApi.Framework;

internal static class DiagnosticsConfig
{
    public const string SourceName = "WebApi";
    public static readonly ActivitySource Source = new(SourceName);
}
