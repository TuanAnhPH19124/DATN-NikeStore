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
    public class ProductSoleConfiguration : IEntityTypeConfiguration<ProductSole>
    {
        public void Configure(EntityTypeBuilder<ProductSole> builder)
        {
            builder.HasKey(p => new { p.SoleId, p.ProductId });

            builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductSoles)
                .HasForeignKey(p => p.ProductId);

            builder.HasOne(p => p.Sole)
                .WithMany(p => p.ProductSoles)
                .HasForeignKey(p => p.SoleId);
        }
    }
}
