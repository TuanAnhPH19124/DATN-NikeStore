using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItems>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShoppingCartItems> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ShoppingCartId).IsRequired();
            builder.HasOne(x => x.ShoppingCarts)
            .WithMany(x => x.ShoppingCartItems)
            .HasForeignKey(x => x.ShoppingCartId);
            builder.HasOne(x => x.Product)
            .WithMany(x => x.ShoppingCartItems)
            .HasForeignKey(x => x.ProductId);
        }
    }
}