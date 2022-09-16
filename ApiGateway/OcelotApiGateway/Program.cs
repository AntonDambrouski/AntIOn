using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
    {
        config.Authority = Environment.GetEnvironmentVariable(EnvironmentVariableNames.IdentityServerUrl);
        config.RequireHttpsMetadata = false;
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddOcelot();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Use(async (context, next) =>
{
    var host = context.Request.Host.ToUriComponent();
    context.Request.QueryString = context.Request.QueryString.Add("gateway_host", host);
    await next(context);
});

await app.UseOcelot();

app.Run();
