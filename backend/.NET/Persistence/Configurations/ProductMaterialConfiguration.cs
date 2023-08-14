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
    public class ProductMaterialConfiguration : IEntityTypeConfiguration<ProductMaterial>
    {
        public void Configure(EntityTypeBuilder<ProductMaterial> builder)
        {
            builder.HasKey(p => new { p.MaterialId, p.ProductId });

            builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductMaterials)
                .HasForeignKey(p => p.ProductId);

            builder.HasOne(p => p.Material)
                .WithMany(p => p.ProductMaterials)
                .HasForeignKey(p => p.MaterialId);
        }
    }
}
