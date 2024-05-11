using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Framework;

namespace WebApi.Features.Newsletter;

internal static class EndpointExtensions
{
    /// <summary>
    /// Registers the endpoint to subscribe to the newsletter.
    /// </summary>
    /// <param name="routes"></param>
    internal static void MapSubscribeNewsletter(this IEndpointRouteBuilder routes) =>
        routes.MapPut("/newsletter/subscribe", async (
            [FromBody] SubscribeNewsletterRequest request,
            [FromServices] ISender sender,
            [FromServices] ILoggerFactory loggerFactory,
            [FromServices] WebApiMetrics metrics) =>
        {
            var logger = loggerFactory.CreateLogger("SubscribeNewsletter.Http");
            logger.LogInformation("Handling HTTP request {Email}", request.Email);

            await sender.Send(new SubscribeNewsletterCommand(request.Email));

            metrics.BumpNewsletterSubscriptionCount();

            logger.LogInformation("Successfully handled HTTP request");
            return TypedResults.NoContent();
        });
}

/// <summary>
/// The type representing the HTTP request to subscribe to the newsletter.
/// </summary>
/// <param name="Email"></param>
internal sealed record SubscribeNewsletterRequest(string Email);

/// <summary>
/// The type representing the command to subscribe to the newsletter.
/// </summary>
/// <param name="Email"></param>
internal sealed record SubscribeNewsletterCommand(string Email) : IRequest
{
    /// <summary>
    /// The class responsible for handling the command to subscribe to the newsletter.
    /// </summary>
    public class Handler : IRequestHandler<SubscribeNewsletterCommand>
    {
        private readonly IStoreNewsletterSubscription _store;
        private readonly ILogger _logger;

        public Handler(IStoreNewsletterSubscription store, ILoggerFactory loggerFactory)
        {
            _store = store;
            _logger = loggerFactory.CreateLogger("SubscribeNewsletter.Handler");
        }

        public async Task Handle(SubscribeNewsletterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling command {Email}", request.Email);

            await _store.Add(new(request.Email));

            _logger.LogInformation("Successfully handled command");
        }
    }
}

/// <summary>
/// Abstraction over the newsletter subscription store.
/// </summary>
internal interface IStoreNewsletterSubscription
{
    Task Add(NewsletterSubscriptionEmail email);
}

/// <summary>
/// An in-memory implementation of the newsletter subscription store.
/// </summary>
internal sealed class InMemoryNewsletterSubscriptionStore : IStoreNewsletterSubscription
{
    private readonly HashSet<NewsletterSubscriptionEmail> _newsletterSubscriptionEmails;
    private readonly ILogger _logger;

    public InMemoryNewsletterSubscriptionStore(
        HashSet<NewsletterSubscriptionEmail> newsletterSubscriptionEmails,
        ILoggerFactory loggerFactory)
    {
        _newsletterSubscriptionEmails = newsletterSubscriptionEmails;
        _logger = loggerFactory.CreateLogger("SubscribeNewsletter.Persistence");
    }

    public Task Add(NewsletterSubscriptionEmail email)
    {
        _logger.LogInformation("Adding newsletter subscription email {Email}", email.Value);

        _newsletterSubscriptionEmails.Add(email);

        _logger.LogInformation("Successfully added newsletter subscription email");
        return Task.CompletedTask;
    }
}