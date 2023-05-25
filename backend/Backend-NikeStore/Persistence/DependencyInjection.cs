using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
    }
}
