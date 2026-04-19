using Grupo28_JSON;
using Grupo28_JSON.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// Registo do cliente do ator externo.
// UseStub = true  -> usa o stub local (sem servico externo real)
// UseStub = false -> usa HttpClient real apontado para ExternalAuth:BaseUrl
var authConfig = builder.Configuration.GetSection("ExternalAuth");
var useStub = authConfig.GetValue<bool>("UseStub");

if (useStub)
{
    var simulateOffline = authConfig.GetValue<bool>("SimulateOffline");
    builder.Services.AddSingleton<IExternalAuthClient>(new StubExternalAuthClient(simulateOffline));
}
else
{
    var baseUrl = authConfig["BaseUrl"]!;
    builder.Services.AddHttpClient<IExternalAuthClient, ExternalAuthClient>(client =>
    {
        client.BaseAddress = new Uri(baseUrl);
    });
}

builder.Services.AddScoped<Model>();

var app = builder.Build();

app.MapControllers();

app.Run();
