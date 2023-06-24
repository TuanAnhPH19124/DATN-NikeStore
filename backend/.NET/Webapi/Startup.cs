using Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using Service;
using Service.Abstractions;
using StackExchange.Redis;
using System.ComponentModel;
using WatchDog;
using WatchDog.src.Enums;

namespace Webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddElasticSearch(Configuration);

            services.AddWatchDogServices(option =>
            {
                option.IsAutoClear = false;
                option.SetExternalDbConnString = Configuration.GetConnectionString("DefaultConnection");
                option.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;
                
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies"; // Scheme mặc định (ví dụ: Cookie Authentication)
                options.DefaultChallengeScheme = "Google"; // Scheme để chuyển hướng đến khi xác thực thất bại (ví dụ: Google Authentication)
            })
            .AddCookie("Cookies") // Đăng ký Cookie Authentication Scheme
            .AddGoogle("Google", options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                options.Scope.Add("phone");
                options.Scope.Add("profile");
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webapi", Version = "v1" });
            });
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IRepositoryManger, RepositoryManager>();
            services.AddPersistence(Configuration);
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DatabaseMigration.StartMigration(app);

            app.UseCors(option =>
            {
                option.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webapi v1"));
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseWatchDogExceptionLogger();

            app.UseWatchDog(option =>
            {
                option.WatchPagePassword = "admin";
                option.WatchPageUsername = "admin";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
