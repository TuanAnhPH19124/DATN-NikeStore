using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCarts>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShoppingCarts> builder)
        {
            builder.HasKey(x => new {x.Id});

            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.ShoppingCarts)
                .HasForeignKey(x => x.AppUserId);

            builder.HasMany(x => x.ShoppingCartItems)
            .WithOne(x => x.ShoppingCarts)
            .HasForeignKey(x => x.ShoppingCartId);
            
        }
    }
}