using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(p => p.Orders)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(x => x.ShoppingCartItems)
                .WithOne(x => x.AppUser)
                .HasForeignKey(x => x.AppUserId);

            builder.HasMany(p => p.Addresses)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.UserId);
        }
    }
}
