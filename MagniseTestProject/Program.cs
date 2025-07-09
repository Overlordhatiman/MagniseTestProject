using MagniseTask.Data;
using MagniseTask.Interfaces;
using MagniseTask.Services.Common;
using MagniseTask.Services.RealTime;
using MagniseTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    AppDomain.CurrentDomain.GetAssemblies();
});

builder.Services.AddDbContext<MarketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

DotNetEnv.Env.Load();

builder.Services.Configure<UserCredentials>(config =>
{
    config.Username = Environment.GetEnvironmentVariable("FINTACHARTS_USERNAME");
    config.Password = Environment.GetEnvironmentVariable("FINTACHARTS_PASSWORD");
});

builder.Services.AddHttpClient("FintachartsClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FintachartsAPI:Uri"]!);
});

builder.Services.AddScoped<IFintachartsAuthService, FintachartsAuthService>();
builder.Services.AddScoped<IFintachartsDataService, FintachartsDataService>();
builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();
builder.Services.AddSingleton<WebSocketClientService>();
builder.Services.AddSingleton<MarketDataService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var marketService = scope.ServiceProvider.GetRequiredService<MarketDataService>();
    await marketService.StartAsync();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();