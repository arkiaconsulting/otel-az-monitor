using System.Diagnostics.Metrics;

namespace WebApi.Framework;

/// <summary>
/// Convenient class to encapsulate the metrics of the WebApi.
/// </summary>
internal sealed class WebApiMetrics
{
    public const string MeterName = "WebApi";

    private readonly Counter<long> _newsletterSubscriptions;

    public WebApiMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName, "1.0.0");
        _newsletterSubscriptions = meter.CreateCounter<long>(
            $"webapi.newsletter_subscriptions.count"
        );
    }

    public void BumpNewsletterSubscriptionCount() =>
        _newsletterSubscriptions.Add(1);
}
