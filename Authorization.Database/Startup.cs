using System.Security.Claims;
using Authorization.Database.DbLayer;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authorization.Database
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>()
                .AddIdentity<AppUser, AppRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequiredLength = 3;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>();
            

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Admin/Login";
                config.AccessDeniedPath = "/Home/AccessDenied";
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                    builder =>
                    {
                        builder.RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "admin"));
                    });

                options.AddPolicy("manager", builder =>
                {
                    builder.RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "admin")
                                                        || context.User.HasClaim(ClaimTypes.Role, "manager"));
                });
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}