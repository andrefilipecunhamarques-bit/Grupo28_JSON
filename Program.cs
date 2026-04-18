using Grupo28_JSON;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddSingleton<Model>();

var app = builder.Build();

app.MapControllers();

app.Run();
