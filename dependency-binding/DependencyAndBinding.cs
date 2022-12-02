namespace Epam.Lab.MinimalApi;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

internal static class DependencyAndBindingExtensions
{
    internal static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<Greeter, CoolGreeter>();
        return services;
    }

    internal static WebApplication MapDependenciesAndBindings(this WebApplication app)
    {
        // di from services
        app.MapGet(
            "/greet/{name}",
            (
                [FromServices] Greeter greeter, string name
            ) => greeter.Hi(name)
        );

        // di from services and params
        app.MapGet(
            "/greet/param/{name}",
            (
                [AsParameters] GreetRequest greetRequest
            ) =>
            {
                string name = greetRequest.Name ?? string.Empty;
                return greetRequest?.Greeter?.Hi(name);
            }
        );

        // json from body
        app.MapPost(
            "/body-map",
            ([FromBody] Point point) => $"point coordinates: {point.X}, {point.Y}"
        );

        // built in classes and reading from stream
        app.MapGet(
            "built-in",
            async (
                HttpRequest request, HttpResponse response,
                Stream body, HttpContext context,
                ClaimsPrincipal user, CancellationToken ct
            ) =>
            {
                var header = request.Headers["X-TEST-INPUT"];
                response.Headers.Add("X-TEST", new[] { "one", "two" });
                
                using var streamReader = new StreamReader(body);
                var text = await streamReader.ReadToEndAsync();

                return $"input body: {text}, header: {header}";
            }
        );

        // parameter binding with static TryParse / string
        app.MapGet(
            "/point",
            (Point? point) => point is {} ? $"point coordinates: {point.X}, {point.Y}" : "point not provided"
        );

        // parameter binding with static BindAsync / httpContext
        app.MapGet(
            "/page",
            (Paging page) => $"page offset: {page.Offset}, size: {page.Size}"
        );

        return app;
    }
}
