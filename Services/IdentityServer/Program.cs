using IdentityServer;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(configs =>
{
    configs.UseInMemoryDatabase("Memory");
});

builder.Services.AddIdentity<AppUser, IdentityRole>(setup =>
{
   // setup.User.RequireUniqueEmail = true;

    setup.Password.RequireNonAlphanumeric = true;
    setup.Password.RequiredLength = 6;
    setup.Password.RequireUppercase = true;
    setup.Password.RequireLowercase = true;
    setup.Password.RequireDigit = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryIdentityResources(Configs.GetIdentityResources())
    .AddInMemoryApiScopes(Configs.GetApiScopes())
    .AddInMemoryClients(Configs.GetClients())
    .AddTestUsers(TestUsers.Users)
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication()
    .AddFacebook(configs =>
    {
        configs.AppId = Environment.GetEnvironmentVariable("FacebookAppId");
        configs.AppSecret = Environment.GetEnvironmentVariable("FacebookAppSecret");
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

await Seed.InitializeUsers(app.Services);

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
