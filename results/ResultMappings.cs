namespace Epam.Lab.MinimalApi;

using  Microsoft.AspNetCore.Http.HttpResults;

internal static class ResultMappings
{
    internal static WebApplication MapResultExamples(this WebApplication app)
    {
        var resultGroup = app.MapGroup("/result");

        // infers the type and adds schema to openapi
        resultGroup.MapGet("person-json", () => new Person("Александр", 23));

        // no schema on openapi
        resultGroup.MapGet("person-not-typed", () => Results.Ok(new Person("Ulan", 18)));

        // openapi schema for type is added
        resultGroup.MapGet("person-typed", () => TypedResults.Ok(new Person("Oleg", 20)));

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-7.0#built-in-results
        resultGroup.MapGet(
            "person/permit/{age}",
            Results<BadRequest, NotFound, Ok<Person>>(int age) => {
                return age switch {
                    < 18 => TypedResults.BadRequest(),
                    > 65 => TypedResults.NotFound(),
                    _ => TypedResults.Ok(new Person("Altynai", 25))
                };
            }
        );

        // result as an object
        resultGroup.MapGet("html", () => new HtmlResult("<h1>HTML returned</h1>"));

        // stream result
        resultGroup.MapGet("kgz-info", async () => 
        {
            var proxyClient = new HttpClient();
            var stream = await proxyClient.GetStreamAsync("https://restcountries.com/v2/alpha?codes=kgz");
            // Proxy the response as JSON
            return Results.Stream(stream, "application/json");
        });

        return app;
    }
}