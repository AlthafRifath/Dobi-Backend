using Dobi.Application.Abstractions.Authentication;
using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Infrastructure.Authentication;
using Dobi.Infrastructure.Identity;
using Dobi.Infrastructure.Notifications;
using Dobi.Infrastructure.Persistence;
using Dobi.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dobi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
            }

            services.AddDbContext<DobiDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IDobiDbContext>(provider =>
                provider.GetRequiredService<DobiDbContext>());

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;

                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddEntityFrameworkStores<DobiDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ISmsSender, DevelopmentSmsSender>();

            return services;
        }
    }
}
