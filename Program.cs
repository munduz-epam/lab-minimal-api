using Epam.Lab.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();
builder.Services.AddDependencies();

var app = builder.Build();

var portEnvVar = Environment.GetEnvironmentVariable("EPAM_LAB_PORT");
if (!int.TryParse(portEnvVar, CultureInfo.InvariantCulture, out var port))
    port = 3000;

app.Urls.Add($"http://+:{port}");

app.UseSwagger();
app.UseSwaggerUI();

app.MapBasic();
app.MapDependenciesAndBindings();
app.MapRouteGroups();
app.MapResultExamples();
app.MapFilterMiddleware();

app.Run();