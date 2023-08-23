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
    public class CategoryProductsConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(p => new { p.ProductId, p.CategoryId });

            builder.HasOne(p => p.Category)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(p => p.CategoryId);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
