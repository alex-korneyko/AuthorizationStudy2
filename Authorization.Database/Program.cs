using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Database.DbLayer;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authorization.Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                DatabaseInitializer.Init(scope.ServiceProvider);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    public static class DatabaseInitializer
    {
        public static void Init(IServiceProvider scopeServiceProvider)
        {
            var userManager = scopeServiceProvider.GetService<UserManager<AppUser>>();

            var appUser = new AppUser
            {
                Name = "Alex", 
                UserName = "alex", 
                Surname = "Rootoff"
            };

            var identityResult = userManager.CreateAsync(appUser, "111").GetAwaiter().GetResult();

            if (identityResult.Succeeded)
            {
                userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.Role, "manager")).GetAwaiter().GetResult();
            }
        }
    }
}