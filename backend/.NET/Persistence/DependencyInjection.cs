using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Configurations;
using StackExchange.Redis;
using System.Text;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext>(builder =>
            {
                var connection = configuration.GetConnectionString("DefaultConnection");
                builder.UseNpgsql(connection, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));

            services.AddIdentity<AppUser, IdentityRole>(option => option.SignIn.RequireConfirmedEmail = true)
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();
             



            //services.Configure<JwtConfigs>(configuration.GetSection("JwtConfig"));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {
                    var key = Encoding.ASCII.GetBytes(configuration.GetSection("JwtConfig:Secret").Value);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };


                });


        }
    }
}
