namespace Epam.Lab.MinimalApi;

internal static class BasiMappingExtensions
{
    internal static WebApplication MapBasic(this WebApplication app)
    {
        app.MapGet("/", () => "GET");
        app.MapPost("/path", () => "Path POST");

        app.MapMethods(
            "/options-or-head",
             new[] { "OPTIONS", "HEAD" },
            () => "This is an options or head request "
        );

        app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");

        app.MapGet(
            "/todos/{id:int}/{text}/{slug:regex(^[a-z0-9_-]+$)}",
            (int id, string text, string slug) => $"int: {id}, text: {text}, slug: {slug}"
        );

        app.MapGet("/structured", () => new
        {
            Title = "JSON",
            Description = "Structured objects returned as json"
        });
        return app;
    }
}
