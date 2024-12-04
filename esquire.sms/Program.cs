using esquire.common;
using esquire.services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace esquire.sms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configSettings = builder.Configuration.GetSection("ConfigurationSettings");
            int expireMinutes = configSettings.GetValue<int>("AuthenticationMinutes");

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(ConfigurationSettings)).Get<ConfigurationSettings>());
            builder.Services.AddScoped<IGoogleService, GoogleService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped(typeof(IDBService<>), typeof(DBService<>));
            builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(expireMinutes);
                options.SlidingExpiration = true;
                options.LoginPath = "/Index";
                options.LogoutPath = "/Index";
                options.ReturnUrlParameter = "";
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login"; // Adjusted to match the new location
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
