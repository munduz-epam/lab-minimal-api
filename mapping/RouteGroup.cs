namespace Epam.Lab.MinimalApi;

internal static class RouteGroupExtensions
{
    internal static WebApplication MapRouteGroups(this WebApplication app)
    {
        var v1 = app.MapGroup("/v1");
        var v2 = app.MapGroup("/v2");

        v1.MapGet("/info", () => "version one");
        v2.MapGet("/info", () => "version two");

        var personGroup = v1.MapGroup("/person/{id:int}");
        personGroup.MapGet(
            "/child/{childName}",
            (int id, string childName) => $"{childName} is a child of person: {id}"
        ).WithName("child");

        personGroup.MapGet(
            "link",
            (string id, LinkGenerator linker) =>
            {
                var link = linker.GetPathByName("child", values: new {id, childName = "Murat"});
                return $"child link is: {link}";
            }
        );

        return app;
    }
}
