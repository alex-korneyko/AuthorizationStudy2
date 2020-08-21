using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authorization.Roles
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieScheme")
                .AddCookie("CookieScheme", config =>
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

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}