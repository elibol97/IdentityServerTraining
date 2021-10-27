using IdentityManagement.Infrastructure.Persistance;
using IdentityManagement.Infrastructure.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
             {
                 options.User.RequireUniqueEmail = true;
                 options.Password.RequiredLength = 0;
                 options.Password.RequiredUniqueChars = 0;
                 options.Password.RequireDigit = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.User.AllowedUserNameCharacters = "qwertyuıopğüasdfghjklşizxcvbnmöç0123456789/*-+QWERTYUIOPĞÜASDFGHJKLŞİ,ZXCVBNMÖÇ.";
             })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    options.EnableTokenCleanup = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));          
                }).AddAspNetIdentity<AppUser>();
            return services;
        }       
        public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser : IdentityUser<int>,new()
        {
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
            return services;
        }
        
        
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<AppIdentityDbContext>(options=>options.UseNpgsql(connectionString));
            services.AddDbContext<AppPersistedGrantDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContext<AppConfigurationDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}
