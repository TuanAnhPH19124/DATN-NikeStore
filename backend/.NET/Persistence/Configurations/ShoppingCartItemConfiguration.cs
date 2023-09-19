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
            builder.HasOne(p => p.Product)
            .WithMany(c => c.ShoppingCartItems)
            .HasForeignKey(c => c.ProductId);

             builder.HasOne(c => c.Color)
            .WithMany(c => c.ShoppingCartItems)
            .HasForeignKey(c => c.ColorId);

             builder.HasOne(s => s.Size)
            .WithMany(c => c.ShoppingCartItems)
            .HasForeignKey(c => c.SizeId);
        }
    }
}