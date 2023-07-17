using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DatabaseMigration
    {
        public static void StartMigration(IApplicationBuilder app)
        {
            using (var createScope = app.ApplicationServices.CreateScope())
            {
                createScope.ServiceProvider.GetService<AppDbContext>()?.Database.Migrate();
            }
        }
    }
}
