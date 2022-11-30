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
        app.MapGet(
            "/greet/{name}",
            (
                [FromServices] Greeter greeter, string name
            ) => greeter.Hi(name)
        );

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

        // Parameter binding
        app.MapGet(
            "/point",
            (Point? point) => point is {} ? $"point coordinates: {point.X}, {point.Y}" : "point not provided"
        );

        app.MapGet(
            "/page",
            (Paging page) => $"page offset: {page.Offset}, size: {page.Size}"
        );

        return app;
    }
}
