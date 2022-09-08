using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public static class Seed
    {
        public static async Task InitializeUsers(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = new AppUser
            {
                UserName = "John",
            };

            var result = await userManager.CreateAsync(user, "Passw123$");
            if (!result.Succeeded)
            {
                Console.WriteLine(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}
