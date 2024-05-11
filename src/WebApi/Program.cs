using WebApi.Features.Newsletter;
using WebApi.Framework;

var builder = WebApplication.CreateBuilder(args);

// Registers the MediatR services and the handlers from the current assembly
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblyContaining<Program>()
);

// Registers the types implied in the newsletter subscription feature
builder.Services.AddNewsletterFeature();

// Registers the Metrics services
builder.Services.AddMetrics()
    .AddSingleton<WebApiMetrics>();

// Configures the logging for the WebApi
builder.Logging.ConfigureWebApiLogging(builder.Environment);

var app = builder.Build();

app.UseHttpsRedirection();

// Registers the endpoint implied in the newsletter subscription feature
app.MapSubscribeNewsletter();

app.Run();
