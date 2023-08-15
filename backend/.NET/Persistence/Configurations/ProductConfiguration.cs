using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.BarCode).IsRequired(false);
            builder.HasIndex(p => p.BarCode).IsUnique();
            builder.Property(p => p.Name).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.CostPrice).IsRequired();
            builder.Property(p => p.RetailPrice).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.ModifiedDate).IsRequired(false);

            builder.HasMany(p => p.Categories)
                   .WithMany(p => p.Products)
                   .UsingEntity(p => p.ToTable("ProductCategory"));

            builder.HasMany(p => p.Tags)
                   .WithMany(p => p.Products)
                   .UsingEntity(p => p.ToTable("ProductTag"));

            builder.HasMany(p => p.Stocks)
                   .WithOne(s => s.Product)
                   .HasForeignKey(s => s.ProductId);

            builder.HasMany(p => p.ProductImages)
                   .WithOne(p => p.Product)
                   .HasForeignKey(p => p.ProductId);
            builder.HasMany(p => p.OrderItems)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

            builder.HasMany(x => x.ShoppingCartItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

            builder.HasMany(p => p.ProductSoles)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            builder.HasMany(p => p.ProductMaterials)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
