using Grupo28_JSON;
using Grupo28_JSON.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var usersFilePath = Path.Combine(builder.Environment.ContentRootPath, "users.txt");
builder.Services.AddSingleton<IUserRepository>(new FileUserRepository(usersFilePath));
builder.Services.AddScoped<Model>();

var app = builder.Build();

app.UseCors("Angular");
app.MapControllers();
app.Run();
