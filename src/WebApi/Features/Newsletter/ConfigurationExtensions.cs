namespace WebApi.Features.Newsletter;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Extension method to register the types implied in the newsletter subscription feature.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddNewsletterFeature(this IServiceCollection services)
    {
        services.AddSingleton<HashSet<NewsletterSubscriptionEmail>>();
        services.AddTransient<IStoreNewsletterSubscription, InMemoryNewsletterSubscriptionStore>();

        return services;
    }
}
