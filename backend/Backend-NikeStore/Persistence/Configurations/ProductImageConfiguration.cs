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
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable(nameof(ProductImage));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ImageUrl).IsRequired();
            builder.Property(p => p.SetAsDefault).IsRequired(false);

            builder.HasOne(p => p.Color)
                   .WithMany(p => p.ProductImages)
                   .HasForeignKey(p => p.ColorId);

            builder.HasOne(p => p.Product)
                   .WithMany(p => p.ProductImages)
                   .HasForeignKey(p => p.ProductId);
        }
    }
}
