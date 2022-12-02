namespace Epam.Lab.MinimalApi;

internal static class FilterMiddlewareExtensions
{
    internal static WebApplication MapFilterMiddleware(this WebApplication app)
    {
        string ColorName(string color) => $"Color specified: {color}!";

        // https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http.Abstractions/src/EndpointFilterInvocationContext.cs
        app.MapGet("/color-selector/{color}", ColorName)
            .AddEndpointFilter(async (invocationContext, next) =>
            {
                var color = invocationContext.GetArgument<string>(0);

                if (color == "red")
                {
                    return Results.Problem("Red not allowed!");
                }
                return await next(invocationContext);
            })
            .AddEndpointFilter<LogFilter>();

        app.Use((context, next) =>
        {
            var result = next(context);
            var method = context.Request.Method;
            var path = context.Request.Path;
            app.Logger.LogInformation("Request: {Method} {Path} finished", method, path);
            return result;
        });

        return app;
    }
}

public sealed class LogFilter : IEndpointFilter
{
    private readonly ILogger<LogFilter> _logger;

    public LogFilter(ILogger<LogFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var path = context.HttpContext.Request.Path;
        _logger.LogInformation("Requested for path: {Path}", path);
        return await next(context);
    }
}