using Grupo28_JSON;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.MapControllers();

app.Run();
