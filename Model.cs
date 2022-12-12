using System.Reflection;

interface Greeter
{
    string Hi(string name);
}

class CoolGreeter : Greeter
{
    public string Hi(string name) => $"Hi! {name}";
}

class GreetRequest
{
    public GreetRequest(Greeter greeter, string name)
    {
        Greeter = greeter;
        Name = name;
    }
    public Greeter Greeter { get; private set; }
    public string Name { get; private set; }
}

record Point(double X, double Y)
{
    public static bool TryParse(
        string? value,
        IFormatProvider? provider,
        out Point? point
    )
    {
        // Format is "(12.3,10.1)"
        var trimmedValue = value?.TrimStart('(').TrimEnd(')');
        var segments = trimmedValue?.Split(',');
        if (segments?.Length == 2
            && double.TryParse(segments[0], out var x)
            && double.TryParse(segments[1], out var y))
        {
            point = new Point(x, y);
            return true;
        }

        point = null;
        return false;
    }
}

record Paging(long Offset, int Size) {
    public static ValueTask<Paging> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        var request = httpContext.Request;
        var sizeStr = request.Query["page-size"];
        var offsetStr = request.Query["page-offset"];
        
        int.TryParse(sizeStr, CultureInfo.InvariantCulture, out var size);
        size = size == 0 ? 1 : size;
        
        long.TryParse(offsetStr, CultureInfo.InvariantCulture, out var offset);
        offset = offset == 0 ? 0 : offset;

        return ValueTask.FromResult(new Paging(offset, size));
    }
}

record Person(string Name, int Age);

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}
